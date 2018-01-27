using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour {

    private World m_world;
    // Use this for initialization

    public int LevelWidth = 100;
    public int LevelHeigth = 80;
    public int MaxPathLen = 30;
    public int MinPathLen = 2;

    public int MaxPaths = 10;
    public int MinPaths = 5;

    void Start()
    {
        m_world = World.GetInstance();
        
      //  m_world.Draw();
    }
    
    private void fill(char[,] toFill, char with)
    {
        for(int i = 0; i < LevelWidth; i++)
        {
            for(int o = 0;o< LevelHeigth; o++)
            {
                toFill[i, o] = with;
            }
        }
    }

    private LevelPoint[] RunPath(int startX, int startY)
    {
        return new LevelPath(startX, startY, MinPathLen, MaxPathLen,LevelWidth,LevelHeigth).RunPath();
    }

    private void applyPath(char[,] worldData, LevelPoint[] path)
    {
        try
        {

            foreach (LevelPoint point in path)
            {
                worldData[point.X, point.Y] = '_';
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    private void GeneratePaths(char[,] worldData)
    {

        int count = Random.Range(MinPaths,MaxPaths);
        LevelPoint[] lastPath = RunPath(LevelWidth/2,LevelHeigth/2);
        applyPath(worldData,lastPath);

        for(int i = 1; i < count; i++)
        {
            LevelPoint start = lastPath[Random.Range(0,lastPath.Length)];
            LevelPoint[] path = RunPath(start.X,start.Y);
            applyPath(worldData, path);
            lastPath = path;
        }


    }

    private void purgeOld()
    {

        if(m_world.GetGrid() == null)
        {
            return;
        }

        foreach(World.Cell cell in m_world.GetGrid().GetCells())
        {
            foreach (World.Tile tile in cell.GetTiles())
            {
                GameObject.Destroy(tile.GetGO());
            }
        }
    }

    public void Generate()
    {

        char[,] worldData = new char[LevelWidth,LevelHeigth];
        fill(worldData,'#');

        purgeOld();

        GeneratePaths(worldData);

        m_world.InitMap(new Map(worldData));
        m_world.Draw();
    }


}
