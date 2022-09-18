using System.Text.RegularExpressions;

namespace Common.Helpers.ReferenceNumberHelper;

public static class ReferenceNumberGenerator
{
    public static string Generate()
    {
        return string.Concat("REF", Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", ""));
    }
}