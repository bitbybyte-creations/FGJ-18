using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity
{
    private static int _nextId = 0;
    private World.Cell _pos;
    private SynchronizedActor _actor;
    private int _x, _y;
    private int _id;
    private EntityStatistics m_stats;

    public EntityStatistics Stats
    {
        get
        {
            return m_stats;
        }
        protected set { m_stats = value; }
    }
    public void GetPosition(out int x, out int y)
    {        
        x = _x;
        y = _y;        
    }
    public Vector2 GetPositionVector()
    {
        return new Vector2(_x, _y);
    }

    public Entity(int x, int y)
    {
        _id = _nextId++;
        _x = x;
        _y = y;
        Move(x, y);
    }

    public SynchronizedActor Actor
    {
        get
        {
            return _actor;
        }
        set
        {
            _actor = value;
        }
    }

    public World.Cell GetCell()
    {
        return _pos;
    }

    public void Move(int x, int y)
    {
        //Debug.Log("Entity " + _id + " Move to " + x + ", " + y);
        _x = x;
        _y = y;
        if (_pos != null)
            _pos.SetEntity(null);
        _pos = World.GetInstance().GetGrid().GetCell(x, y);
        _pos.SetEntity(this);
    }    
}

public class Player : Entity
{

    public Player(int x, int y, EntityStatistics stats) : base(x, y)
    {
        Stats = stats;
        GenerateHeatMap(x, y);
    }

    public new void Move(int x, int y)
    {
        base.Move(x, y);
        GenerateHeatMap(x, y);
    }

    public int[,] GenerateHeatMap(int x, int y)
    {
        World w = World.Instance;
        World.Cell[,] c = w.GetGrid().GetCells();

        int[,] heatMap = new int[c.GetLength(0), c.GetLength(1)];
        for (int i = 0; i < c.GetLength(0); i++)
            for (int j = 0; j < c.GetLength(1); j++)
                heatMap[i, j] = -1;

        Queue<Pos> queue = new Queue<Pos>();
        Pos pos = new Pos(x, y);
        queue.Enqueue(pos);
        heatMap[pos.x, pos.y] = 0; ;
        int xm = c.GetLength(0);
        int ym = c.GetLength(1);

        while (queue.Count > 0)
        {
            Pos cell = queue.Dequeue();
            int cost = heatMap[cell.x, cell.y] + 1;
            if (cell.x + 1 < xm && !c[cell.x + 1, cell.y].IsBlocked && heatMap[cell.x + 1, cell.y] == -1)
            {
                heatMap[cell.x + 1, cell.y] = cost;
                queue.Enqueue(new Pos(cell.x + 1, cell.y));
            }
            if (cell.y + 1 < ym && !c[cell.x, cell.y + 1].IsBlocked && heatMap[cell.x, cell.y + 1] == -1)
            {
                heatMap[cell.x, cell.y + 1] = cost;
                queue.Enqueue(new Pos(cell.x, cell.y + 1));
            }
            if (!(cell.x - 1 < 0) && !c[cell.x - 1, cell.y].IsBlocked && heatMap[cell.x - 1, cell.y] == -1)
            {
                heatMap[cell.x - 1, cell.y] = cost;
                queue.Enqueue(new Pos(cell.x - 1, cell.y));
            }
            if (!(cell.y - 1 < 0) && !c[cell.x, cell.y - 1].IsBlocked && heatMap[cell.x, cell.y - 1] == -1)
            {
                heatMap[cell.x, cell.y - 1] = cost;
                queue.Enqueue(new Pos(cell.x, cell.y - 1));
            }
        }

        w.SetHeatMap(heatMap);
        return heatMap;
    }

    class Pos
    {
        public int x;
        public int y;

        public Pos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

}

public class Monster : Entity
{
    public Monster(int x, int y, EntityStatistics stats) : base(x, y)
    {
        Stats = stats;
    }
}
