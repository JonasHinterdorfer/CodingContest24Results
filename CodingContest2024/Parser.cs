public class Parser
{
    private readonly string _inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Input");
    private int _currentIndex = 0;
    private readonly int _infoLinesStart;

    private List<Func<List<string>, object>> Parsers { get; }

    public Parser(List<Func<List<string>, object>> parsers, bool isTest = false, int infoLinesStart = 0)
    {
        if (isTest)
        {
            _inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Test_Input");
        }

        _infoLinesStart = infoLinesStart;
        Parsers = parsers;
    }

    public List<object> Go()
    {
        List<object> parsed = new();
        var input = ReadInputFile().GetEnumerator();

        List<string> infoLines = new();
        for (int i = 0; i < _infoLinesStart && input.MoveNext(); i++)
        {
            infoLines.Add(input.Current);
        }

        while (input.MoveNext())
        {
            foreach (Func<List<string>, object> parsing in Parsers)
            {
                List<string> lines = ProcessLines(input);
                if (Parsers.IndexOf(parsing) == 0)
                {
                    lines.InsertRange(0, infoLines);
                }
                parsed.Add(parsing.Invoke(lines));
            }
        }

        _currentIndex++;
        return parsed;
    }

    private IEnumerable<string> ReadInputFile()
    {
        return File.ReadLines((Directory.GetFiles(_inputFilePath))[_currentIndex]);
    }

    private List<string> ProcessLines(IEnumerator<string> input)
    {
        List<string> lines = new();
        string parameters = input.Current;
        int spaceIndex = parameters.IndexOf(' ');
        int numberOfLines = int.Parse(parameters.Substring(0, spaceIndex));
        string others = parameters.Substring(spaceIndex + 1);

        while (lines.Count < numberOfLines && input.MoveNext())
        {
            lines.Add(input.Current);
        }

        lines.Insert(0, others);
        return lines;
    }
}