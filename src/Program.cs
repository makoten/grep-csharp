
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
