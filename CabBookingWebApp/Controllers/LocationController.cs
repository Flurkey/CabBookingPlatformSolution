using CabBookingWebApp.Models;
using CabBookingWebApp.Services;
using Microsoft.AspNetCore.Mvc;

public class LocationController : Controller
{
    private readonly LocationApiService _service;

    public LocationController(LocationApiService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var userEmail = HttpContext.Session.GetString("userEmail");
        if (string.IsNullOrEmpty(userEmail)) return RedirectToAction("Login", "Customer");

        var locations = await _service.GetLocationsAsync(userEmail);

        foreach (var loc in locations)
        {
            loc.Weather = await _service.GetWeatherAsync(loc.City);
        }

        return View(locations);
    }


    public IActionResult Add() => View();

    [HttpPost]
    public async Task<IActionResult> Add(Location loc)
    {
        loc.UserId = HttpContext.Session.GetString("userEmail");
        await _service.AddLocationAsync(loc);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.DeleteLocationAsync(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Weather(string city)
    {
        var result = await _service.GetWeatherAsync(city);
        return View(model: result);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var location = await _service.GetByIdAsync(id);
        if (location == null) return NotFound();
        return View(location);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Location updated)
    {
        if (!ModelState.IsValid)
            return View(updated);

        await _service.UpdateAsync(updated.Id, updated);
        return RedirectToAction("Index");
    }

}
