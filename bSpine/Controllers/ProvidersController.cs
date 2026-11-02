using bSpine.Models;
using bSpine.Services;
using Microsoft.AspNetCore.Mvc;

namespace bSpine.Controllers;

public class ProvidersController : Controller
{
    private const string CreateBind = "Name,Type,Specialty,Phone,Email,Address,City";
    private const string EditBind = "Id," + CreateBind;

    private readonly IProviderService _providers;

    public ProvidersController(IProviderService providers)
    {
        _providers = providers;
    }

    public async Task<IActionResult> Index(ProviderType? type, string? search)
    {
        ViewData["Type"] = type;
        ViewData["Search"] = search;
        var providers = await _providers.GetAllAsync(type, search);
        return View(providers);
    }

    public IActionResult Create()
    {
        return View(new Provider());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(CreateBind)] Provider provider)
    {
        if (!ModelState.IsValid)
        {
            return View(provider);
        }

        await _providers.CreateAsync(provider);
        TempData["Success"] = $"Provider {provider.Name} created.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var provider = await _providers.GetByIdAsync(id.Value);
        if (provider == null)
        {
            return NotFound();
        }

        return View(provider);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind(EditBind)] Provider provider)
    {
        if (id != provider.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(provider);
        }

        if (!await _providers.UpdateAsync(provider))
        {
            return NotFound();
        }

        TempData["Success"] = "Provider updated.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var provider = await _providers.GetByIdAsync(id.Value);
        if (provider == null)
        {
            return NotFound();
        }

        return View(provider);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var error = await _providers.DeleteAsync(id);
        if (error != null)
        {
            TempData["Error"] = error;
        }
        else
        {
            TempData["Success"] = "Provider deleted.";
        }

        return RedirectToAction(nameof(Index));
    }
}
