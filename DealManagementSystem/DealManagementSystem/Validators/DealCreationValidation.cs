using DealManagementSystem.Models;
using FluentValidation;

namespace DealManagementSystem.Validators
{
    public class DealCreationValidation : AbstractValidator<Deals>
    {
        public DealCreationValidation()
        {
            RuleFor(deal => deal.Name)
                .NotEmpty().WithMessage("Deal name is required.")
                .MaximumLength(200).WithMessage("Deal name must not exceed 200 characters.");

            RuleFor(deal => deal.Slug)
                .NotEmpty().WithMessage("Slug is required.")
                .MaximumLength(200).WithMessage("Slug must not exceed 200 characters.");

            RuleFor(deal => deal.VideoUrl)
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Video URL must be a valid URL.")
                .When(deal => !string.IsNullOrEmpty(deal.VideoUrl));

            RuleFor(deal => deal.Hotels)
                .NotEmpty().WithMessage("At least one hotel is required.")
                .Must(hotels => hotels.All(hotel => hotel != null))
                .WithMessage("All hotels must be valid.");

            RuleFor(deal => deal.Itineraries)
                .NotEmpty().WithMessage("At least one itinerary is required.")
                .Must(itineraries => itineraries.All(itinerary => itinerary != null))
                .WithMessage("All itineraries must be valid.");
        }
    }
}
