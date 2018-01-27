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

    private static Ping[] allPings_;

    private TypeWriter typeWriter_;

    public string path_ = "WorldMapPrefabs/prefab_Ping";

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
    public Ping[] allPings {
        get {
           
            return allPings_;
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
        typeWriter.ClearWriter();
        
    }

    public void ShowEncounterButton(bool show = true) {

        if (EncounterUI_ == null) {
            EncounterUI_ = GameObject.Find("EncounterUI");
            GoToEncounterButton_ = EncounterUI_.transform.Find("ExploreButton").GetComponent<Button>();
        }
        GoToEncounterButton_.gameObject.SetActive(show);
    }

    public Ping SpawnPingType(PING_TYPES type) {

        Ping returnPing;
        GameObject pingObj = null;
        pingObj = (GameObject)Instantiate(Resources.Load(path_));
        returnPing = pingObj.GetComponent<Ping>();
        returnPing.type = type;
        return returnPing;
    }

    public void InitializeWorldMap() {
        // Gather all zones
        PingZone[] allZones = FindObjectsOfType<PingZone>();

        foreach (PingZone pz in allZones) {
            //I could add it directly but it looks messy
            Ping newPing = pz.Initialize();
            Debug.Log(newPing);
        };

        allPings_ = FindObjectsOfType<Ping>();
        Debug.Log("All pings nr: " + allPings.Length);

        // Hides all the pings that aren't the end ping or the player ping
        foreach (Ping ping in allPings_) {

            if (ping.type_ != PING_TYPES.END || ping.type_ !=PING_TYPES.PLAYER) {
                ping.SetPingState(PING_STATES.HIDDEN);
            }
            else if (ping.type_ == PING_TYPES.END) {
                ping.SetPingState(PING_STATES.PINGED);
            }
        }
    }

}
