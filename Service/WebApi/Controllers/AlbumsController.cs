namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Albums; 
using WebApi.Adapters.AlbumAdapter;
using WebApi.Models.Common;
using WebApi.Adapters.Common;
using WebApi.Services;
using FluentValidation;
using FluentValidation.Results;

[ApiController]
[Route("[controller]")]
public class AlbumsController : ControllerBase
{
    private IAlbumService _albumService;
    private IAlbumAdapter _albumAdapter;
    private IPagingAdapter _pagingAdapter;
    private IValidator<AlbumCreateRequest> _albumValidator;


    public AlbumsController(IValidator<AlbumCreateRequest> albumValidator, IAlbumService labelService, IAlbumAdapter labelAdapter, IPagingAdapter pagingAdapter)
    {
        _albumService = labelService;
        _albumAdapter = labelAdapter;
        _pagingAdapter = pagingAdapter;
        _albumValidator = albumValidator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(AlbumCreateRequest model)
    {
        ValidationResult results = this._albumValidator.Validate(model);

        if (!results.IsValid)
        {
            return StatusCode(400, new { Errors = results.Errors });
        }

        AlbumModel label = await _albumService.Create(model);

        AlbumResponseModel responseAlbum = _albumAdapter.convertFromModelToResponseModel(label);

        return StatusCode(201, responseAlbum);
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery]
        AlbumSearchRequest request,
        [FromQuery]
        PagingRequestInfo paging
    )
    {
        AlbumSearchModel searchAlbumModel = this._albumAdapter.convertFromRequestToSearchModel(request);

        PagingInfo pagingInfo = this._pagingAdapter.convertFromPagingRequestInfoToPagingInfo(paging);

        PagedList<AlbumModel> labels = await _albumService.Search(searchAlbumModel, pagingInfo);

        PagedListResponse<AlbumResponseModel> responseModel = new PagedListResponse<AlbumResponseModel>();

        labels.Items.ForEach(x =>
        {
            responseModel.Items.Add(_albumAdapter.convertFromModelToResponseModel(x));
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
        AlbumModel? label = await _albumService.GetById(id);

        if (label == null)
        {
            return NotFound();
        }

        AlbumResponseModel responseAlbum = _albumAdapter.convertFromModelToResponseModel(label);

        return Ok(responseAlbum);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id, AlbumUpdateRequest model)
    {
        AlbumModel? label = await _albumService.Update(id, model);

        if (label == null)
        {
            return NotFound();
        }

        AlbumResponseModel responseAlbum = _albumAdapter.convertFromModelToResponseModel(label);

        return Ok(responseAlbum);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        AlbumModel? label = await _albumService.Delete(id);

        if (label == null)
        {
            return NotFound();
        }

        AlbumResponseModel responseAlbum = _albumAdapter.convertFromModelToResponseModel(label);

        return Ok(responseAlbum);
    }
}