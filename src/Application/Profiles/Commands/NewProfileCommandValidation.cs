using FluentValidation;

namespace Application.Profiles.Commands
{
    public class NewProfileCommandValidation : AbstractValidator<NewProfileCommand>
    {
        public NewProfileCommandValidation()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Country).NotEmpty();
            RuleFor(x => x.Firstname).NotEmpty();
            RuleFor(x => x.Lastname).NotEmpty();
        }
    }
}