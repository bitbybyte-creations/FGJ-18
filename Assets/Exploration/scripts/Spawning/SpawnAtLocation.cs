﻿using System.Collections;
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
        Spawn(xPosition, yPosition, prefab);
    }

    public static void Spawn(int x, int y, GameObject prefab)
    {
        World.Grid grid = World.Instance.GetGrid();
        World.Cell cell = grid.GetCell(x, y);
        if (!cell.IsBlocked)
        {            
            //Vector3 position = new Vector3(x, heightOffset, y);            
            GameObject spawn = Instantiate<GameObject>(prefab);
            spawn.transform.position = spawn.transform.position;
            SynchronizedActor actor = spawn.GetComponent<SynchronizedActor>();
            if (actor != null)
            {
                Entity entity = null;
                if (actor.tag == "Player")
                {
                    entity= new Player(x, y, EntityStatistics.GetRandomPlayerStats());
                }
                else if (actor.tag == "Enemy")
                {
                    MonsterConfig config = spawn.GetComponent<MonsterConfig>();
                    entity = new Monster(x, y, config.GetStats());
                }
                if (entity != null)
                    actor.InitActor(entity);
            }
        }
        
        

    }
}
