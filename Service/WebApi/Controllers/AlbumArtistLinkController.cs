namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Models.AlbumArtistLinks; 
using WebApi.Adapters.AlbumArtistLinkAdapter;
using WebApi.Models.Common;
using WebApi.Adapters.Common;
using WebApi.Services;
using FluentValidation;
using FluentValidation.Results;
using WebApi.Validators;

[ApiController]
[Route("[controller]")]
public class AlbumArtistLinksController : ControllerBase
{
    private IAlbumArtistLinkService _albumAlbumArtistLinkLinkService;
    private IAlbumArtistLinkAdapter _albumAlbumArtistLinkLinkAdapter;
    private IPagingAdapter _pagingAdapter;
    private IValidator<AlbumArtistLinkCreateRequest> _albumArtistLinkValidator;

    public AlbumArtistLinksController(IValidator<AlbumArtistLinkCreateRequest> albumArtistLinkValidator, IAlbumArtistLinkService labelService, IAlbumArtistLinkAdapter labelAdapter, IPagingAdapter pagingAdapter)
    {
        _albumAlbumArtistLinkLinkService = labelService;
        _albumAlbumArtistLinkLinkAdapter = labelAdapter;
        _pagingAdapter = pagingAdapter;
        _albumArtistLinkValidator = albumArtistLinkValidator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(AlbumArtistLinkCreateRequest model)
    {
        ValidationResult results = this._albumArtistLinkValidator.Validate(model);

        if(!results.IsValid)
        {
            return StatusCode(400, new { Errors = results.Errors });
        }
         
        AlbumArtistLinkModel label = await _albumAlbumArtistLinkLinkService.Create(model);

        AlbumArtistLinkResponseModel responseAlbumArtistLink = _albumAlbumArtistLinkLinkAdapter.convertFromModelToResponseModel(label);

        return StatusCode(201, responseAlbumArtistLink);
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery]
        AlbumArtistLinkSearchRequest request,
        [FromQuery]
        PagingRequestInfo paging
    )
    {
        AlbumArtistLinkSearchModel searchAlbumArtistLinkModel = this._albumAlbumArtistLinkLinkAdapter.convertFromRequestToSearchModel(request);

        PagingInfo pagingInfo = this._pagingAdapter.convertFromPagingRequestInfoToPagingInfo(paging);

        PagedList<AlbumArtistLinkModel> labels = await _albumAlbumArtistLinkLinkService.Search(searchAlbumArtistLinkModel, pagingInfo);

        PagedListResponse<AlbumArtistLinkResponseModel> responseModel = new PagedListResponse<AlbumArtistLinkResponseModel>();

        labels.Items.ForEach(x =>
        {
            responseModel.Items.Add(_albumAlbumArtistLinkLinkAdapter.convertFromModelToResponseModel(x));
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
        AlbumArtistLinkModel? label = await _albumAlbumArtistLinkLinkService.GetById(id);

        if (label == null)
        {
            return NotFound();
        }

        AlbumArtistLinkResponseModel responseAlbumArtistLink = _albumAlbumArtistLinkLinkAdapter.convertFromModelToResponseModel(label);

        return Ok(responseAlbumArtistLink);
    } 

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        AlbumArtistLinkModel? label = await _albumAlbumArtistLinkLinkService.Delete(id);

        if (label == null)
        {
            return NotFound();
        }

        AlbumArtistLinkResponseModel responseAlbumArtistLink = _albumAlbumArtistLinkLinkAdapter.convertFromModelToResponseModel(label);

        return Ok(responseAlbumArtistLink);
    }
}