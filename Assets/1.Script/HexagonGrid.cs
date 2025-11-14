using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class HexagonGrid : MonoBehaviour
{
    private const int FirstLineCount = 11;
    private const int ScendLineCount = 10;
    public Grid grid;
    private readonly List<Bubble[]> _hexList = new();

    public void AddHexLine(int lineCount = 1)
    {
        for (int i = 0; i < lineCount; ++i)
        {
            var line = _hexList.Count % 2;
            _hexList.Add(line == 0 ? new Bubble[FirstLineCount] : new Bubble[ScendLineCount]);
        }
    }

    [Button]
    public Vector3 GetGridPosition(Vector3Int position)
    {
        var error = CheckError(position.x, position.y);
        if (error)
            return Vector3.zero;
        var vector3 = grid.GetCellCenterLocal(position);
        vector3.y *= -1;
        return vector3;
    }

    private bool CheckError(int lineCount, int index)
    {
        var first = lineCount % 2 == 0;
        if (first && index < FirstLineCount)
        {
            Debug.LogError("범위 오류");
            return false;
        }

        if (false == first && index < ScendLineCount)
        {
            Debug.LogError("범위 오류");
            return false;
        }
        return true;
    }
}
