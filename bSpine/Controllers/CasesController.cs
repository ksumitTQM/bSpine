using bSpine.Models;
using bSpine.Models.ViewModels;
using bSpine.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bSpine.Controllers;

public class CasesController : Controller
{
    private const string CreateBind =
        "CaseNumber,ClientId,AssignedAttorney,Paralegal,Status,OpenedDate,StatuteOfLimitationsDate,Description";
    private const string EditBind = "Id," + CreateBind;

    private readonly ICaseService _cases;
    private readonly IClientService _clients;

    public CasesController(ICaseService cases, IClientService clients)
    {
        _cases = cases;
        _clients = clients;
    }

    public async Task<IActionResult> Index(string? search, CaseStatus? status)
    {
        var model = new CaseListViewModel
        {
            Search = search,
            Status = status,
            Cases = await _cases.SearchAsync(search, status)
        };
        return View(model);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var model = await _cases.GetDetailAsync(id.Value);
        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    public async Task<IActionResult> Create(int? clientId)
    {
        await LoadClientsAsync(clientId);
        return View(new LegalCase
        {
            ClientId = clientId ?? 0,
            OpenedDate = DateTime.Today,
            StatuteOfLimitationsDate = DateTime.Today.AddYears(2)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(CreateBind)] LegalCase legalCase)
    {
        if (!string.IsNullOrWhiteSpace(legalCase.CaseNumber) &&
            await _cases.CaseNumberExistsAsync(legalCase.CaseNumber))
        {
            ModelState.AddModelError(nameof(LegalCase.CaseNumber), "This case number is already in use.");
        }

        if (!ModelState.IsValid)
        {
            await LoadClientsAsync(legalCase.ClientId);
            return View(legalCase);
        }

        await _cases.CreateAsync(legalCase);
        TempData["Success"] = $"Case {legalCase.CaseNumber} created.";
        return RedirectToAction(nameof(Details), new { id = legalCase.Id });
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var legalCase = await _cases.GetByIdAsync(id.Value);
        if (legalCase == null)
        {
            return NotFound();
        }

        await LoadClientsAsync(legalCase.ClientId);
        return View(legalCase);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind(EditBind)] LegalCase legalCase)
    {
        if (id != legalCase.Id)
        {
            return NotFound();
        }

        if (!string.IsNullOrWhiteSpace(legalCase.CaseNumber) &&
            await _cases.CaseNumberExistsAsync(legalCase.CaseNumber, legalCase.Id))
        {
            ModelState.AddModelError(nameof(LegalCase.CaseNumber), "This case number is already in use.");
        }

        if (!ModelState.IsValid)
        {
            await LoadClientsAsync(legalCase.ClientId);
            return View(legalCase);
        }

        if (!await _cases.UpdateAsync(legalCase))
        {
            return NotFound();
        }

        TempData["Success"] = "Case updated.";
        return RedirectToAction(nameof(Details), new { id = legalCase.Id });
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var model = await _cases.GetDetailAsync(id.Value);
        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!await _cases.DeleteAsync(id))
        {
            return NotFound();
        }

        TempData["Success"] = "Case deleted.";
        return RedirectToAction(nameof(Index));
    }

    // ---- Add items from the case detail page ----

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTreatment(int id, [Bind(Prefix = "NewTreatment")] TreatmentInput input)
    {
        if (!ModelState.IsValid)
        {
            var model = await _cases.GetDetailAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            model.NewTreatment = input;
            return View("Details", model);
        }

        if (!await _cases.AddTreatmentAsync(id, input))
        {
            return NotFound();
        }

        TempData["Success"] = "Treatment added.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRequest(int id, [Bind(Prefix = "NewRequest")] RecordsRequestInput input)
    {
        if (!ModelState.IsValid)
        {
            var model = await _cases.GetDetailAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            model.NewRequest = input;
            return View("Details", model);
        }

        if (!await _cases.AddRequestAsync(id, input))
        {
            return NotFound();
        }

        TempData["Success"] = "Records request added.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddNote(int id, [Bind(Prefix = "NewNote")] NoteInput input)
    {
        if (!ModelState.IsValid)
        {
            var model = await _cases.GetDetailAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            model.NewNote = input;
            return View("Details", model);
        }

        if (!await _cases.AddNoteAsync(id, input))
        {
            return NotFound();
        }

        TempData["Success"] = "Note added.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkRequestReceived(int requestId, int caseId)
    {
        if (!await _cases.MarkRequestReceivedAsync(requestId))
        {
            return NotFound();
        }

        TempData["Success"] = "Records request marked as received.";
        return RedirectToAction(nameof(Details), new { id = caseId });
    }

    private async Task LoadClientsAsync(int? selectedId)
    {
        var clients = await _clients.GetAllAsync();
        ViewBag.Clients = new SelectList(clients, nameof(Client.Id), nameof(Client.FullName), selectedId);
    }
}
