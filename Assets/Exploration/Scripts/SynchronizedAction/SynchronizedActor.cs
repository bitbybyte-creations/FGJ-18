using BitByByte.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizedActor : MonoBehaviour {
    
    private ulong m_nextAction = 0;
    public event System.Action<bool> OnTurnStatusChange;
    private Entity m_myEntity;

    public EntityStatistics Stats
    {
        get
        {
            return m_myEntity.Stats;
        }
    }

    public void InitActor(Entity entity)
    {        
        m_myEntity = entity;
        m_attackingEntity = GetComponent<AttackingEntity>();
        SynchronizeEntity();
        
    }

    private void SynchronizeEntity()
    {
        m_myEntity.Actor = this;
        Vector3 pos = transform.position;
        int x, y;
        m_myEntity.GetPosition(out x, out y);
        pos.x = x;
        pos.z = y;
        transform.position = pos;
        //Debug.Log("Synchronize Entity " + name + ", pos " + x + ',' + y);
    }

    public Entity Entity
    {
        get
        {
            return m_myEntity;
        }
    }

    public ulong ActionTime
    {
        get
        {
            return m_nextAction;
        }
        set
        {
            m_nextAction = value;
        }
    }

    public abstract class SyncAction
    {
        public abstract void Execute();
    }
    public class IdleAction : SyncAction
    {
        private SynchronizedActor m_actor;
        private int m_idleDuration;
        public IdleAction(SynchronizedActor actor, int timeStep)
        {
            m_actor = actor;
            m_idleDuration = timeStep;
        }
        public override void Execute()
        {
            
        }
    }


    private SyncAction m_next;
    private AttackingEntity m_attackingEntity;

    public bool HasNextAction
    {
        get
        {
            return m_next != null;
        }        
    }

    public AttackingEntity AttackingEntity { get { return m_attackingEntity; } }

    public void ResolveAction()
    {
        //Debug.Log("Resolving Next Action");
        if (m_next != null)
        {
            //Debug.Log(name +" Executing Action");
            m_next.Execute();
            m_next = null;
        }
    }

    /// <summary>
    /// return true if assign was ok, false if action already existsts
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool AssignAction(SyncAction action)
    {
        if (m_next ==null)
        {
            m_next = action;
            return true;
        }
        return false;
    }

    public void GiveTurn()
    {
        //Debug.Log("Actor: " + name + " Begin Turn");
        if (OnTurnStatusChange != null)
            OnTurnStatusChange(true);
    }

    internal void Moan(int dmg)
    {
        Debug.Log("I got damage: " + dmg);
        printDamage(dmg);
        bool dead = Stats.Health <= 0;
        if (dead)
        {
            Debug.Log(Stats.Name + " died...");            
            Synchronizer.KillEntity(this);
        }

        //todo print damage text;
    }

    private void printDamage(int dmg)
    {
        Transform canvas = GameObject.Find("_canvas").transform;
        BitByByte.UI.FloatingTextBuilder<FloatingText> text = new FloatingTextBuilder<FloatingText>(transform, canvas);
        text.Text = dmg.ToString();
        text.Color = Color.red;
        text.FontSize = 40;
        text.Velocity = new Vector3(0f, 30f, 0f);
        text.Build();

    }

    public void EndTurn()
    {
        //Debug.Log("Actor: " + name + " End Turn");
        if (OnTurnStatusChange != null)
            OnTurnStatusChange(false);
    }

	
}
