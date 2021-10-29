using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GridInfo
{
    public int size;
    public bool isAbstacle;
    public Vector2Int AvailableSize;
}

public class MapInfo
{
    public GridInfo[,] Grids;
    public int Height{get; private set;}
    public int Width{get; private set;}

    public void Init(int width, int height, List<RectInt> obstacles)
    {
        Width = width;
        Height = height;
        Grids = new GridInfo[width, height];

        foreach (var obstacle in obstacles)
        {
            foreach (var item in obstacle.allPositionsWithin)
            {
                Grids[item.x, item.y].isAbstacle = true;
            }
        }

        InitCalMapSize();
    }

    private void InitCalMapSize()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if(Grids[i,j].isAbstacle) continue;

                var size = CalSize(i, j);
                Grids[i, j].AvailableSize = size;
                Grids[i, j].size = Mathf.Min(size.x, size.y);
            }
        }
    }

    private Vector2Int CalSize(int i, int j)
    {
        Vector2Int size = Vector2Int.one;

        // right
        int rightI = i - 1;
        if(isInMap(rightI, j))
        {
            size.x = Mathf.Max(size.x, Grids[rightI, j].AvailableSize.x + 1);
        }

        // down;
        int downJ = j - 1;
        if(isInMap(i, downJ))
        {
            size.y = Mathf.Max(size.y, Grids[i, downJ].AvailableSize.y + 1);
        }

        return size;
    }

    bool isInMap(int i, int j)
    {
        return i >= 0 && j >= 0 && i < Width && j < Height;
    }
}
