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

    private LevelType m_levelType = LevelType.None;
    private LevelPoint m_startPoint;

    private const int UP_LEFT = 0;
    private const int UP_RIGTH = 1;
    private const int DOWN_LEFT = 2;
    private const int DOWN_RIGTH = 3;

    public LevelPoint[] FourCorners = new LevelPoint[4];

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
                offerAsCorner(point);
                worldData[point.X, point.Y] = '_';
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    private void offerAsCorner(LevelPoint point)
    {


        if((FourCorners[UP_LEFT].X - FourCorners[UP_LEFT].Y)/2 > (point.X - point.Y)/2)
        {
            FourCorners[UP_LEFT] = point;
        }

        if ((FourCorners[UP_RIGTH].X + FourCorners[UP_RIGTH].Y)/2 < (point.X + point.Y)/2)
        {
            FourCorners[UP_RIGTH] = point;
        }

        if ((FourCorners[DOWN_LEFT].X + FourCorners[DOWN_LEFT].Y)/2 > (point.X + point.Y)/2)
        {
            FourCorners[DOWN_LEFT] = point;
        }

        if ((FourCorners[DOWN_RIGTH].X - FourCorners[DOWN_RIGTH].Y)/2 < (point.X - point.Y)/2)
        {
            FourCorners[DOWN_RIGTH] = point;
        }

        /*if(FourCorners[UP_LEFT].Y <= point.Y)
        {
            if(FourCorners[UP_LEFT].X >= point.X)
            {
                FourCorners[UP_LEFT] = point;
            }
        }
        else if (FourCorners[UP_LEFT].X > point.X)
        {
            if((FourCorners[UP_LEFT].Y - point.Y) < (LevelHeigth / 8))
            {
                FourCorners[UP_LEFT] = point;
            }
        }

        if (FourCorners[UP_RIGTH].Y <= point.Y)
        {
            if (FourCorners[UP_RIGTH].X <= point.X)
            {
                FourCorners[UP_RIGTH] = point;
            }
        }
        else if (FourCorners[UP_RIGTH].X < point.X)
        {
            if ((FourCorners[UP_RIGTH].Y - point.Y) < (LevelHeigth / 8))
            {
                FourCorners[UP_RIGTH] = point;
            }
        }

        if (FourCorners[DOWN_LEFT].Y >= point.Y)
        {
            if (FourCorners[DOWN_LEFT].X >= point.X)
            {
                FourCorners[DOWN_LEFT] = point;
            }
        }
        else if (FourCorners[DOWN_LEFT].X > point.X)
        {
            if ((FourCorners[DOWN_LEFT].Y - point.Y) < (LevelHeigth / 8))
            {
                FourCorners[DOWN_LEFT] = point;
            }
        }

        if (FourCorners[DOWN_RIGTH].Y >= point.Y)
        {
            if (FourCorners[DOWN_RIGTH].X <= point.X)
            {
                FourCorners[DOWN_RIGTH] = point;
            }
        }
        else if (FourCorners[DOWN_RIGTH].X < point.X)
        {
            if ((FourCorners[DOWN_RIGTH].Y - point.Y) < (LevelHeigth / 8))
            {
                FourCorners[DOWN_RIGTH] = point;
            }
        }*/
    }

    private void GeneratePaths(char[,] worldData)
    {

        int count = Random.Range(MinPaths,MaxPaths);
        LevelPoint[] lastPath = RunPath(LevelWidth/2,LevelHeigth/2);
        if(m_levelType == LevelType.Ambush)
        {
            m_startPoint = lastPath[0];
        }
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


        FourCorners[UP_LEFT].X = LevelWidth;
        FourCorners[UP_LEFT].Y = 0;

        FourCorners[UP_RIGTH].X = 0;
        FourCorners[UP_RIGTH].Y = 0;

        FourCorners[DOWN_LEFT].Y = LevelHeigth;
        FourCorners[DOWN_LEFT].X = LevelWidth;

        FourCorners[DOWN_RIGTH].X = 0;
        FourCorners[DOWN_RIGTH].Y = LevelHeigth;

        if (m_world.GetGrid() == null)
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

    public LevelPoint StartPoint
    {
        get
        {
            return m_startPoint;
        }
    }


    public void Generate(LevelType levelType)
    {

        char[,] worldData = new char[LevelWidth,LevelHeigth];
        fill(worldData,'#');

        purgeOld();

        GeneratePaths(worldData);

        m_world.InitMap(new Map(worldData));
        m_world.Draw();

        foreach(World.Tile tile in m_world.GetGrid().GetCell(FourCorners[UP_LEFT].X, FourCorners[UP_LEFT].Y).GetTiles())
        {
            tile.GetGO().GetComponent<MeshRenderer>().material.color = Color.red;
        }

        foreach (World.Tile tile in m_world.GetGrid().GetCell(FourCorners[UP_RIGTH].X, FourCorners[UP_RIGTH].Y).GetTiles())
        {
            tile.GetGO().GetComponent<MeshRenderer>().material.color = Color.magenta;
        }

        foreach (World.Tile tile in m_world.GetGrid().GetCell(FourCorners[DOWN_LEFT].X, FourCorners[DOWN_LEFT].Y).GetTiles())
        {
            tile.GetGO().GetComponent<MeshRenderer>().material.color = Color.cyan;
        }

        foreach (World.Tile tile in m_world.GetGrid().GetCell(FourCorners[DOWN_RIGTH].X, FourCorners[DOWN_RIGTH].Y).GetTiles())
        {
            tile.GetGO().GetComponent<MeshRenderer>().material.color = Color.green;
        }

    }


}
