using UnityEngine;

public static class ScoreHelper
{
    private static int _currentCombo = 1;
    public static int PopBubbleScore => 10 * _currentCombo;
    public static readonly int DropBubbleScore = 50;
    
    public static readonly int PopEnergyBubbleScore = 250;
    public static readonly int DropBoomBubbleScore = 100;
    public static readonly int SpareBubbleScore = 1000;
    public static int TotalScore = 0;

    public static void AddCombo() { ++_currentCombo; }

    public static void ReSetCombo() { _currentCombo = 1; }

    public static void Init()
    {
        _currentCombo = 1;
        TotalScore = 0;
    }
}
