using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponList
{
    public static Weapon Fist;
    public static Weapon Blaster;
    public static Weapon MutantFist;
    public static Weapon MutantClaw;
    public static Weapon MutantHammer;


    static WeaponList()
    {
        Fist = new Weapon("Fist", 1, 10, 0.75f, 1, 0, 6);
        Blaster = new Weapon("Blaster", 15, 25, 0.50f, 7, 1, 10);
        MutantFist = new Weapon("MutantFist", 1, 3, 0.25f, 1, 0, 8);
        MutantClaw = new Weapon("MutantClaw", 3, 6, 0.25f, 1, 0, 5);
        MutantHammer = new Weapon("MutantHammer", 5, 10, 0.25f, 1, 0, 15);
    }

}

public class Weapon
{
    private string m_name;
    private int m_minDamage;
    private int m_maxDamage;
    private int m_range;
    private float m_baseHitChance;
    private int m_energyCost;
    private int m_timeCost;

    public string Name { get { return m_name; } set { m_name = value; } }
    public int MinDamage { get { return m_minDamage; } set { m_minDamage = value; } }
    public int MaxDamage { get { return m_maxDamage; } set { m_maxDamage = value; } }
    public int Range { get { return m_range; } set { m_range = value; } }
    public float HitChance { get { return m_baseHitChance; } set { m_baseHitChance = value; } }
    public int EnergyCost { get { return m_energyCost; } set { m_energyCost = value; } }
    public int TimeCost { get { return m_timeCost; } set { m_timeCost = value; } }

    public float DiceRoll
    {
        get { return UnityEngine.Random.value; }
    }
    public Weapon(string name, int minDmg, int maxDmg, float hitChance, int range, int eCost, int tCost)
    {
        m_name = name;
        m_minDamage = minDmg;
        m_maxDamage = maxDmg;
        m_baseHitChance = hitChance;
        m_range = range;
        m_energyCost = eCost;
        m_timeCost = tCost;
    }

    public virtual bool Hit(float skill)
    {
        return DiceRoll <= (m_baseHitChance + skill);
    }

    public virtual int GetDamage()
    {
        return UnityEngine.Random.Range(m_minDamage, m_maxDamage);
    }
}

public class AttackingEntity : MonoBehaviour
{
    public  enum WeaponSlot
    {
        Primary,
        Secondary
    }
    private Weapon[] m_weapons = new Weapon[2];
    
    private SynchronizedActor m_actor;
        
    public class AttackResult
    {
        private ResultValue m_result;
        private Weapon m_weapon;

        public ResultValue Result
        {
            get
            {
                return m_result;
            }
        }

        public Weapon Weapon
        {
            get
            {
                return m_weapon;
            }
        }

        public enum ResultValue
        {
            Hit,
            Miss,
            OutOfRange,
            OutOfSight,
            NoTarget,
            NoEnergy
        }

        public AttackResult(ResultValue result, Weapon weapon)
        {
            m_result = result;
            m_weapon = weapon;
        }
    }

    public Weapon AssignWeapon(WeaponSlot slot, Weapon weapon)
    {
        Weapon old = m_weapons[(int)slot];
        m_weapons[(int)slot] = weapon;
        return old;
    }

    // Use this for initialization
    void Start()
    {
        m_actor = GetComponent<SynchronizedActor>();
        if (m_actor == null)
            m_actor = gameObject.AddComponent<SynchronizedActor>();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public AttackResult Attack(Entity entity)
    {
        int x, y;
        entity.GetPosition(out x, out y);
        return Attack(x, y);
    }

    public AttackResult Attack(int x, int y)
    {
        AttackResult result = null;
        World.Cell cell = World.Instance.GetGrid().GetCell(x, y);
        int distance = (int)(new Vector2((float)x, (float)y) - m_actor.Entity.GetPositionVector()).magnitude;
        if (!cell.ContainsEntity)
        {
            Debug.Log("No Target");
            return new AttackResult(AttackResult.ResultValue.NoTarget, null);
        }
        

        foreach (Weapon wep in m_weapons)
        {
            if (wep == null)
            {
                Debug.Log("Null weapon");
                break;
            }
            Debug.Log("Using weapon: " + wep.Name);
            if (wep.EnergyCost <= m_actor.Stats.Energy)             //has energy
            {
                Debug.Log("Has Energy");
                if (distance > wep.Range)                           //within range
                {
                    Debug.Log("Out of range");
                    result = new AttackResult(AttackResult.ResultValue.OutOfRange, wep);
                }
                else
                {
                    Debug.Log("Within Range");
                    if (wep.EnergyCost > 0)
                        m_actor.Stats.Energy -= wep.EnergyCost;
                    if (wep.Hit(m_actor.Stats.Attack))              //ghit
                    {
                        Debug.Log("Hit");
                        result = new AttackResult(AttackResult.ResultValue.Hit, wep);
                    }
                    else
                    {
                        Debug.Log("Miss");
                        result = new AttackResult(AttackResult.ResultValue.Miss, wep);
                    }
                    break;
                }
            }
            else
            {
                Debug.Log("Out of Energy");
                result = new AttackResult(AttackResult.ResultValue.NoEnergy, wep);
            }
        }

        return result;
    }
       
}
