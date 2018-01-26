using UnityEngine;
using System.Collections;

public class ExplorationInit : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Invoke("Init", 2f);
    }

    public void Init()
    {
        ///baash stooppoid
        Debug.Log("Synchronizer Create");
        Synchronizer.Start();
    }
}
