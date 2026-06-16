using System.Diagnostics.CodeAnalysis;

namespace PlanShare.Domain.Extensions;
public static class StringExtension
{
    public static bool NotEmpty([NotNullWhen(returnValue: true)] this string? value) => string.IsNullOrWhiteSpace(value: value).IsFalse();
}