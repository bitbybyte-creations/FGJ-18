using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizedActor : MonoBehaviour {
    
    private int m_cooldown = 0;
    public event System.Action<bool> OnTurnStatusChange;

    public int ActionTime
    {
        get
        {
            return m_cooldown;
        }
        set
        {
            m_cooldown = value;
        }
    }

    public abstract class SyncAction
    {
        public abstract void Execute();
    }

    private SyncAction m_next;

    public bool HasNextAction
    {
        get
        {
            return m_next != null;
        }        
    }

    public void ResolveAction()
    {
        Debug.Log("Resolving Next Action");
        if (m_next != null)
        {
            Debug.Log(name +" Executing Action");
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
        Debug.Log("Actor: " + name + " Begin Turn");
        if (OnTurnStatusChange != null)
            OnTurnStatusChange(true);
    }
    public void EndTurn()
    {
        Debug.Log("Actor: " + name + " End Turn");
        if (OnTurnStatusChange != null)
            OnTurnStatusChange(false);
    }

	
}
