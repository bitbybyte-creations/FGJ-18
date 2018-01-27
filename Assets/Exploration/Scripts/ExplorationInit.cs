using UnityEngine;
using System.Collections;

public class ExplorationInit : MonoBehaviour
{
    private World m_world;
    // Use this for initialization
    void Start()
    {
        m_world = World.InitPlain();
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
