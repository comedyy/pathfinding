using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;
using UnityEngine;
public class PathFindingByBlock
{
    MapInfo _info;
    FastPriorityQueue<PathFindingNode> _openList;

    PathFindingNode[,] _nodes;

    MapBlocks _blocks;

    public PathFindingByBlock(MapInfo info)
    {
        _info = info;
        _blocks = new MapBlocks(info);
        _nodes = new PathFindingNode[_info.Width, _info.Height];
        for (int i = 0; i < _info.Width; i++)
        {
            for (int j = 0; j < _info.Height; j++)
            {
                _nodes[i, j] = new PathFindingNode(i * _info.Height + j);
                _nodes[i, j].pos = new Vector2Int(i, j);
            }
        }

        _openList = new FastPriorityQueue<PathFindingNode>(_info.Height * _info.Width);
    }

    public List<Vector2Int> FindPathWithAStar(Vector2Int from, Vector2Int to, int size, List<DebugNode> searchNodes)
    {
        int searchCount = 0;
        _openList.Clear();
        for (int i = 0; i < _info.Width; i++)
        {
            for (int j = 0; j < _info.Height; j++)
            {
                _nodes[i, j] = new PathFindingNode(i * _info.Height + j);
                _nodes[i, j].pos = new Vector2Int(i, j);
            }
        }

        var initNode = _nodes[from.x, from.y];
        initNode.g = 1;
        AddToOpenList(_nodes[from.x, from.y], null, to);

        while (_openList.Count > 0)
        {
            var current = _openList.Dequeue();
            searchCount++;

            if(searchNodes != null)
            {
                searchNodes.Add(new DebugNode()
                {
                    debugPos = current.pos,
                    debugValue = current.f
                });
            }

            if(current.pos == to)
            {
                Debug.Log("OpenList:" + _openList.Count + " searchCount:"+  searchCount);
                break;
            }

            foreach (var item in GetNeibor(current, size))
            {
                item.g = current.g + 1;
                AddToOpenList(item, current, to);
            }
        }

        List<Vector2Int> lst = new List<Vector2Int>();

        // Find
        if(_nodes[to.x, to.y].parentNode != null)
        {
            var current = _nodes[to.x, to.y];
            lst.Add(current.pos);

            while (current.parentNode != null)
            {
                current = current.parentNode;
                lst.Add(current.pos);
            }

            lst.Reverse();
        }
        
        Debug.Log("pathSize:" + lst.Count);

        return lst;
    }

    private void AddToOpenList(PathFindingNode pathFindingNode, PathFindingNode parent, Vector2Int to)
    {
        Vector2Int diff = pathFindingNode.pos - to;
        pathFindingNode.h = Mathf.Abs(diff.x) + Mathf.Abs(diff.y);
        pathFindingNode.parentNode = parent;

        if(pathFindingNode.QueueIndex > 0)
        {
            _openList.UpdatePriority(pathFindingNode, pathFindingNode.f);
        }
        else
        {
            _openList.Enqueue(pathFindingNode, pathFindingNode.f);
        }
    }

    
    static Vector2Int[] dirs = new Vector2Int[]{
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(0, 1),
    };
    private IEnumerable<PathFindingNode> GetNeibor(PathFindingNode current, int size)
    {
        foreach (var dir in dirs)
        {
            var pos = current.pos + dir;
            if(!IsInMap(pos)) continue;
            if(_info.Grids[pos.x, pos.y].isAbstacle) continue;
            if(_info.Grids[pos.x, pos.y].size < size) continue;

            var item = _nodes[pos.x, pos.y];
            if(item.g == 0 || item.g > (current.g + 1))
            {
                yield return item;
            }
        }
    }

    bool IsInMap(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < _info.Width && pos.y < _info.Height;
    }
}
