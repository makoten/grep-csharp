
static bool MatchPattern(string inputLine, string pattern)
{
    
    if (pattern.Length == 1)
    {
        return inputLine.Contains(pattern);
    }
    
    if (pattern.Contains(@"\d"))
    {
        return inputLine.Any(char.IsNumber);
    }

    if (pattern.Contains(@"\w"))
    {
        return inputLine.Any(char.IsLetterOrDigit);
    }

    if (HasNegativeCharacterGroups(pattern))
    {
        // TODO: That's a little too "happy path-ish" for me tbh
        var start = pattern.IndexOf('^') + 1;
        var end = pattern.IndexOf(']');
        var lookup = pattern[start..end];
        return !inputLine.Any(x => lookup.Contains(x, StringComparison.InvariantCultureIgnoreCase));
    }
    
    // I don't do return Has... because the alternative is an exception, not false
    if (HasPositiveCharacterGroups(pattern))
    {
        // TODO: That's a little too "happy path-ish" for me tbh
        var start = pattern.IndexOf('[') + 1;
        var end = pattern.IndexOf(']');
        var lookup = pattern[start..end];
        return inputLine.Any(x => lookup.Contains(x, StringComparison.InvariantCultureIgnoreCase));
    }

    
    
    throw new ArgumentException($"Unhandled pattern: {pattern}");
}

if (args[0] != "-E")
{
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}

string pattern = args[1];
string inputLine = Console.In.ReadToEnd();

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");

Environment.Exit(MatchPattern(inputLine, pattern) ? 0 : 1);

static bool HasPositiveCharacterGroups(string input)
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

static bool HasNegativeCharacterGroups(string input)
{
    if (HasPositiveCharacterGroups(input))
    {
        for (var i = 0; i < input.Length-1; i++)
        {
            if (input[i] == '[' && input[i + 1] == '^')
                return true;
        }
    }

    return false;
}