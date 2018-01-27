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
        Debug.Log("Entity " + _id + " Move to " + x + ", " + y);
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
    }

}

public class Monster : Entity
{
    public Monster(int x, int y, EntityStatistics stats) : base(x, y)
    {
        Stats = stats;
    }
}
