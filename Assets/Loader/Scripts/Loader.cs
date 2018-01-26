using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader {
    
	public static Map LoadMap()
    {
        char[,] tiles = new char[,] {
            { '#','_','_','#','#','#','#','#' },
            { '#','_','_','#','/','_','\\','#' },
            { '#','_','_','#','_','_','_','#' },
            { '#','_','_','#','_','_','_','#' },
            { '#','_','_','#','c','_','%','#' },
            { '#','_','_','#','#','_','#','#' },
            { '_','_','_','_','_','_','_','_' },
            { '#','_','_','#','#','#','#','#' },
        };
        return new Map(8, 8, tiles);
    }
}

public class Map
{
    public readonly int X;
    public readonly int Y;
    private char[,] _tiles;

    public Map(int x, int y, char[,] tiles)
    {
        X = x;
        Y = y;
        _tiles = tiles;
    }

    public char[,] GetTiles()
    {
        return _tiles;
    }

    public char GetTile(int x, int y)
    {
        return _tiles[x, y];
    }
}
