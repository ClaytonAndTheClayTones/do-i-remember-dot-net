namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Labels;
using WebApi.Services;
using WebApi.Adapters;
using System.Linq;
using System.Reflection.Emit;

[ApiController]
[Route("[controller]")]
public class LabelsController : ControllerBase
{
    private ILabelService _labelService;
    private ILabelAdapter _labelAdapter;

    public LabelsController(ILabelService labelService, ILabelAdapter labelAdapter)
    {
        _labelService = labelService;
        _labelAdapter = labelAdapter;
    }


    [HttpPost]
    public async Task<IActionResult> Create(CreateLabelRequest model)
    {
        LabelModel label = await _labelService.Create(model);

        LabelResponseModel responseLabel = _labelAdapter.convertFromModelToResponseModel(label);

        return StatusCode(201, responseLabel);
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery]
        SearchLabelRequest request
    )
    {
        SearchLabelModel searchLabelModel = this._labelAdapter.convertFromRequestToSearchModel(request);

        IEnumerable<LabelModel> labels = await _labelService.Search(searchLabelModel);

        IEnumerable<LabelResponseModel> responseLabels = labels.Select(x =>
        {
            return _labelAdapter.convertFromModelToResponseModel(x);
        });

        return Ok(responseLabels);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        LabelModel? label = await _labelService.GetById(id);

        if (label == null)
        {
            return NotFound();
        }

        LabelResponseModel responseLabel = _labelAdapter.convertFromModelToResponseModel(label);

        return Ok(responseLabel);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateLabelRequest model)
    {
        LabelModel? label = await _labelService.Update(id, model);

        if (label == null)
        {
            return NotFound();
        }

        LabelResponseModel responseLabel = _labelAdapter.convertFromModelToResponseModel(label);

        return Ok(responseLabel);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        LabelModel? label = await _labelService.Delete(id);

        if (label == null)
        {
            return NotFound();
        }

        LabelResponseModel responseLabel = _labelAdapter.convertFromModelToResponseModel(label);

        return Ok(responseLabel);
    }
}