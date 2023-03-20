namespace DefaultNamespace;


public class BreadthSolver : Solver<Tuple<bool, int, int>>
{

    // unfortunately tuple is immutable, so we have to create a new tuple to sign the sequence
    private static List<Tuple<bool, int, int>> TraceSequence(Tuple<bool, int, int>[,] dynMap, 
                                                                in List<Tuple<bool, int, int>> sequence, int startIdx)
    {
        int x = sequence[startIdx-1].Item2, y = sequence[startIdx-1].Item3;
        var sequenceN = new List<Tuple<bool, int, int>>();

        for (var i = startIdx - 1; i >= 0; --i)
        {
            int currentX = sequence[i].Item2, currentY = sequence[i].Item3;
            
            if (currentX != x || currentY != y) continue;
            sequenceN.Insert(0, sequence[i]);
            var tuple = dynMap[x, y];
            x = tuple.Item2; y = tuple.Item3;
        }

        return sequenceN;
    }

    protected override Tuple<int, int> MergerItemGetter(Tuple<bool, int, int> item)
    {
        return Tuple.Create(item.Item2, item.Item3);
    }

    protected override bool IsFound(int idx1, int idx2, Tuple<bool, int, int> item)
    {
        var (_, x, y) = item;
        if (idx1 == x && idx2 == y)
        { 
            return true;
        }
        return false;
    }

    public override Solution Solve(in bool tsp)
    {
        var map = TreasureMap.MapArr;
        var (startIdx1, startIdx2) = TreasureMap.StartPoint; 
        var treasureCount = TreasureMap.TreasureCount;
        var size = map.GetLength(0);
        
        var trace = new Tuple<bool, int, int>[map.GetLength(0), map.GetLength(1)];
        CreateOrClearTrace(trace, map, Tuple.Create(true, 0, 0), Tuple.Create(false, 0, 0));
        
        var sequence = new List<Tuple<bool, int, int>>();
        var finalSequence = new List<Tuple<int, int>>();
        var treasureSet = new HashSet<int>();
        
        sequence.Add(Tuple.Create(false, startIdx1, startIdx2));
        trace[0, 0] = Tuple.Create(true, trace[0,0].Item2, trace[0,0].Item3);
        int treasureFound = 0, leftIdx = 0, rightIdx = 1, nodesCheckedCount = 0;
        
        var startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        
        while (leftIdx < rightIdx)
        {
            ++nodesCheckedCount;
            var (_, idx1, idx2) = sequence[leftIdx++];
            
            if (map[idx1, idx2] == 'T' && !treasureSet.Contains(idx1*size+idx2))
            {
                treasureSet.Add(idx1 * size + idx2);
                ++treasureFound;
                
                var pSequence = TraceSequence(trace, sequence, leftIdx);
                sequence = pSequence;
                leftIdx = rightIdx = pSequence.Count;
                
                CreateOrClearTrace(trace, map, Tuple.Create(true, 0, 0), Tuple.Create(false, 0, 0));
                trace[idx1, idx2] = Tuple.Create(true, trace[idx1, idx2].Item2, trace[idx1, idx2].Item3);
                
                MergeSequenceWithDuplicateEnds(finalSequence, sequence);
            }

            if (treasureFound == treasureCount)
            {
                if (tsp && map[startIdx1, startIdx2] != 'T')
                {
                    --treasureFound;
                    map[startIdx1, startIdx2] = 'T';
                }
                else break;
            }
            
            var tuple = Tuple.Create(true, idx1, idx2);
            
            if (idx2 - 1 >= 0 && trace[idx1, idx2 - 1].Item1 == false)
            {
                trace[idx1, idx2 - 1] = tuple;
                sequence.Add(Tuple.Create(true, idx1, idx2-1)); ++rightIdx;
            }

            if (idx2 + 1 < map.GetLength(1) && trace[idx1, idx2 + 1].Item1 == false) 
            {
                trace[idx1, idx2 + 1] = tuple;
                sequence.Add(Tuple.Create(true, idx1, idx2+1)); ++rightIdx;   
            }

            if (idx1 - 1 >= 0 && trace[idx1 - 1, idx2].Item1 == false)
            {
                trace[idx1-1, idx2] = tuple;
                sequence.Add(Tuple.Create(true, idx1-1, idx2)); ++rightIdx; 
            }

            if (idx1 + 1 < map.GetLength(0) && trace[idx1 + 1, idx2].Item1 == false) 
            {
                trace[idx1+1, idx2] = tuple;
                sequence.Add(Tuple.Create(true, idx1+1, idx2)); ++rightIdx;   
            }

        }
        
        var endTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        if (tsp) map[startIdx1, startIdx2] = 'K';

        return new Solution
        {
            Path = TracePath(finalSequence), 
            Sequence = finalSequence,
            ExecutionTime = endTime-startTime, 
            NodesCheckedCount = nodesCheckedCount
        };
    }

    // example
    public static void Main()
    {
        var arr = new char[,]{ { 'K', 'R', 'R', 'R' }, { 'X', 'R', 'X', 'T'}, {'T', 'T', 'R', 'R'}, {'X', 'R', 'X', 'X'} };

        var treasureMap = new TreasureMap()
        {
            MapArr = arr,
            StartPoint = Tuple.Create(0, 0),
            TreasureCount = 3
        };

        var x = new BreadthSolver {TreasureMap = treasureMap};
        
        var solution = x.Solve(true);
        solution.Sequence.ForEach(Console.Write);
    }
    
}