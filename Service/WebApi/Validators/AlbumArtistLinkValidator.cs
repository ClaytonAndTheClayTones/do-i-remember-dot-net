using FluentValidation;
using WebApi.Models.AlbumArtistLinks;
using WebApi.Validators.Common;

namespace WebApi.Validators
{  
    public class AlbumArtistLinkPostValidator : AbstractValidator<AlbumArtistLinkCreateRequest>
    {
        ICommonValidators _commonValidators;

        public AlbumArtistLinkPostValidator(ICommonValidators commonValidators)
        {
            this._commonValidators = commonValidators;

            RuleFor(x => x.AlbumId).NotNull().WithMessage(this._commonValidators.GenerateErrorMessage("AlbumId", ErrorMessageTypes.MissingRequiredField));
            RuleFor(x => x.AlbumId).Must(value => this._commonValidators.IsNullableGuid(value)).WithMessage(this._commonValidators.GenerateErrorMessage("AlbumId", ErrorMessageTypes.TypeError_Guid));

            RuleFor(x => x.ArtistId).NotNull().WithMessage(this._commonValidators.GenerateErrorMessage("ArtistId", ErrorMessageTypes.MissingRequiredField));
            RuleFor(x => x.ArtistId).Must(value => this._commonValidators.IsNullableGuid(value)).WithMessage(this._commonValidators.GenerateErrorMessage("ArtistId", ErrorMessageTypes.TypeError_Guid));
        } 
    } 
}