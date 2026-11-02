using bSpine.Models;
using bSpine.Services;
using Microsoft.AspNetCore.Mvc;

namespace bSpine.Controllers;

public class ClientsController : Controller
{
    private const string CreateBind = "FirstName,LastName,DateOfBirth,Phone,Email,Address,InjuryDate,InjuryType,InjurySummary";
    private const string EditBind = "Id," + CreateBind;

    private readonly IClientService _clients;

    public ClientsController(IClientService clients)
    {
        _clients = clients;
    }

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["Search"] = search;
        var clients = await _clients.GetAllAsync(search);
        return View(clients);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = await _clients.GetByIdAsync(id.Value);
        if (client == null)
        {
            return NotFound();
        }

        return View(client);
    }

    public IActionResult Create()
    {
        return View(new Client
        {
            DateOfBirth = DateTime.Today.AddYears(-40),
            InjuryDate = DateTime.Today
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(CreateBind)] Client client)
    {
        if (!ModelState.IsValid)
        {
            return View(client);
        }

        await _clients.CreateAsync(client);
        TempData["Success"] = $"Client {client.FullName} created.";
        return RedirectToAction(nameof(Details), new { id = client.Id });
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = await _clients.GetByIdAsync(id.Value);
        if (client == null)
        {
            return NotFound();
        }

        return View(client);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind(EditBind)] Client client)
    {
        if (id != client.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(client);
        }

        if (!await _clients.UpdateAsync(client))
        {
            return NotFound();
        }

        TempData["Success"] = "Client updated.";
        return RedirectToAction(nameof(Details), new { id = client.Id });
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = await _clients.GetByIdAsync(id.Value);
        if (client == null)
        {
            return NotFound();
        }

        return View(client);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var error = await _clients.DeleteAsync(id);
        if (error != null)
        {
            TempData["Error"] = error;
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = "Client deleted.";
        return RedirectToAction(nameof(Index));
    }
}
