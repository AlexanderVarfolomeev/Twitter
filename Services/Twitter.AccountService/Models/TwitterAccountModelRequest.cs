using AutoMapper;
using FluentValidation;
using Twitter.Entities.Users;

namespace Twitter.AccountService.Models;

public class TwitterAccountModelRequest
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string? Patronymic { get; set; }
    public DateTime Birthday { get; set; }
    public string PageDescription { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}

public class TwitterAccountModelRequestValidator : AbstractValidator<TwitterAccountModelRequest>
{
    public TwitterAccountModelRequestValidator()
    {
        RuleFor(x => x.Birthday)
            .InclusiveBetween(new DateTime(1900, 1, 1), DateTime.Now)
            .WithMessage("Unsupported date.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Please enter correct email address.");

        RuleFor(x => x.Name)
            .MinimumLength(1)
            .MaximumLength(30)
            .WithMessage("The name must contain from 1 to 30 characters");

        RuleFor(x => x.Password)
            .MinimumLength(6)
            .MaximumLength(20)
            .WithMessage("The password must contain from 6 to 20 characters");
        
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$")
            .WithMessage("Uncorrected phone number.");

        RuleFor(x => x.UserName)
            .MinimumLength(5)
            .MaximumLength(20)
            .WithMessage("The username must contain from 5 to 20 characters");
    }
}


public class TwitterAccountModelRequestProfile : Profile
{
    public TwitterAccountModelRequestProfile()
    {
        CreateMap<TwitterAccountModelRequest, TwitterUser>();
    }
}