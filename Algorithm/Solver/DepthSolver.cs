namespace DefaultNamespace;

public class DepthSolver : Solver<Tuple<int, int>>
{

    private bool dfs(in char[,] map, bool[,] visited, 
                            in int idx1, in int idx2, ref int nodesCheckedCount,
                            HashSet<int> treasureSet, List<Tuple<int, int>> sequence)
    {
        ++nodesCheckedCount;
        visited[idx1, idx2] = true;
        int setIndex = idx1 * map.GetLength(0) + idx2;

        if (map[idx1, idx2] == 'T' && !treasureSet.Contains(setIndex))
        {
            sequence.Insert(0, Tuple.Create(idx1, idx2));
            treasureSet.Add(setIndex);
            return true;
        }

        var res = false;

        if (idx2 - 1 >= 0 && !visited[idx1, idx2 - 1]) 
            res = dfs(map, visited, idx1, idx2 - 1, ref nodesCheckedCount, treasureSet, sequence);

        if (idx2 + 1 < map.GetLength(1) && !visited[idx1, idx2 + 1] && !res)
            res = dfs(map, visited, idx1, idx2+1, ref nodesCheckedCount, treasureSet, sequence);

        if (idx1 - 1 >= 0 && !visited[idx1 - 1, idx2] && !res)
            res = dfs(map, visited, idx1-1, idx2, ref nodesCheckedCount, treasureSet, sequence);

        if (idx1 + 1 < map.GetLength(0) && !visited[idx1 + 1, idx2] && !res) 
            res = dfs(map, visited, idx1+1, idx2, ref nodesCheckedCount, treasureSet, sequence);
        
        if (res)
        {
            var tuple = Tuple.Create(idx1, idx2);
            sequence.Insert(0, tuple);
        }
        
        return res;
    }

    protected override Tuple<int, int> MergerItemGetter(Tuple<int, int> item)
    {
        return item;
    }

    protected override bool IsFound(int idx1, int idx2, Tuple<int, int> item)
    {
        var (x, y) = item;
        if (idx1 == x && idx2 == y) return true;
        return false;
    }

    public override Solution Solve(in bool tsp)
    {
        var map = treasureMap.MapArr;
        var (startIdx1, startIdx2) = treasureMap.StartPoint; 
        var treasureCount = treasureMap.TreasureCount;
        
        List<Tuple<int, int>> sequence = new(), tempSequence = new();
        var visited = new bool[map.GetLength(0), map.GetLength(1)];
        int treasureFound = 0, idx1 = startIdx1, idx2 = startIdx2, nodesCheckedCount = 0;
        var treasureSet = new HashSet<int>();

        var startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        while (treasureCount != treasureFound)
        {
            var avail = dfs(map, visited, idx1, idx2, ref nodesCheckedCount, treasureSet, tempSequence);
            
            if (avail)
            {
                ++treasureFound;
                // merge list
                MergeSequenceWithDuplicateEnds(sequence, tempSequence);
                // update starting x and y  
                (idx1, idx2) = tempSequence.Last();
            }
            
            // clear visited
            CreateOrClearTrace(visited, map, true, false);
            
            if (tsp && treasureCount == treasureFound)
            {
                if (map[startIdx1, startIdx2] == 'T')
                {
                    break;
                }
                --treasureFound;
                map[startIdx1, startIdx2] = 'T';
            }
            
            // clear temp
            tempSequence.Clear();
        }
        
        var endTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        
        if (tsp)
        {
            map[startIdx1, startIdx2] = 'K';
        }
        
        return new Solution
        {
            Path = TracePath(sequence), 
            Sequence = sequence,
            ExecutionTime = endTime-startTime, 
            NodesCheckedCount = nodesCheckedCount
        };
    }

}