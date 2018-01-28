using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public class Constants
    {
        public const float ROAM_MOVE_CHANCE = 0.25f;
        public const int DEFAULT_IDLE_DURATION = 10;
    }

    public enum Behaviour
    {
        Passive,
        Roam,
        Hunt
    }

    public Behaviour currentBehaviour = Behaviour.Roam;
    private MovingEntity m_movingEntity;
    private SynchronizedActor m_actor;
    private bool m_myTurn;
    // Use this for initialization
    void Start () {
        m_movingEntity = GetComponent<MovingEntity>();
        m_actor = GetComponent<SynchronizedActor>();
        m_actor.OnTurnStatusChange += OnTurnStatusChange;
	}

    private void OnTurnStatusChange(bool myTurn)
    {
        //Debug.Log("Enemy: " + name + " turn status: " + myTurn);
        m_myTurn = myTurn;
    }

    // Update is called once per frame
    void Update () {
        Debug.Log(currentBehaviour);
        if (m_myTurn)
        {
            switch (currentBehaviour)
            {
                case Behaviour.Passive:
                    break;
                case Behaviour.Roam:
                    float random = UnityEngine.Random.value;
                    if(random < Constants.ROAM_MOVE_CHANCE)
                    {                        
                        Vector2 dir = UnityEngine.Random.insideUnitCircle.normalized;
                        m_movingEntity.Move(dir);
                        Synchronizer.Continue(m_actor, m_movingEntity.moveActionCost);
                    }
                    else
                    {
                        m_actor.AssignAction(new SynchronizedActor.IdleAction(m_actor, Constants.DEFAULT_IDLE_DURATION));
                        Synchronizer.Continue(m_actor, Constants.DEFAULT_IDLE_DURATION);
                    }
                    break;
                case Behaviour.Hunt:
                    int[,] heatMap = World.Instance.GetHeatMap();
                    int x = -1;
                    int y = -1;
                    m_actor.Entity.GetPosition(out x, out y);

                    int xm = heatMap.GetLength(0);
                    int ym = heatMap.GetLength(1);

                    int xt = -1;
                    int yt = -1;
                    int cost = int.MaxValue;

                    if (x + 1 < xm && heatMap[x + 1, y] < cost && -1 < heatMap[x + 1, y])
                    {
                        cost = heatMap[x + 1, y];
                        xt = 1;
                        yt = 0;
                    }
                    if (y + 1 < ym && heatMap[x, y + 1] < cost && -1 < heatMap[x, y + 1])
                    {
                        cost = heatMap[x, y + 1];
                        xt = 0;
                        yt = 1;
                    }
                    if (-1 < x - 1 && heatMap[x - 1, y] < cost && -1 < heatMap[x - 1, y])
                    {
                        cost = heatMap[x - 1, y];
                        xt = -1;
                        yt = 0;
                    }
                    if (-1 < x - 1 && heatMap[x, y - 1] < cost && -1 < heatMap[x, y - 1])
                    {
                        cost = heatMap[x, y - 1];
                        xt = 0;
                        yt = -1;
                    }
                    
                    m_movingEntity.Move(new Vector2(xt, yt));
                    Synchronizer.Continue(m_actor, m_movingEntity.moveActionCost);

                    break;
            }
        }
	}
}
