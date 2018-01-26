using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synchronizer
{

    private static Synchronizer m_instance;

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

    public SynchronizedActor Player
    {
        get
        {
            return m_actorList[0];
        }
    }

    private List<SynchronizedActor> m_actorList = new List<SynchronizedActor>();
    private int m_time = 0;

    public static bool IsPlayerTurn { get { return Instance.Player.ActionTime == 0; } }

    public Synchronizer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        m_actorList.Add(player.GetComponent<SynchronizedActor>());
        GameObject[] enemas = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject go in enemas)
        {
            SynchronizedActor a = go.GetComponent<SynchronizedActor>();
            if (a != null)
                m_actorList.Add(a);
        }
        Debug.Log("Actors loaded from scene: " + m_actorList.Count);
    }

    internal static void Continue(SynchronizedActor actor, int actionCost)
    {
        Debug.Log(actor.name + " Action Complete");
        actor.ActionTime += actionCost;
        actor.EndTurn();
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
            next.GiveTurn();
        }


    }

    private void ForwardTime()
    {        
        foreach (SynchronizedActor actor in m_actorList)
        {
            Debug.Log(actor.name + " Next Action ? " + actor.HasNextAction);
            if (actor.HasNextAction)
            {
                actor.ResolveAction();
            }
        }
        m_time++;
        Debug.Log("Time: " + m_time);
    }

    private SynchronizedActor GetNextActorTurn()
    {
        foreach (SynchronizedActor a in m_actorList)
        {
            if (a.ActionTime <= m_time)
            {
                return a;
            }
        }
        return null;
    }
}
