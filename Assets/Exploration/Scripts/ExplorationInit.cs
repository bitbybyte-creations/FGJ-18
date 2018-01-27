using UnityEngine;
using System.Collections;

public class ExplorationInit : MonoBehaviour
{
    private World m_world;
    // Use this for initialization
    void Start()
    {
        Map map = Loader.LoadFloorTileMap(64);
        map.GetTiles()[32, 32] = '#';
        m_world = new World(map);
        m_world.Draw();
        Invoke("Init", 2f);
    }

    public void Init()
    {
        
        ///baash stooppoid
        Debug.Log("Synchronizer Create");
        Synchronizer.Start();
    }
}
