using UnityEngine;

public static class ScoreHelper
{
    private static int _currentCombo;
    public static readonly int PopBubbleScore = 10 * _currentCombo;
    public static readonly int DropBubbleScore = 50;
    
    public static readonly int PopEnergyBubbleScore = 250;
    public static readonly int DropEnergyBubbleScore = 100;
    public static void AddCombo() { ++_currentCombo; }
    public static void ReSetCombo() { _currentCombo = 0; }
}
