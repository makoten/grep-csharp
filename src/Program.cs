
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

    // I don't do return Has... because the alternative is an exception, not false
    if (HasPositiveCharacterGroups(pattern))
    {
        return true;
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
    var firstBracket = false;
    var secondBracket = false;
    foreach (var c in input)
    {
        switch (c)
        {
            case '[':
                firstBracket = true;
                break;
            case ']':
                secondBracket = true;
                break;
        }

        if (firstBracket && secondBracket)
            return true;
    }

    return false;
}