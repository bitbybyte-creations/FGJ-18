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
        if (m_myTurn)
        {
            switch (currentBehaviour)
            {
                case Behaviour.Passive:
                    break;
                case Behaviour.Roam:
                    float random = UnityEngine.Random.value;
                    if(random <Constants.ROAM_MOVE_CHANCE)
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
                    break;
            }
        }
	}
}
