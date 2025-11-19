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
}
