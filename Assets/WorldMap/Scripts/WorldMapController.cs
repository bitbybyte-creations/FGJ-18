using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapController : Object {

    private static WorldMapController _Instance;

    private PlayerWorldMap player_;

    private Transform worldMap_;

    private Camera worldMapCamera_;

    private GameObject EncounterUI_;

    private Button GoToEncounterButton_;

    private TypeWriter typeWriter_;

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
    public Transform worldMap
    {
        get
        {
            if (worldMap_ == null)
            {
                worldMap_ = GameObject.Find("WorldMap").transform;
            };
            return worldMap_;
        }
    }
    public Camera worldMapCamera {
        get {
            if (worldMapCamera_ == null) {
                worldMapCamera_ = GameObject.FindGameObjectWithTag("WorldMapCamera").GetComponent<Camera>();
            }
            return worldMapCamera_;
        }
    }
    public TypeWriter typeWriter {
        get {
            if (typeWriter_ == null) {
                typeWriter_ = FindObjectOfType<TypeWriter>();
            }
            return typeWriter_;
        }
    }

    void Start()
    {
        

    }

    public void ZoomIntoPing(Ping target) {

        // Zooms world map camera to ping, and displays encounter data
        typeWriter.Write(target.encounter_.flavorText_);
    }

    public void ZoomOutOfPing() {
        // Zooms back to default position
        typeWriter.Write("");
        GoToEncounterButton_.gameObject.SetActive(false);
    }

    public void ShowEncounterButton() {

        if (EncounterUI_ == null) {
            EncounterUI_ = GameObject.Find("EncounterUI");
            GoToEncounterButton_ = EncounterUI_.transform.Find("ExploreButton").GetComponent<Button>();
            GoToEncounterButton_.gameObject.SetActive(true);
        }
    }

        void CreateRandomPings()
        {

        }

}
