using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapController : Object {

    private static WorldMapController _Instance;

    private PlayerWorldMap player_;

    public static WorldMapController instance_
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new WorldMapController();
            }
            return _Instance;
        }
    }

    public PlayerWorldMap player
    {
        get
        {
           if (player_ == null)
            {
                player_ = FindObjectOfType<PlayerWorldMap>();
            }
            return player_;
        }
    }

    void Start()
    {


    }

}
