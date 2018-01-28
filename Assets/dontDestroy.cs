using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroy : MonoBehaviour {

    public static bool blaa = false;
	// Use this for initialization
	void Start () {
        if (!blaa)
        {
            DontDestroyOnLoad(this);
            blaa = true;
        }
	}
}
