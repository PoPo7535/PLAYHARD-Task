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
    private const int SecondLineCount = 10;
    private readonly List<Bubble[]> _hexList = new();
    private readonly List<bool[]> _hexVisitList = new();
    [SerializeField]private Grid grid;
    public Vector2 CellSize => grid.cellSize;

    public void Awake()
    {
        AddHexLine(11);
    }

    public void FindDropBubble(Vector2Int[] cellPos)
    {
        Queue<Vector2Int> queue = new();
        foreach (var vec in cellPos)
        {
            queue.Enqueue(vec);
            _hexVisitList[vec.y][vec.x] = true;
        }

        var firstVisit = new Vector2Int[] { new(-1, 0), new(1, 0), new(-1, 1), new(0, 1) };
        var secondVisit = new Vector2Int[] { new(-1, 0), new(1, 0), new(0, 1), new(1, 1) };
        
        while (queue.Count > 0)
        {
            var cell = queue.Dequeue();
            var row = 0 == cell.y % 2 ? FirstLineCount : SecondLineCount;
            var visit = 0 == cell.y % 2 ? firstVisit : secondVisit;
            foreach (var vec in visit)
            {
                var findCell = vec + cell;

                if (findCell.x >= 0 && findCell.x < row &&
                    findCell.y >= 0 && findCell.y < _hexList.Count)
                    continue;
                Visit(findCell);
            }
        }

        for (int i = 0; i < _hexVisitList.Count; ++i)
            for (int j = 0; j < _hexVisitList[i].Length; ++j) 
                if (false == _hexVisitList[i][j]) 
                    _hexList[i][j].Drop();

        VisitClear();

        return;

        void Visit(Vector2Int findCell)
        {
            if (false == _hexList[findCell.y][findCell.x].IsUnityNull() &&
                false == _hexVisitList[findCell.y][findCell.x])
            {
                queue.Enqueue(findCell);
                _hexVisitList[findCell.y][findCell.x] = true;
            }
        }
    }


    private void VisitClear()
    {
        foreach (var arr in _hexVisitList)
            Array.Clear(arr, 0, arr.Length);
    }
    public void AddHexLine(int lineCount = 1)
    {
        for (int i = 0; i < lineCount; ++i)
        {
            var isFirstLine = 0 == (_hexList.Count % 2);
            _hexList.Add(isFirstLine ? new Bubble[FirstLineCount] : new Bubble[SecondLineCount]);
            _hexVisitList.Add(isFirstLine ? new bool[FirstLineCount] : new bool[SecondLineCount]);
        }
    }

    public void SetBubble(Bubble bubble, Vector2Int cell, BubbleType type)
    {
        var pos = GetCellNumberToPos(cell);
        if (bubble.IsUnityNull())
            bubble = BubblePool.I.Pool.Get();
        bubble.SetType(type);
        if (type == BubbleType.None)
        {
            _hexList[cell.y][cell.x] = null;
            return;
        }
        
        bubble.transform.SetParent(transform);
        bubble.transform.position = pos;
        if (cell.y == _hexList.Count)
            AddHexLine(1 + cell.y - _hexList.Count);
        _hexList[cell.y][cell.x] = bubble;
    }

    public List<Bubble> CollectConnectedBubbles(Vector2Int cell)
    {
        var firstVisit = new Vector2Int[] { new(-1, 0), new(1, 0), new(-1, 1), new(0, 1), new(-1, -1), new(0, -1) };
        var secondVisit = new Vector2Int[] { new(-1, 0), new(1, 0), new(0, 1), new(1, 1), new(0, -1), new(1, -1) };
        var type = _hexList[cell.y][cell.x].MyType;
        var cellQueue = new Queue<Vector2Int>();
        var cellList = new List<Bubble>();
        cellQueue.Enqueue(cell);
        cellList.Add(_hexList[cell.y][cell.x]);
        _hexVisitList[cell.y][cell.x] = true;

        while (0 < cellQueue.Count)
        {
            cell = cellQueue.Dequeue();
            var vis = 0 == cell.y % 2 ? firstVisit : secondVisit;
            foreach (var vi in vis)
            {
                var newCell = cell + vi;
                var row = 0 == newCell.y % 2 ? FirstLineCount : SecondLineCount;
                if (newCell.x >= 0 && newCell.x < row &&
                    newCell.y >= 0 && newCell.y < _hexList.Count &&
                    false == _hexList[newCell.y][newCell.x].IsUnityNull() &&
                    false == _hexVisitList[newCell.y][newCell.x])
                {
                    if (_hexList[newCell.y][newCell.x].MyType == type)
                    {
                        cellQueue.Enqueue(newCell);
                        cellList.Add(_hexList[newCell.y][newCell.x]);
                    }
                    _hexVisitList[newCell.y][newCell.x] = true;
                }
            }
        }

        VisitClear();
        return cellList;
    }
    
    public void MoveCellBubble(Vector2Int startCell, Vector2Int endCell, Action endCallBack = null, float dur = 0.1f)
    {
        var pos = GetCellNumberToPos(endCell);
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
        var cellPos = (Vector2Int)grid.WorldToCell(pos);
        cellPos.y *= -1;
        return cellPos;
    }

    public Vector3 GetCellNumberToPos(Vector2Int cell)
    {
        var vector3 = grid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y * -1, 1));
        return vector3;
    }

    public Vector3 GetPosToWorldPos(Vector2 position)
    {
        return GetCellNumberToPos(GetPosToCellNumber(position));
    }
}
