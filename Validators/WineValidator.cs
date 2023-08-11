namespace IntroLab.Validators
{
    public class WineValidator : AbstractValidator<Wines>
    {
        public WineValidator()
        {
            //ctor generates a constructor
            RuleFor(w => w.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(w => w.Year).GreaterThan(1900).LessThan(2023).WithMessage("Year is required or invalid");
        }
    }
}
