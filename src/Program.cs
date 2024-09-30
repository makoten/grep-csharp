static bool MatchPattern(string inputLine, string pattern)
{
    if (pattern.Length == 1)
        return inputLine.Contains(pattern);

    var patternIdx = 0;
    foreach (var currentChar in inputLine)
    {
        // if it's the end of the pattern but there are still characters in the input string

        if (pattern[patternIdx] == '\\')
        {
            patternIdx++;
            if ((pattern[patternIdx] == 'w' && !(char.IsLetterOrDigit(currentChar) || currentChar == '_'))
                || (pattern[patternIdx] == 'd' && !char.IsNumber(currentChar)))
                return false;
        }
        else if (currentChar != pattern[patternIdx])
        {
            return false;
        }

        patternIdx++;
        if (patternIdx == pattern.Length)
            return true;
    }

    return true;
    
   
    //  Keeping this old code around won't hurt Clueless
    // if (HasNegativeCharacterGroups(pattern))
    // {
    //     var start = pattern.IndexOf('^') + 1;
    //     var end = pattern.IndexOf(']');
    //     var lookup = pattern[start..end];
    //     return !inputLine.Any(x => lookup.Contains(x, StringComparison.InvariantCulture));
    // }
    //
    // if (HasPositiveCharacterGroups(pattern))
    // {
    //     var start = pattern.IndexOf('[') + 1;
    //     var end = pattern.IndexOf(']');
    //     var lookup = pattern[start..end];
    //     return inputLine.Any(x => lookup.Contains(x, StringComparison.InvariantCulture));
    // }

    // throw new ArgumentException($"Unhandled pattern: {pattern}");
}

if (args[0] != "-E")
{
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}

// var pattern = @"\d apple";
// var inputLine = "sally has 1 orange";

string pattern = args[1];
string inputLine = Console.In.ReadToEnd();

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");

// Console.WriteLine(MatchPattern(inputLine, pattern) ? "match" : "no match!");
Environment.Exit(MatchPattern(inputLine, pattern) ? 0 : 1);

// static bool HasPositiveCharacterGroups(string input)
// {
//     var openingBracket = false;
//     var fullBrackets = false;
//     foreach (var c in input)
//     {
//         switch (c)
//         {
//             case '[':
//                 openingBracket = true;
//                 break;
//             case ']':
//                 if (openingBracket)
//                     fullBrackets = true;
//                 break;
//         }
//
//         if (fullBrackets)
//             return true;
//     }
//
//     return false;
// }
//
// static bool HasNegativeCharacterGroups(string input)
// {
//     if (HasPositiveCharacterGroups(input))
//     {
//         for (var i = 0; i < input.Length-1; i++)
//         {
//             if (input[i] == '[' && input[i + 1] == '^')
//                 return true;
//         }
//     }
//
//     return false;
// }