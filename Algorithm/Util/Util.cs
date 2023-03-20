namespace DefaultNamespace;

public class Util
{
    public static char[,] PopulateMapFromFile(string path)
    {
        var lines = File.ReadAllLines(path);
        
        char[,] map = new char[lines.GetLength(0), lines[0].Length];
        for (var i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Replace(" ", "");
            for (var i1 = 0; i1 < line.Length; i1++)
            {
                map[i, i1] = line.ElementAt(i1);
            }
        }
        return map;
    }
}