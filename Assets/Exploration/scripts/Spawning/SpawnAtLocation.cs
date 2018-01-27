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
        //startSpawn();
        World.OnWorldInitialized(startSpawn);
	}

    protected virtual void startSpawn(World world)
    {
        Spawn(xPosition, yPosition);
    }

    public void Spawn(int x, int y)
    {
        World.Grid grid = World.Instance.GetGrid();
        World.Cell cell = grid.GetCell(x, y);
        if (!cell.IsBlocked)
        {            
            //Vector3 position = new Vector3(x, heightOffset, y);            
            GameObject spawn = Instantiate<GameObject>(prefab);
            spawn.transform.position = spawn.transform.position + new Vector3(0, heightOffset, 0);
            SynchronizedActor actor = spawn.GetComponent<SynchronizedActor>();
            if (actor != null)
            {
                Entity entity = null;
                if (actor.tag == "Player")
                {
                    entity= new Player(x, y);
                }
                else if (actor.tag == "Enemy")
                {
                    entity = new Monster(x, y);
                }
                if (entity != null)
                    actor.InitActor(entity);
            }
        }
        
        

    }
}
