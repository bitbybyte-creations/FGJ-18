using UnityEngine;
using System.Collections;

public class ExplorationInit : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Map map = Loader.LoadFloorTileMap(64);
        map.GetTiles()[32, 32] = '#';
        World.Instance.Init(map).Draw();

        Invoke("Init", 2f);
    }

    

    public void Init()
    {
        
        ///baash stooppoid
        Debug.Log("Synchronizer Create");
        Synchronizer.Start();
    }
}
