using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class HexagonGrid : MonoBehaviour
{
    private const int FirstLineCount = 11;
    private const int ScendLineCount = 10;
    public Grid grid;
    private readonly List<Bubble[]> _hexList = new();

    public void Start()
    {
        AddHexLine(10);
        for (int i = 0; i < FirstLineCount; ++i)
            SetBubble(null, 6, i);
        for (int i = 0; i < ScendLineCount; ++i)
            SetBubble(null, 7, i);
    }

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
            bubble = BubblePool.I.Pool.Get();
        bubble.SetType(BubbleType.Bule);
        bubble.transform.SetParent(transform);
        bubble.transform.position = pos;
        _hexList[line][index] = bubble;
        
    }
    public Vector3Int GetHitPointToCell(RaycastHit2D hit)
    {
        var localPos = new Vector3(
            hit.point.x,
            hit.point.y - (grid.cellSize.y / 2));
        var vector3 = grid.WorldToCell(localPos);
        return vector3;
    }
    public Vector3 GetGridPosition(Vector3Int position)
    {
        var error = false == CheckError(position.x, position.y * -1);
        if (error)
            return Vector3.zero;
        var vector3 = grid.GetCellCenterWorld(position);
        return vector3;
    }
    [Button]
    public Vector3 GetGridPosition(int line, int index)
    {
        var error = false == CheckError(line, index);
        if (error)
            return Vector3.zero;
        var vector3 = grid.GetCellCenterWorld(new Vector3Int(index, line * -1, 1));
        return vector3;
    }

    private bool CheckError(int line, int index)
    {
        // var first = line % 2 == 0;
        // if (first && index < FirstLineCount)
        // {
        //     Debug.LogError("범위 오류");
        //     return false;
        // }
        //
        // if (false == first && index < ScendLineCount)
        // {
        //     Debug.LogError("범위 오류");
        //     return false;
        // }
        return true;
    }
}
