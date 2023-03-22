namespace DefaultNamespace;

public class Util
{
    public static char[,] PopulateMapFromFile(string path)
    {
        var lines = File.ReadAllLines(path);
        char[] validChars = { 'K', 'T', 'R', 'X' };
        var expectedLineLength = lines[0].Replace(Environment.NewLine, "").Replace(" ", "").Length;

        char[,] map = new char[lines.GetLength(0), expectedLineLength];
        for (var i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Replace(Environment.NewLine, "").Replace(" ", "");
            if (line.Length != expectedLineLength)
            {
                throw new Exception(
                    "Invalid map file. Inconsistent line length." + Environment.NewLine +
                    "Line: " + i + " Length: " + line.Length + Environment.NewLine +
                    "Expected Length: " + expectedLineLength
                    );
            }
            for (var j = 0; j < expectedLineLength; j++)
            {
                if (!validChars.Contains(line[j]))
                {
                    throw new Exception("Invalid character in map file. Valid characters: K, T, R, X");
                }
                map[i, j] = line.ElementAt(j);
            }
        }
        //for (var i = 0; i < lines.Length; i++)
        //{
        //    string line = lines[i].Replace(" ", "");
        //    for (var i1 = 0; i1 < line.Length; i1++)
        //    {
        //        map[i, i1] = line.ElementAt(i1);
        //    }
        //}
        return map;
    }
}