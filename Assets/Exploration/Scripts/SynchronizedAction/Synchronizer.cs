using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synchronizer
{

    private static Synchronizer m_instance;

    public event Action OnAllEnemiesDiedEvent;
    public event Action OnPlayerDiedEvent;
    private bool m_started = false;

    public static Synchronizer Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new Synchronizer();
            }
            return m_instance;
        }
    }

    public static void Start()
    {
        Instance.m_started = true;
        Instance.init();
        SynchronizedActor a = Instance.GetNextActorTurn();
        if (a != null)
        {
            Debug.Log("Actor Found: " + a);
            a.GiveTurn();
        }
        else
        {
            Debug.Log("Null Actor!");
        }
    }

    private void init()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        m_player = player.GetComponent<SynchronizedActor>();
        m_actorList.Add(m_player);
        GameObject[] enemas = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject go in enemas)
        {
            SynchronizedActor a = go.GetComponent<SynchronizedActor>();
            if (a != null)
                m_actorList.Add(a);
        }
        Debug.Log("Actors loaded from scene: " + m_actorList.Count);
    }

    internal static void Reset()
    {
        Debug.LogError("Synchronizer Reset!");
        Instance.m_player = null;
        m_instance = null;
    }

    public SynchronizedActor Player
    {
        get
        {
            return m_player;
        }
    }

    private SynchronizedActor m_player;
    private List<SynchronizedActor> m_actorList = new List<SynchronizedActor>();
    private ulong m_time = 0;

    public static bool IsPlayerTurn { get { return Instance.Player.ActionTime == 0; } }

    public Synchronizer()
    {
        Debug.LogError("Synchronizer Constructed");        
    }

    internal static void Continue(SynchronizedActor actor, int actionCost)
    {
        if (Instance.m_started)
        {
            actor.ActionTime += (ulong)actionCost;
            actor.EndTurn();
            //Debug.Log(actor.name + " Action Complete, Next Action Time: "+actor.ActionTime);
            SynchronizedActor next = Instance.GetNextActorTurn();
            int count = 100;
            while (next == null && count > 0)
            {
                count--;
                Instance.ForwardTime();
                next = Instance.GetNextActorTurn();
            }
            if (count == 0)
                Debug.LogError("Continue Loop Timeout!");
            else
            {
                //Debug.Log("Giving Turn to " + next.name);
                next.GiveTurn();
            }
        }

    }

    private void ForwardTime()
    {
        if (Instance.m_started)
        {
            foreach (SynchronizedActor actor in m_actorList)
            {
                //Debug.Log(actor.name + " Next Action ? " + actor.HasNextAction);
                if (actor.HasNextAction)
                {
                    actor.ResolveAction();
                }
            }
            m_time++;
            //Debug.Log("Time: " + m_time);
        }
    }

    private SynchronizedActor GetNextActorTurn()
    {
        if (Instance.m_started)
        {
            foreach (SynchronizedActor a in m_actorList)
            {
                //Debug.Log(a.name + " AT: " + a.ActionTime + " vs. " + m_time);
                if ((ulong)a.ActionTime <= m_time)
                {
                    return a;
                }
            }
        }
        return null;

    }

    internal static void KillEntity(SynchronizedActor actor)
    {
        Instance.kill(actor);

    }
    private void kill(SynchronizedActor actor)
    {
        if (!m_started)
            return;
        for (int i = 0; i < m_actorList.Count; i++)
        {
            if (actor == m_actorList[i])
            {
                m_actorList.RemoveAt(i);
                break;
            }

        }
        Entity e = actor.Entity;
        actor.Entity.GetCell().SetEntity(null);
        actor.gameObject.SetActive(false);
        if (e.GetType() == typeof(Player))
        {
            m_player = null;
            if (OnPlayerDiedEvent != null)
            {
                OnPlayerDiedEvent();
            }
        }
        else if (m_actorList.Count == 1) // only player remains
        {
            if (OnAllEnemiesDiedEvent != null)
            {
                OnAllEnemiesDiedEvent();
            }
        }

    }
}
