using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

public class HexagonGrid : LocalSingleton<HexagonGrid>
{
    private const int FirstLineCount = 11;
    private const int ScendLineCount = 10;
    private readonly List<Bubble[]> _hexList = new();
    [SerializeField]private Grid grid;
    public Vector2 CellSize => grid.cellSize;

    public void Start()
    {
        AddHexLine(11);
        // for (int i = 0; i < FirstLineCount; ++i)
        //     SetBubble(null, 6, i);
        // for (int i = 0; i < ScendLineCount; ++i)
        //     SetBubble(null, 7, i);
    }

    public void AddHexLine(int lineCount = 1)
    {
        for (int i = 0; i < lineCount; ++i)
        {
            var isFirstLine = 0 == (_hexList.Count % 2);
            _hexList.Add(isFirstLine ? new Bubble[FirstLineCount] : new Bubble[ScendLineCount]);
        }
    }

    public void SetBubble(Bubble bubble, Vector2Int cell)
    {
        var pos = GetCellPos(cell);
        if (bubble.IsUnityNull())
            bubble = BubblePool.I.Pool.Get();
        bubble.SetType(BubbleType.Bule);
        bubble.transform.SetParent(transform);
        bubble.transform.position = pos;
        if (cell.y == _hexList.Count)
            AddHexLine(1 + cell.y - _hexList.Count);
        _hexList[cell.y][cell.x] = bubble;
    }

    public void MoveCellBubble(Vector2Int startCell, Vector2Int endCell, Action endCallBack = null, float dur = 0.2f)
    {
        var pos = GetCellPos(endCell);
        _hexList[startCell.y][startCell.x].transform.DOMove(pos, dur).SetEase(Ease.Linear).OnComplete(() =>
        {
            endCallBack?.Invoke();
        });
        _hexList[endCell.y][endCell.x] = _hexList[startCell.y][startCell.x];
        _hexList[startCell.y][startCell.x] = null;
    }

    public bool IsValid(Vector2Int cell)
    {
        return false == _hexList[cell.y][cell.x].IsUnityNull();
    }
    public Vector2Int GetPosToCellNumber(Vector2 pos)
    {
        return (Vector2Int)grid.WorldToCell(pos);
    }

    public Vector3 GetCellPos(Vector2Int cell)
    {
        var vector3 = grid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y * -1, 1));
        return vector3;
    }

    public Vector3 GetPosToWorldPos(Vector2 position)
    {
        return  GetCellPos(GetPosToCellNumber(position));
    }
    public Vector3 GetPosToWorldPosTest(Vector2 position)
    {
        return  GetCellPos(GetPosToCellNumber(position));
    }

}
