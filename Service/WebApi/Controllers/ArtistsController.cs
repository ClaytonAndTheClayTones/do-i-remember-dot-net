namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Artists;
using WebApi.Services;
using WebApi.Adapters;
using WebApi.Common;
using System.Linq;
using System.Reflection.Emit;
using WebApi.Adapters.ArtistAdapter;
using WebApi.Models.Common;
using WebApi.Adapters.Common;

[ApiController]
[Route("[controller]")]
public class ArtistsController : ControllerBase
{
    private IArtistService _artistService;
    private IArtistAdapter _artistAdapter;
    private IPagingAdapter _pagingAdapter;

    public ArtistsController(IArtistService labelService, IArtistAdapter labelAdapter, IPagingAdapter pagingAdapter)
    {
        _artistService = labelService;
        _artistAdapter = labelAdapter;
        _pagingAdapter = pagingAdapter;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateArtistRequest model)
    {
        ArtistModel label = await _artistService.Create(model);

        ArtistResponseModel responseArtist = _artistAdapter.convertFromModelToResponseModel(label);

        return StatusCode(201, responseArtist);
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery]
        SearchArtistRequest request,
        [FromQuery]
        PagingRequestInfo paging
    )
    {
        SearchArtistModel searchArtistModel = this._artistAdapter.convertFromRequestToSearchModel(request);

        PagingInfo pagingInfo = this._pagingAdapter.convertFromPagingRequestInfoToPagingInfo(paging);

        PagedList<ArtistModel> labels = await _artistService.Search(searchArtistModel, pagingInfo);

        PagedListResponse<ArtistResponseModel> responseModel = new PagedListResponse<ArtistResponseModel>();

        labels.Items.ForEach(x =>
        {
            responseModel.Items.Add(_artistAdapter.convertFromModelToResponseModel(x));
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
        ArtistModel? label = await _artistService.GetById(id);

        if (label == null)
        {
            return NotFound();
        }

        ArtistResponseModel responseArtist = _artistAdapter.convertFromModelToResponseModel(label);

        return Ok(responseArtist);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateArtistRequest model)
    {
        ArtistModel? label = await _artistService.Update(id, model);

        if (label == null)
        {
            return NotFound();
        }

        ArtistResponseModel responseArtist = _artistAdapter.convertFromModelToResponseModel(label);

        return Ok(responseArtist);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        ArtistModel? label = await _artistService.Delete(id);

        if (label == null)
        {
            return NotFound();
        }

        ArtistResponseModel responseArtist = _artistAdapter.convertFromModelToResponseModel(label);

        return Ok(responseArtist);
    }
}