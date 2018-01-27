using UnityEngine;
using System.Collections;
using System;

public class MonsterConfig : MonoBehaviour
{
    public MonsterType type;
    public enum MonsterType
    {
        Walker,
        Heavy,
        Stalker
    }

    // Use this for initialization
    protected void Start()
    {
        AttackingEntity ae = GetComponent<AttackingEntity>();

        switch (type)
        {
            case MonsterType.Heavy:
                ae.AssignWeapon(AttackingEntity.WeaponSlot.Primary, WeaponList.MutantHammer);
                break;
            case MonsterType.Stalker:
                ae.AssignWeapon(AttackingEntity.WeaponSlot.Primary, WeaponList.MutantClaw);
                break;
            case MonsterType.Walker:
                ae.AssignWeapon(AttackingEntity.WeaponSlot.Primary, WeaponList.MutantFist);
                break;
        }
    }

    internal EntityStatistics GetStats()
    {
        switch (type)
        {
            case MonsterType.Heavy:
                return EntityStatistics.MutantHeavy;
            case MonsterType.Stalker:
                return EntityStatistics.MutantStalker;
            case MonsterType.Walker:
                return EntityStatistics.MutantWalker;
        }
        return null;
    }
}
