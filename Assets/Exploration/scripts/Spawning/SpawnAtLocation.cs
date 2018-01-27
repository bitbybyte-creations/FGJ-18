using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAtLocation : MonoBehaviour {

    public GameObject prefab;

    public int xPosition;
    public int yPosition;
    public float heightOffset = 0.5f;
	// Use this for initialization
	void Start () {
        startSpawn();
	}

    protected virtual void startSpawn()
    {
        Spawn(xPosition, yPosition);
    }

    public void Spawn(int x, int y)
    {
        Vector3 position = new Vector3(x, heightOffset, y);
        GameObject spawn = Instantiate<GameObject>(prefab, position, new Quaternion() );

    }
}
