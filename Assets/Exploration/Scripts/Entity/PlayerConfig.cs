using UnityEngine;
using System.Collections;

public class PlayerConfig : MonoBehaviour
{
    public Weapon primary = WeaponList.Fist;
    public Weapon secondary = WeaponList.Blaster;    

    // Use this for initialization
    protected void Start()
    {
        AttackingEntity e = GetComponent<AttackingEntity>();
        e.AssignWeapon(AttackingEntity.WeaponSlot.Primary, primary);
        if (secondary != null) {
            e.AssignWeapon(AttackingEntity.WeaponSlot.Secondary, secondary);
        }
    }
    
}
