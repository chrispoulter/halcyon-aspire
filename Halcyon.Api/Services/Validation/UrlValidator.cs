namespace Halcyon.Api.Services.Validation;

using FluentValidation;

public static class UrlValidator
{
    public static IRuleBuilderOptions<T, string> Url<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .Must(url =>
            {
                if (url is null)
                {
                    return true;
                }

                return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                    && (
                        uriResult.Scheme == Uri.UriSchemeHttp
                        || uriResult.Scheme == Uri.UriSchemeHttps
                    );
            })
            .WithMessage("'{PropertyName}' is not a valid url.");
}
