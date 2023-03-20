namespace DefaultNamespace;

public class Util
{
    public static char[,] PopulateMapFromFile(string path)
    {
        var lines = File.ReadAllLines(path);
        char[] validChars = { 'K', 'T', 'R', 'X' };

        char[,] map = new char[lines.GetLength(0), lines[0].Length];
        for (var i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Replace(Environment.NewLine, "").Replace(" ", "");
            for (var j = 0; j < line.Length; j++)
            {
                if (!validChars.Contains(line[j]))
                    throw new Exception("Invalid character in map file. Valid characters: K, T, R, X");
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