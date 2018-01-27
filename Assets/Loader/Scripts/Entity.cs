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
}

public class Monster : IEntity
{
    
}
