using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private readonly Vector2Int[] _leftLine =
    {
        new(3, 4), new(2, 4), new(1, 4),
        new(0, 5),
        new(1, 6), new(2, 6), new(3, 6), new(4, 6),
        new(4, 7),
        new(4, 8), new(3, 8), new(2, 8), new(1, 8),
        new(0, 9),
        new(1, 10), new(2, 10), new(3, 10)
    };
    private readonly Vector2Int[] _rightLine =
    {
        new(7, 4), new(8, 4), new(9, 4),
        new(9, 5),
        new(9, 6), new(8, 6), new(7, 6), new(6, 6),
        new(5, 7),
        new(6, 8), new(7, 8), new(8, 8), new(9, 8),
        new(9, 9),
        new(9, 10), new(8, 10), new(7, 10)
    };

    [Button]
    public void Test()
    {
        BubbleLineRefill(_leftLine);
    }
    [Button]
    public void Test2()
    {
        BubbleLineRefill(_rightLine);
    }

    public void BubbleLineRefill(Vector2Int[] line)
    {
        if (HexagonGrid.I.IsValid(line.Last()))
            return;
        HexagonGrid.I.SetBubble(null, line[0]);
        for (int i = line.Length - 2; i >= 0; i--)
        {
            if (false == HexagonGrid.I.IsValid(line[i])) 
                continue;
            if (i != 0)
                HexagonGrid.I.MoveCellBubble(line[i], line[i + 1]);
            else
                HexagonGrid.I.MoveCellBubble(line[i], line[i + 1], () => BubbleLineRefill(line));
        }
    }
}
