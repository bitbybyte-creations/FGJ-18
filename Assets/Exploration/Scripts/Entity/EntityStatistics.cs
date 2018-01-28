using UnityEngine;
using System.Collections;

public class EntityStatistics
{
    private string m_name = "Blerbo";
    private int m_energy = 100;
    private float m_attack = 0.10f;
    private int m_health = 10;
    private int m_moveTime = 10;
    public string Name { get { return m_name; }internal set { m_name = value; if (OnStatsChangedEvent != null) OnStatsChangedEvent(); } }
    public int Energy { get { return m_energy; } internal set { m_energy = value; if (OnStatsChangedEvent != null) OnStatsChangedEvent(); } }
    public float Attack { get { return m_attack; } internal set { m_attack = value; if (OnStatsChangedEvent != null) OnStatsChangedEvent(); } }
    public int Health { get { return m_health; } internal set { m_health = value; if (OnStatsChangedEvent != null) OnStatsChangedEvent(); } }
    public int MoveTime { get { return m_moveTime; } internal set { m_moveTime = value; if (OnStatsChangedEvent != null) OnStatsChangedEvent(); } }

    public string SpeedString
    {
        get
        {
            return string.Format("{0:P0}", 10f / (float)MoveTime);
        }
    }

    public string AttackString
    {
        get
        {
            return string.Format("{0:P0}", Attack);
        }
    }

    public event System.Action OnStatsChangedEvent;

    public EntityStatistics(string entityName)
    {
        m_name = entityName;
    }

    public static readonly EntityStatistics MutantHeavy;
    public static readonly EntityStatistics MutantWalker;
    public static readonly EntityStatistics MutantStalker;
    static EntityStatistics()
    {
        MutantWalker = new EntityStatistics("Walker");

        MutantWalker.Attack = Random.Range(0.25f,0.40f);
        MutantWalker.Health = Random.Range(15,25);
        MutantWalker.MoveTime = Random.Range(11,13);

        MutantHeavy = new EntityStatistics("Heavy");
        MutantHeavy.Attack = Random.Range(0.10f,0.25f);
        MutantHeavy.Health = Random.Range(30,50);
        MutantHeavy.MoveTime = Random.Range(13,16);

        MutantStalker = new EntityStatistics("Hunter");
        MutantStalker.Attack = Random.Range(0.40f,0.50f);
        MutantStalker.Health = Random.Range(10,16);
        MutantStalker.MoveTime = Random.Range(7,9);
    }

    public static EntityStatistics GetRandomPlayerStats()
    {
        EntityStatistics stats = new EntityStatistics("The Astronaut");
        stats.Energy = Random.Range(90, 140);
        stats.Health = Random.Range(15, 20);
        stats.Attack = Random.Range(0.10f, 0.25f);
        stats.MoveTime = Random.Range(8, 12);
        return stats;
    }
    
    public EntityStatistics Clone()
    {
        EntityStatistics res = new EntityStatistics(Name);
        res.Attack = Attack;
        res.Health = Health;
        res.MoveTime = MoveTime;
        return res;

    }
}
