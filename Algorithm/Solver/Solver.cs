using System.Security.Cryptography;

namespace DefaultNamespace;

public abstract class Solver<T>
{
    protected TreasureMap treasureMap;

    protected abstract Tuple<int, int> MergerItemGetter(T item);
    protected abstract bool IsFound(int idx1, int idx2, T item);
    public abstract Solution Solve(in bool tsp);

    protected void MergeSequenceWithDuplicateEnds(List<Tuple<int, int>> sequence, List<T> addition)
    {
        if (sequence.Count == 0)
        {
            foreach (var tuple in addition)
            {
                sequence.Add(MergerItemGetter(tuple));
            }
            return;
        }
        
        var (lastX, lastY) = sequence.Last(); var found = false;
        sequence.RemoveAt(sequence.Count-1);

        foreach (var tuple in addition)
        {
            found = found ? found : IsFound(lastX, lastY, tuple);
            if (!found) continue;
            sequence.Add(MergerItemGetter(tuple));
        }
    }
    
    protected static void CreateOrClearTrace<T>(T[,] trace, in char[,] map, T xValue, T nonXValue)
    {
        for (var i = 0; i < trace.GetLength(0); i += 1) 
        {
            for (var j = 0; j < trace.GetLength(1); j += 1)
            {
                if (map[i, j] == 'X')
                {
                    trace[i, j] = xValue;
                }
                else
                {
                    trace[i, j] = nonXValue;
                }
                
            }
        }
    }

    protected static List<char> TracePath(in List<Tuple<int, int>> sequence)
    {
        var pathList = new List<char>();
        for (var i = 0; i < sequence.Count - 1; ++i)
        {
            pathList.Add(GetDirection(sequence[i], sequence[i+1]));
        }
        return pathList;
    }
    
    private static char GetDirection(Tuple<int, int> initialPosition, Tuple<int, int> finalPosition)
    {
        int inX = initialPosition.Item1, inY = initialPosition.Item2,
            finX = finalPosition.Item1, finY = finalPosition.Item2;

        if (finX > inX)
        {
            return 'D';
        }

        if (finX < inX)
        {
            return 'U';
        }
        
        if (finY > inY)
        {
            return 'R';
        }

        if (finY < inY)
        {
            return 'L';
        }

        return 'S';
    }
    
}