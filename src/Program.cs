if (args[0] != "-E")
{
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}

var pattern = args[1];
var inputLine = Console.In.ReadToEnd();

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");

var matcher = new PatternMatcher(pattern);
Environment.Exit(matcher.MatchWithInput(inputLine) ? 0 : 1);

/// <summary>
///     This class provides the functionality to pattern match various inputs
///     with a specific regular expression that must be provided at object creation.
///     It doesn't use Regex built-in methods, that would make the class a simple wrapper and bloat.
/// </summary>
/// <param name="pattern">The regular expression that we'll be used to match</param>
internal class PatternMatcher(string pattern)
{
    public bool MatchWithInput(string input)
    {
        if (pattern.Length == 1)
            return input.Contains(pattern);

        if (HasNegativeCharacterGroups(pattern))
        {
            var start = pattern.IndexOf('^') + 1;
            var end = pattern.IndexOf(']');
            var lookup = pattern[start..end];
            return !input.Any(x => lookup.Contains(x, StringComparison.InvariantCulture));
        }

        if (HasPositiveCharacterGroups(pattern))
        {
            var start = pattern.IndexOf('[') + 1;
            var end = pattern.IndexOf(']');
            var lookup = pattern[start..end];
            return input.Any(x => lookup.Contains(x, StringComparison.InvariantCulture));
        }

        var inputIdx = 0;
        var matchFound = false;
        if (pattern[0] == '^')
            return Matcher(input, inputIdx);

        while (inputIdx < input.Length)
        {
            matchFound = Matcher(input, inputIdx);
            if (matchFound)
                return true;

            inputIdx++;
        }

        return false;
    }

    /// <summary>
    ///     This method checks whether a substring of the current input
    ///     satisfies the encapsulated pattern.
    /// </summary>
    /// <param name="input">the provided input</param>
    /// <param name="i">the index used to read characters from the input</param>
    /// <returns>True if the input satisfies the pattern, false if the pattern fails at any point.</returns>
    private bool Matcher(string input, int i)
    {
        // edge case for strict string start
        var patternIdx = pattern[0] == '^' ? 1 : 0;
        while (patternIdx < pattern.Length)
        {
            // ensure that we're not at the end of the input
            if (i == input.Length)
                // handle the end of line pattern match
                return pattern[patternIdx] == '$';

            if (pattern[patternIdx] == '\\')
            {
                patternIdx++;
                if ((pattern[patternIdx] == 'w' && !(char.IsLetterOrDigit(input[i]) || input[i] == '_')) ||
                    (pattern[patternIdx] == 'd' && !char.IsNumber(input[i])))
                    return false;
            }
            else if (pattern[patternIdx] == '+')
            {
                var repeatingLetter = pattern[patternIdx - 1];
                while (input[i] == repeatingLetter && i < input.Length) i++;

                // correcting offset input index due to repeated character matching
                i--;
            }
            else if (pattern[patternIdx] != input[i])
            {
                return false;
            }


            patternIdx++;
            i++;
        }

        return true;
    }

    private bool HasPositiveCharacterGroups(string input)
    {
        var openingBracket = false;
        var fullBrackets = false;
        foreach (var c in input)
        {
            switch (c)
            {
                case '[':
                    openingBracket = true;
                    break;
                case ']':
                    if (openingBracket)
                        fullBrackets = true;
                    break;
            }

            if (fullBrackets)
                return true;
        }

        return false;
    }

    private bool HasNegativeCharacterGroups(string input)
    {
        for (var i = 0; i < input.Length - 1; i++)
            if (input[i] == '[' && input[i + 1] == '^')
                return true;

        return false;
    }
}