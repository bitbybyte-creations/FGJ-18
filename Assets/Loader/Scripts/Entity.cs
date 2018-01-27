using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    
}

public class Player : IEntity
{
    private int _x;
    private int _y;

    public Player(int x, int y)
    {
        _x = x;
        _y = y;
    }
    public void GetPosition(out int x, out int y)
    {
        x = _x;
        y = _y;
    }

    public Vector2 PositionVector
    {
        get
        {
            return new Vector2(_x, _y);
        }
    }
}

public class Monster : IEntity
{
    
}
