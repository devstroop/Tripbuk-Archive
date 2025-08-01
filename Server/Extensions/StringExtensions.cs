using System.Text;

namespace Tripbuk.Server.Extensions;

public static class StringExtensions
{
    public static string GetPrefix(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty; // Return empty string for null or whitespace input.

        // Split input into words, removing extra spaces
        var words = input.Split([' '], StringSplitOptions.RemoveEmptyEntries);

        // Filter out non-alphanumeric characters in words
        var validWords = words
            .Select(word => new string(word.Where(char.IsLetterOrDigit).ToArray()))
            .Where(word => !string.IsNullOrEmpty(word))
            .ToList();

        if (validWords.Count == 0)
            return string.Empty;

        // Take the first character of up to the first two valid words
        string prefix;
        if (validWords.Count == 1)
        {
            // If only one valid word, take the first two characters (or fewer if the word is too short)
            prefix = validWords[0].Length >= 2
                ? validWords[0].Substring(0, 2)
                : validWords[0]; // Handle single-letter words gracefully
        }
        else
        {
            // Otherwise, take the first character of the first two words
            prefix = string.Concat(validWords.Take(2).Select(word => word[0]));
        }

        // Return the result in uppercase
        return prefix.ToUpperInvariant();
    }
    
    public static string ToKebabCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        StringBuilder result = new StringBuilder();
        result.Append(char.ToLower(input[0])); // Start with the first character in lowercase

        for (int i = 1; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]))
            {
                result.Append('-'); // Add a hyphen before uppercase letters
                result.Append(char.ToLower(input[i])); // Convert to lowercase
            }
            else
            {
                result.Append(input[i]); // Append lowercase letters as-is
            }
        }

        return result.ToString();
    }
}