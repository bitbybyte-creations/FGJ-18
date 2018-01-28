using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour {

    private World m_world;
    // Use this for initialization

    public int LevelWidth = 100;
    public int LevelHeigth = 100;
    public int MaxPathLen = 50;
    public int MinPathLen = 20;

    public int MaxPaths = 40;
    public int MinPaths = 20;


    public int MaxEnemySpawnCount = 50;
    public int MinEnemySpawnCount = 10;

    public int EnemyMinDistance = 10;

    public int MinObjectiveDistance = 20;

    public LevelType LevelType = LevelType.None;
    public LevelBuildMode BuildMode = LevelBuildMode.FollowAndContinue;

    public LevelPoint StartPoint;
    public LevelPoint EndPoint;
    public LevelPoint ObjectivePoint;

    public bool Verbose = false;
    
    private const int UP_LEFT = 0;
    private const int UP_RIGTH = 1;
    private const int DOWN_LEFT = 2;
    private const int DOWN_RIGTH = 3;

    public LevelPoint[] FourCorners = new LevelPoint[4];
    public LevelPoint[] EnemySpawnPoints = null;

    public List<LevelPoint> FloorPoints = new List<LevelPoint>();

    void Awake()
    {
       m_world = World.GetInstance();
    }

    void Start()
    {
        
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
                if (!FloorPoints.Contains(point))
                {
                    FloorPoints.Add(point);
                }
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


        if((FourCorners[UP_LEFT].X - FourCorners[UP_LEFT].Y)/2f > (point.X - point.Y)/2f)
        {
            FourCorners[UP_LEFT] = point;
        }

        if ((FourCorners[UP_RIGTH].X + FourCorners[UP_RIGTH].Y)/2f < (point.X + point.Y)/2f)
        {
            FourCorners[UP_RIGTH] = point;
        }

        if ((FourCorners[DOWN_LEFT].X + FourCorners[DOWN_LEFT].Y)/2f > (point.X + point.Y)/2f)
        {
            FourCorners[DOWN_LEFT] = point;
        }

        if ((FourCorners[DOWN_RIGTH].X - FourCorners[DOWN_RIGTH].Y)/2f < (point.X - point.Y)/2f)
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
        if(LevelType == LevelType.Ambush)
        {
            StartPoint = lastPath[0];
        }
        applyPath(worldData,lastPath);

        for(int i = 1; i < count; i++)
        {
            LevelPoint start;
            if (BuildMode == LevelBuildMode.FollowAndContinue)
            {
                start = lastPath[Random.Range(0, lastPath.Length)];
            }
            else if(BuildMode == LevelBuildMode.RandomContinue)
            {
                start = FloorPoints[Random.Range(0, FloorPoints.Count)];
            }
            else
            {
                start.X = LevelWidth / 2;
                start.Y = LevelHeigth / 2;
            }
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

        FloorPoints.Clear();

        if (m_world == null || m_world.GetGrid() == null)
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

    private void generateSpawnEnemySpawnPoints()
    {

        for(int i = 0; i < EnemySpawnPoints.Length; i++)
        {

            while (true)
            {
                EnemySpawnPoints[i] = FloorPoints[Random.Range(0, FloorPoints.Count)];

                if (Mathf.Abs(EnemySpawnPoints[i].X - StartPoint.X) + Mathf.Abs(EnemySpawnPoints[i].Y - StartPoint.Y) > EnemyMinDistance)
                {
                    break;
                }

            }
        }


    }


    private void generateObjectivePoint()
    {

        int count = 0;
        while (true)
        {

            if(count > FloorPoints.Count * FloorPoints.Count)
            {
                MinObjectiveDistance--;
                count = 0;
            }

            ObjectivePoint = FloorPoints[Random.Range(0, FloorPoints.Count)];
            count++;
            if (Mathf.Abs(ObjectivePoint.X - StartPoint.X) + Mathf.Abs(ObjectivePoint.Y - StartPoint.Y) > MinObjectiveDistance)
            {
                break;
            }
        }

    }

    private void generateEndPoint()
    {

        while (true)
        {
            EndPoint = FourCorners[Random.Range(0, 4)];

            if (!EndPoint.Equals(StartPoint))
            {
                break;
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

        if (LevelType != LevelType.Ambush)
        {
            StartPoint = FourCorners[Random.Range(0, 4)];
            generateObjectivePoint();
            generateEndPoint();
        }

        EnemySpawnPoints = new LevelPoint[Random.Range(MinEnemySpawnCount, MaxEnemySpawnCount)];
        generateSpawnEnemySpawnPoints();

        if (Verbose)
        {
            foreach (World.Tile tile in m_world.GetGrid().GetCell(FourCorners[UP_LEFT].X, FourCorners[UP_LEFT].Y).GetTiles())
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


            foreach (World.Tile tile in m_world.GetGrid().GetCell(StartPoint.X, StartPoint.Y).GetTiles())
            {
                tile.GetGO().GetComponent<MeshRenderer>().material.color = Color.yellow;
            }

            foreach (LevelPoint point in EnemySpawnPoints)
            {
                foreach (World.Tile tile in m_world.GetGrid().GetCell(point.X, point.Y).GetTiles())
                {
                    tile.GetGO().GetComponent<MeshRenderer>().material.color = Color.black;
                }
            }

            if (LevelType != LevelType.Ambush)
            {
                foreach (World.Tile tile in m_world.GetGrid().GetCell(ObjectivePoint.X, ObjectivePoint.Y).GetTiles())
                {
                    tile.GetGO().GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.4f, 0.1f);
                }

            }
        }

        if (LevelType != LevelType.Ambush)
        {
            foreach (World.Tile tile in m_world.GetGrid().GetCell(EndPoint.X, EndPoint.Y).GetTiles())
            {
                tile.GetGO().GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }


}
