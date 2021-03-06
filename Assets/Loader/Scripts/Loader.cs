﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader {
    
	public static Map LoadMap()
    {
        char[,] tiles = new char[,] {
            { '#','_','_','#','#','#','#','#' },
            { '#','_','_','#','_','_','_','#' },
            { '#','_','_','#','_','_','_','#' },
            { '#','_','_','#','_','_','_','#' },
            { '#','_','_','#','#','_','#','#' },
            { '#','_','_','#','#','_','#','#' },
            { '_','_','_','_','_','_','_','_' },
            { '#','_','_','#','#','#','#','#' },
        };
        //tiles = new char[,] {
        //    { '_','_','_','_','_' },
        //    { '_','3','#','1','_' },
        //    { '_','#','#','#','_' },
        //    { '_','9','#','7','_' },
        //    { '_','_','_','_','_' }
        //};
        
        return new Map(tiles);
    }
    public static Map LoadFloorTileMap(int size)
    {
        char[,] tiles = new char[size, size];
        for (int i = 0; i<size;i++)
        {
            for (int j=0; j<size;j++)
            {
                tiles[i, j] = '_';
            }
        }
        return new Map(tiles);
    }
}

public class Map
{
    public readonly int X;
    public readonly int Y;
    private char[,] _tiles;

    public Map(char[,] tiles)
    {
        X = tiles.GetLength(0);
        Y = tiles.GetLength(1);
        _tiles = tiles;
    }

    public char[,] GetTiles()
    {
        return _tiles;
    }
    
    public char GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x > _tiles.GetLength(0) - 1 || y > _tiles.GetLength(1) - 1)
            return '#';
        return _tiles[x, y];
    }
}
