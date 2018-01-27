using UnityEngine;
using System.Collections;

public class CombatSolver
{
    public static AttackingEntity.AttackResult Fight(SynchronizedActor actor, SynchronizedActor target)
    {
        Debug.Log("Begin Attack: " + actor.Stats.Name + " vs. " + target.Stats.Name);
        Entity tentity = target.Entity;
        int x, y;
        tentity.GetPosition(out x, out y);
        AttackingEntity.AttackResult ares = actor.AttackingEntity.Attack(x, y);        
        if (ares != null && (ares.Result == AttackingEntity.AttackResult.ResultValue.Hit))
        {
            int dmg = ares.Weapon.GetDamage();
            tentity.Stats.Health -= dmg;
            tentity.Actor.Moan(dmg);

        }
        return ares;
    }
}