using FluentValidation;
using WebApi.Models.Albums;
using WebApi.Validators.Common;

namespace WebApi.Validators
{  
    public class AlbumPostValidator : AbstractValidator<AlbumCreateRequest>
    {
        ICommonValidators _commonValidators;

        public AlbumPostValidator(ICommonValidators commonValidators)
        {
            this._commonValidators = commonValidators;

            RuleFor(x => x.Name).NotNull().WithMessage(this._commonValidators.GenerateErrorMessage("Name", ErrorMessageTypes.MissingRequiredField));

            RuleFor(x => x.LabelId).Must(value => value == null || this._commonValidators.IsGuid(value)).WithMessage(this._commonValidators.GenerateErrorMessage("LabelId", ErrorMessageTypes.TypeError_Guid));

            RuleFor(x => x.DateReleased).NotNull().WithMessage(this._commonValidators.GenerateErrorMessage("DateReleased", ErrorMessageTypes.MissingRequiredField));

            RuleFor(x => x.DateReleased).Must(value => this._commonValidators.IsDateOnly(value)).WithMessage(this._commonValidators.GenerateErrorMessage("DateReleased", ErrorMessageTypes.TypeError_DateOnly));
        } 
    } 
}