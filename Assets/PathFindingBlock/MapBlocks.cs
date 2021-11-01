using System.Collections.Generic;
using UnityEngine;

class LineDistance
{
    public Gate nextGate;
    public float distance;
}

class Block
{
    public Vector2 vector2;
    public List<Gate> gates;

    Dictionary<Gate, List<LineDistance>> lineDistance = new Dictionary<Gate, List<LineDistance>>();

    public Block(Vector2 vector2)
    {
        this.vector2 = vector2;
        gates = new List<Gate>();
        InitDistance();
    }

    void InitDistance()
    {

    }
}

class Gate
{
    public Block b1;
    public Block b2;
}

public class MapBlocks
{
    private MapInfo info;
    const float BLOCK_SIZE = 10f;

    Block[,] _blocks;
    int Width = 0;
    int Height = 0;

    public MapBlocks(MapInfo info)
    {
        this.info = info;

        Width = Mathf.CeilToInt(info.Width / BLOCK_SIZE);
        Height = Mathf.CeilToInt(info.Width / BLOCK_SIZE);
        _blocks = new Block[Width, Height];

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                _blocks[i,j] = new Block(new Vector2(i, j));
            }
        }
    }

    void InitGates()
    {
        for (int j = 0; j < Height; j++)
        {
            for (int i = 1; i < Width; i++)
            {

            }
        }
        
        for (int j = 1; j < Height; j++)
        {
            
        }
    }
}