using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingEntity : MonoBehaviour {

    public enum AttackType
    {
        Melee,
        Ranged
    }
    public class AttackResult
    {
        public enum ResultValue
        {
            Hit,
            Miss,
            OutOfRange,
            OutOfSight
        }
    }



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public AttackResult Attack(int x, int y)
    {

        AttackResult result = null;

        return result;
    }
}
