using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAtLocation : MonoBehaviour {

    public GameObject prefab;
    public bool spawnAtPosition = false;
    public int xPosition;
    public int yPosition;
    public float heightOffset = 0.5f;
	// Use this for initialization
	void Start () {
        if (spawnAtPosition)
        {
            Spawn((int)transform.position.x, (int)transform.position.y);
        }
        else Spawn(xPosition, yPosition);
	}

    public void Spawn(int x, int y)
    {
        Vector3 position = new Vector3(x + 0.5f, heightOffset, y + 0.5f);
        GameObject spawn = Instantiate<GameObject>(prefab, position, new Quaternion() );

    }
}
