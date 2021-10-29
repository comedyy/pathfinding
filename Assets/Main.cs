using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Main : MonoBehaviour
{
    MapInfo _mapInfo = new MapInfo();
    PathFinding _pathFinding;

    List<Vector2Int> pathSize1;
    List<Vector2Int> pathSize2;
    List<Vector2Int> pathSize3;

    void Start()
    {
        _mapInfo.Init(18, 20, new List<RectInt>(){
            new RectInt(0, 0, 10, 3),
            new RectInt(5, 4, 4, 4),
            new RectInt(4, 10, 4, 4),
            new RectInt(3, 17, 4, 3),
        });
        
        _pathFinding = new PathFinding(_mapInfo);
        pathSize1 = _pathFinding.FindPathWithAStar(new Vector2Int(15, 2), new Vector2Int(2, 6), 1);
        pathSize2 = _pathFinding.FindPathWithAStar(new Vector2Int(15, 2), new Vector2Int(2, 6), 2);
        pathSize3 = _pathFinding.FindPathWithAStar(new Vector2Int(15, 2), new Vector2Int(2, 6), 3);

        DumpPath(pathSize1);
        DumpPath(pathSize2);
        DumpPath(pathSize3);
    }

    private void DumpPath(List<Vector2Int> pathSize3)
    {
        if(pathSize3 == null) return;

        Debug.LogError(string.Join(",", pathSize3.Select(m=>string.Format("{0}", m)) ));
    }

    private void OnDrawGizmos()
    {
        Vector3 from = new Vector3(0, 0, 0);
        Vector3 to = new Vector3(0, 0, _mapInfo.Height);
        for (int i = 0; i <= _mapInfo.Width; i++)
        {
        Gizmos.color = Color.white;
            Gizmos.DrawLine(from + new Vector3(i, 0, 0), to + new Vector3(i, 0, 0));
        }

        from = new Vector3(0, 0, 0);
        to = new Vector3(_mapInfo.Width, 0, 0);
        for (int j = 0; j <= _mapInfo.Height; j++)
        {
            Gizmos.DrawLine(from + new Vector3(0, 0, j), to + new Vector3(0, 0, j));
        }

        for (int i = 0; i < _mapInfo.Width; i++)
        {
            for (int j = 0; j < _mapInfo.Height; j++)
            {
                // 1. 障碍
                Gizmos.color = _mapInfo.Grids[i, j].isAbstacle ? Color.blue : Color.white;
                var pos = new Vector3(i + 0.5f, 0, j + 0.5f);
                Gizmos.DrawWireCube(pos, new Vector3(1, 0, 1));
                Gizmos.color = Color.black;

                // 2. 宽度
                UnityEditor.Handles.Label(pos + Vector3.up * 0.1f, _mapInfo.Grids[i, j].size.ToString());
            }
        }

        DumpPathGraGizoms(pathSize1, Color.red);
        DumpPathGraGizoms(pathSize2, Color.green);
        DumpPathGraGizoms(pathSize3, Color.yellow);
    }

    private void DumpPathGraGizoms(List<Vector2Int> pathSize, Color yellow)
    {
        if(pathSize == null) return;

        Gizmos.color = yellow;

        for(int i = 1; i < pathSize.Count; i++)
        {
            Gizmos.DrawLine(GenVec3(pathSize[i-1] + new Vector2(0.5f, 0.5f)), GenVec3(pathSize[i] + new Vector2(0.5f, 0.5f)));
        }

        Gizmos.color = Color.white;
    }

    Vector3 GenVec3(Vector2 vec)
    {
        return new Vector3(vec.x, 0, vec.y);
    }
}
