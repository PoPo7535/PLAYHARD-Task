using Sirenix.OdinInspector;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Vector2Int[] _leftLine =
    {
        new(2, 4), new(1, 4),
        new(0, 5),
        new(1, 6), new(2, 6), new(3, 6), new(4, 6),
        new(4, 7),
        new(4, 8), new(3, 8), new(2, 8), new(1, 8),
        new(0, 9),
        new(1, 10), new(2, 10), new(3, 10)
    };
    [Button]
    public void Foo()
    {
        for (int i = 0; i < _leftLine.Length; ++i)
        {
            HexagonGrid.I.SetBubble(null, _leftLine[i]);
        }
    }
}
