namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Labels;
using WebApi.Services;

[ApiController]
[Route("[controller]")]
public class LabelsController : ControllerBase
{
    private ILabelService _labelService;

    public LabelsController(ILabelService labelService)
    { 
        _labelService = labelService; 
    }

    [HttpGet]
    public async Task<IActionResult> Search(SearchLabelRequest model)
    {
        var labels = await _labelService.Search(model);
        return Ok(labels);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var label = await _labelService.GetById(id);
        return Ok(label);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateLabelRequest model)
    {
        await _labelService.Create(model);
        return Ok(new { message = "Label created" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdateLabelRequest model)
    {
        await _labelService.Update(id, model);
        return Ok(new { message = "Label updated" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _labelService.Delete(id);
        return Ok(new { message = "Label deleted" });
    }
}