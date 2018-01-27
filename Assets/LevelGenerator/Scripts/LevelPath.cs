using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[System.Serializable]
public struct LevelPoint
{
    public int X;
    public int Y;

}


public class LevelPath
{

    private int m_startX;
    private int m_startY;
    private int m_minSize;
    private int m_maxSize;
    private int m_levelWith;
    private int m_levelHeigth;

    private static Random Rand = new Random();

    private LevelPoint[] m_points = null;

    public LevelPath(int startX, int startY, int minSize, int maxSize, int levelWith, int levelHeigth)
    {
        m_startX = startX;
        m_startY = startY;
        m_minSize = minSize;
        m_maxSize = maxSize;
        m_levelHeigth = levelHeigth;
        m_levelWith = levelHeigth;
    }

    private LevelPoint m_direction;
    private LevelPoint m_lastDirection;
    private void changeDirection()
    {
        m_direction.X = 0;
        m_direction.Y = 0;

        int dir = Rand.Next(10000) % 4;
        switch (dir)
        {
            case 0:
                m_direction.X = -1;
                break;
            case 1:
                m_direction.Y = -1;
                break;
            case 2:
                m_direction.X = 1;
                break;
            case 3:
                m_direction.Y = 1;
                break;
            case 4:
                m_direction.X = 1;
                m_direction.Y = 1;
                break;
        }

        if(m_direction.X == m_lastDirection.Y && m_direction.Y == m_lastDirection.Y)
        {
            changeDirection();
        }
        else
        {
            m_lastDirection = m_direction;
        }

    }
    public LevelPoint[] RunPath()
    {


        if(m_points != null)
        {
            return m_points;
        }

        changeDirection();
        int len = Rand.Next(m_minSize, m_maxSize);

        LevelPoint[] path = new LevelPoint[len];

        path[0].X = m_startX;
        path[0].Y = m_startY;
        
        for(int i = 1;i < len; i++)
        {
try_again:
            //should change direction
            if(Rand.Next(1000) % 3 == 0)
            {
                changeDirection();
            }


            path[i].X = path[i - 1].X + m_direction.X;
            path[i].Y = path[i - 1].Y + m_direction.Y;

            if(path[i].X >= m_levelWith || path[i].Y >= m_levelHeigth || path[i].X < 0 || path[i].Y < 0)
            {
                goto try_again;
            }

        }
        m_points = path;
        return path;
    }

}

