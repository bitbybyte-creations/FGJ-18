using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorldMap : MonoBehaviour {

    public float energyleft_ = 200f;
    public float speed_;

    public Vector2 travelTarget_;

    // Calculate energy cost using this
    public float EnergyCost(Vector2 position)
    {
        // Get distance
        float distance = Vector2.Distance(transform.position, position);
        // Divide by player speed
        distance /= speed_;
        // and return
        return distance;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
