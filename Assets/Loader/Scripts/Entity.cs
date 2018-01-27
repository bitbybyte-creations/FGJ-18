using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity
{
    private World.Cell _pos;

    public Entity(int x, int y)
    {
        Move(x, y);
    }

    public World.Cell GetCell()
    {
        return _pos;
    }

    public void Move(int x, int y)
    {
        _pos.SetEntity(null);
        _pos = World.GetInstance().GetGrid().GetCell(x, y);
        _pos.SetEntity(this);
    }
}

public class Player : Entity
{

    public Player(int x, int y) : base(x, y)
    {
        
    }
}

public class Monster : Entity
{
    public Monster(int x, int y) : base(x, y)
    {

    }
}
