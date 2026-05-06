namespace TextAnalyzerApp;

internal class TextAnalyzer
{
    private char[] SentenceEnders = { '.', '?', '!' };
    public int sentenceCount { get; private set; }
    public int charCount { get; private set; }
    public int wordCount { get; private set; }
    public int questionCount { get; private set; }
    public int exclamationCount { get; private set; }

    public void Analyze(string str)
    {
        charCount = str.Length;
        string[] words = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        wordCount = words.Length;

        foreach (var c in str)
        {
            if (!SentenceEnders.Contains(c)) continue;

            sentenceCount++;
            if (c == '?') questionCount++;
            if (c == '!') exclamationCount++;
        }
    }

    public override string ToString()
    {
        return 
            $"Кількість речень: {sentenceCount}" +
            $"\nКількість символів: {charCount}" +
            $"\nКількість слів: {wordCount}" +
            $"\nКількість питальних речень: {questionCount}" +
            $"\nКількість окличних речень: {exclamationCount}";
    }
}
