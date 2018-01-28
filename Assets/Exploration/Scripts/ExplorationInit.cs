using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

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
        Cursor.visible = true;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(WorldMapController.currentSceneName_));
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

            try
            {

                int index = UnityEngine.Random.Range(0, monsterPrefabs.Length);
                SpawnAtLocation.Spawn(point.X, point.Y, monsterPrefabs[index]);
                count++;
                if (count == monsterCount)
                    break;

            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
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

        WorldMapController.instance_.typeWriter.Write("I need to find the energy, marked in <color=#ff0>yellow</color>, or exit via <color=#f00>red tile</color>", true, false);        
        PlayerControls c = Synchronizer.Instance.Player.GetComponent<PlayerControls>();
        ReturnToWorldMap m = GetComponent<ReturnToWorldMap>();
        c.OnPlayerMovedToEndEvent += m.FinishScene;
        c.OnPlayerMovedToObjectiveEvent += delegate {            
            Synchronizer.Instance.Player.Entity.Stats.Energy += UnityEngine.Random.Range(60, 100);            
            WorldMapController.instance_.typeWriter.Write("This is what I'm looking for! It's time to leave... fast...", true, false);
        };
        Synchronizer.Instance.Player.Entity.Stats.Energy = (int)WorldMapController.instance_.energy;
        
        
    }

}
