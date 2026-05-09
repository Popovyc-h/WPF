using System.IO;
using System.Security.Cryptography;

namespace FileDuplicateChecker;

internal class FileDuplicateService
{
    public List<string> FindDuplicates(string path)
    {
        var originals = new List<string>();
        var files = Directory.GetFiles(path);

        var group = files.GroupBy(file =>
        {
            using var readStream = File.OpenRead(file);
            return Convert.ToHexString(MD5.HashData(readStream));
        });

        foreach (var g in group)
            originals.Add(g.MinBy(f => File.GetCreationTime(f)));

        return originals;
    }

    public List<string> MoveOriginalFiles(List<string> originals, string destinationPath)
    {
        var result = new List<string>();

        foreach (var original in originals)
        {
            var fileName = Path.GetFileName(original);
            var destination = Path.Combine(destinationPath, fileName);

            File.Move(original, destination);
            result.Add(fileName);
        }

        return result;
    }
}
