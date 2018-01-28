using UnityEngine;
using System.Collections;
using System;

public class ExplorationInit : MonoBehaviour
{
    public int monsterCount;
    public GameObject playerPrefab;
    public GameObject[] monsterPrefabs;
    private LevelBuilder m_builder;
    public event Action OnInitEvent;
    // Use this for initialization
    void Start()
    {
        //Map map = Loader.LoadFloorTileMap(64);
        //map.GetTiles()[32, 32] = '#';
        //World.Instance.Init(map).Draw();
        m_builder = GetComponent<LevelBuilder>();
        m_builder.Generate();
        SpawnMonsters(m_builder.EnemySpawnPoints);
        SpawnPlayer();
        Invoke("Init", 2f);
    }

    private void SpawnPlayer()
    {
        SpawnAtLocation.Spawn(m_builder.StartPoint.X, m_builder.StartPoint.Y, playerPrefab);
    }

    private void SpawnMonsters(LevelPoint[] enemySpawnPoints)
    {
        int count = 0;
        foreach (LevelPoint point in enemySpawnPoints)
        {
            int index = UnityEngine.Random.Range(0, monsterPrefabs.Length);
            SpawnAtLocation.Spawn(point.X, point.Y, monsterPrefabs[index]);
            count++;
            if (count == monsterCount)
                break;
        }
    }

    public void Init()
    {
        
        ///baash stooppoid
        Debug.Log("Synchronizer Create");
        Synchronizer.Start();
        GetComponent<PlayerUI>().Init();
        if (OnInitEvent != null)
            OnInitEvent();
    }

}
