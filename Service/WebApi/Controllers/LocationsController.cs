namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Locations;
using WebApi.Services;
using WebApi.Adapters.LocationAdapter;
using WebApi.Models.Common;
using WebApi.Adapters.Common;

[ApiController]
[Route("[controller]")]
public class LocationsController : ControllerBase
{
    private ILocationService _locationService;
    private ILocationAdapter _locationAdapter;
    private IPagingAdapter _pagingAdapter;

    public LocationsController(ILocationService locationService, ILocationAdapter locationAdapter, IPagingAdapter pagingAdapter)
    {
        _locationService = locationService;
        _locationAdapter = locationAdapter;
        _pagingAdapter = pagingAdapter;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateLocationRequest model)
    {
        LocationModel location = await _locationService.Create(model);

        LocationResponseModel responseLocation = _locationAdapter.convertFromModelToResponseModel(location);

        return StatusCode(201, responseLocation);
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery]
        SearchLocationRequest request,
        [FromQuery]
        PagingRequestInfo paging
    )
    {
        SearchLocationModel searchLocationModel = this._locationAdapter.convertFromRequestToSearchModel(request);

        PagingInfo pagingInfo = this._pagingAdapter.convertFromPagingRequestInfoToPagingInfo(paging);

        PagedList<LocationModel> locations = await _locationService.Search(searchLocationModel, pagingInfo);

        PagedListResponse<LocationResponseModel> responseModel = new PagedListResponse<LocationResponseModel>();

        locations.Items.ForEach(x =>
        {
            responseModel.Items.Add(_locationAdapter.convertFromModelToResponseModel(x));
        });

        if (locations.PagingInfo != null)
        {
            responseModel.PagingInfo = _pagingAdapter.convertFromPagingResultInfoToPagingResponseInfo(locations.PagingInfo);
        }

        return Ok(responseModel);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        LocationModel? location = await _locationService.GetById(id);

        if (location == null)
        {
            return NotFound();
        }

        LocationResponseModel responseLocation = _locationAdapter.convertFromModelToResponseModel(location);

        return Ok(responseLocation);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateLocationRequest model)
    {
        LocationModel? location = await _locationService.Update(id, model);

        if (location == null)
        {
            return NotFound();
        }

        LocationResponseModel responseLocation = _locationAdapter.convertFromModelToResponseModel(location);

        return Ok(responseLocation);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        LocationModel? location = await _locationService.Delete(id);

        if (location == null)
        {
            return NotFound();
        }

        LocationResponseModel responseLocation = _locationAdapter.convertFromModelToResponseModel(location);

        return Ok(responseLocation);
    }
}