using System.Collections.Generic;
using UnityEngine;

public class HexagonGrid
{
    private List<BubbleType[]> hexList = new();

    public void AddHexLine(int lineCount = 1)
    {
        for (int i = 0; i < lineCount; ++i)
        {
            var line = hexList.Count % 2;
            hexList.Add(line == 0 ? new BubbleType[11] : new BubbleType[10]);
        }
    }
}
