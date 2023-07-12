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

            RuleFor(x => x.ArtistId).NotNull();
            RuleFor(x => x.ArtistId).Must(value => this._commonValidators.IsNullableGuid(value));
            RuleFor(x => x.AlbumId).NotNull();
            RuleFor(x => x.AlbumId).Must(value => this._commonValidators.IsNullableGuid(value)); 
        } 
    }
}

