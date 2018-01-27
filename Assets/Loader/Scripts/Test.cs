using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    
    void Start () {

        World.Instance.InitTest().Draw();
    }


    void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                int x = (int)(hit.point.x + 0.5);
                int y = (int)(hit.point.y + 0.5);
                int z = (int)(hit.point.z + 0.5);
                Vector3 loc = new Vector3(x, y, z);
                Debug.Log(loc);
            }
        }
    }
}


