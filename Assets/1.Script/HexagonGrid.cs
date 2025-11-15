using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class HexagonGrid : MonoBehaviour
{
    private const int FirstLineCount = 11;
    private const int ScendLineCount = 10;
    public Bubble bubbleObj;
    public Grid grid;
    private readonly List<Bubble[]> _hexList = new();

    public void AddHexLine(int lineCount = 1)
    {
        for (int i = 0; i < lineCount; ++i)
        {
            var isFirstLine = 0 == (_hexList.Count % 2);
            _hexList.Add(isFirstLine ? new Bubble[FirstLineCount] : new Bubble[ScendLineCount]);
        }
    }

    public void SetBubble(Bubble bubble, int line, int index)
    {
        var pos = GetGridPosition(line, index);
        if (bubble.IsUnityNull())
            bubble = Instantiate(bubbleObj);
        bubble.transform.position = pos;
        _hexList[line][index] = bubble;
        
    }
    public Vector3 GetHitPointToCell(RaycastHit2D hit)
    {
        var localPos = new Vector3(
            hit.point.x,
            hit.point.y - (grid.cellSize.y / 2));
        var vector3 = grid.WorldToCell(localPos);
        return vector3;
    }
    public Vector3 GetGridPosition(Vector3Int position)
    {
        var error = CheckError(position.x, position.y);
        if (error)
            return Vector3.zero;
        var vector3 = grid.GetCellCenterLocal(position);
        vector3.y *= -1;
        return vector3;
    }
    public Vector3 GetGridPosition(int line, int index)
    {
        var error = CheckError(line, index);
        if (error)
            return Vector3.zero;
        var vector3 = grid.GetCellCenterLocal(new Vector3Int(line, index, 1));
        vector3.y *= -1;
        return vector3;
    }

    private bool CheckError(int line, int index)
    {
        var first = line % 2 == 0;
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
