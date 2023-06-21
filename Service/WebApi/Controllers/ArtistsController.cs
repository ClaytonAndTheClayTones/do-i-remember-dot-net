namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Labels;
using WebApi.Services;
using WebApi.Adapters;
using WebApi.Common;
using System.Linq;
using System.Reflection.Emit;
using WebApi.Adapters.LabelAdapter;
using WebApi.Models.Common;
using WebApi.Adapters.Common;

[ApiController]
[Route("[controller]")]
public class LabelsController : ControllerBase
{
    private ILabelService _labelService;
    private ILabelAdapter _labelAdapter;
    private IPagingAdapter _pagingAdapter;

    public LabelsController(ILabelService labelService, ILabelAdapter labelAdapter, IPagingAdapter pagingAdapter)
    {
        _labelService = labelService;
        _labelAdapter = labelAdapter;
        _pagingAdapter = pagingAdapter;
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
        SearchLabelRequest request,
        [FromQuery]
        PagingRequestInfo paging
    )
    {
        SearchLabelModel searchLabelModel = this._labelAdapter.convertFromRequestToSearchModel(request);

        PagingInfo pagingInfo = this._pagingAdapter.convertFromPagingRequestInfoToPagingInfo(paging);

        PagedList<LabelModel> labels = await _labelService.Search(searchLabelModel, pagingInfo);

        PagedListResponse<LabelResponseModel> responseModel = new PagedListResponse<LabelResponseModel>();

        labels.Items.ForEach(x =>
        {
            responseModel.Items.Add(_labelAdapter.convertFromModelToResponseModel(x));
        });

        if (labels.PagingInfo != null)
        {
            responseModel.PagingInfo = _pagingAdapter.convertFromPagingResultInfoToPagingResponseInfo(labels.PagingInfo);
        }

        return Ok(responseModel);
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