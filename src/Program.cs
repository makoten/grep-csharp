static bool MatchPattern(string input, string pattern)
{
    if (pattern.Length == 1)
        return input.Contains(pattern);
    var inputIdx = 0;
    var matchFound = false;
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
    // edge case for strict start of string
    if (pattern[0] == '^')
        return Matcher(input, pattern, 1);
    while (inputIdx < input.Length)
    {
        matchFound = Matcher(input, pattern, inputIdx);
        if (matchFound)
            return true;
        
        inputIdx++;
    }
    
    return false;
    
    bool Matcher(string input, string pattern, int i)
    {
        var patternIdx = 0;
        while (patternIdx < pattern.Length)
        {
            // ensure that we're not at the end of the input
            if (i == input.Length)
                return false;
            
            if (pattern[patternIdx] == '\\')
            {
                patternIdx++;
                if (pattern[patternIdx] == 'w' && !(char.IsLetterOrDigit(input[i]) || input[i] == '_') || 
                    pattern[patternIdx] == 'd' && !char.IsNumber(input[i]))
                    return false;
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