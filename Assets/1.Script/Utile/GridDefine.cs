using UnityEngine;

public static class GridDefine
{
    public static readonly Vector2Int[] FirstLineVisit = 
    {
        new(-1, 1), new(0, 1), 
        new(-1, 0), new(1, 0),
        new(-1, -1), new(0, -1)
    };
    public static readonly Vector2Int[] SecondLineVisit =
    {
        new(0, 1), new(1, 1), 
        new(-1, 0), new(1, 0), 
        new(0, -1), new(1, -1)
    };

    public static readonly Vector2Int[] FirstAreaLineVisit =
    {
        new(-1, 2), new(0, 2), new(1, 2),
        new(-2, 1), new(-1, 1), new(0, 1), new(1, 1),
        new(-2, 0), new(-1, 0), new(1, 0), new(2, 0),
        new(-2, -1), new(-1, -1), new(0, -1), new(1, -1),
        new(-1, -2), new(0, -2), new(1, -2),
    };
    public static readonly Vector2Int[] SecondAreaLineVisit =
    {
        new(-1, 2), new(0, 2), new(1, 2),
        new(-1, 1), new(0, 1), new(1, 1), new(2, 1), 
        new(-2, 0), new(-1, 0), new(1, 0), new(2, 0), 
        new(-1, -1), new(0, -1), new(1, -1), new(2, -1), 
        new(-1, -2), new(0, -2), new(1, -2),
    };
    public static readonly Vector2Int[] BossLeftLine =
    {
        new(3, 4), new(2, 4), new(1, 4),
        new(0, 5),
        new(1, 6), new(2, 6), new(3, 6), new(4, 6),
        new(4, 7),
        new(4, 8), new(3, 8), new(2, 8), new(1, 8),
        new(0, 9),
        new(1, 10), new(2, 10), new(3, 10)
    };
    public static readonly Vector2Int[] BossRightLine =
    {
        new(7, 4), new(8, 4), new(9, 4),
        new(9, 5),
        new(9, 6), new(8, 6), new(7, 6), new(6, 6),
        new(5, 7),
        new(6, 8), new(7, 8), new(8, 8), new(9, 8),
        new(9, 9),
        new(9, 10), new(8, 10), new(7, 10)
    };

}
