using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldMapController : Object {

    private static WorldMapController _Instance;

    private static PlayerWorldMap player_;

    private static float energy_;

    private static Transform worldMap_;

    private static Image fadeCanvas_;

    private static Camera worldMapCamera_;

    private static GameObject EncounterUI_;

    private static Button GoToEncounterButton_;

    private static Ping[] allPings_;

    private static TypeWriter typeWriter_;

    private Ping furthestPing_;

    private int LeanTweenId_;

    public static Ping currentPing_;

    public static string currentSceneName_;

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
                Debug.Log("Assigning world map player");
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
                Debug.Log("Assigning world map variable");
                worldMap_ = GameObject.Find("WorldMap").transform;
            };
            return worldMap_;
        }
    }
    public Image FadeCanvas
    {
        get
        {
            if (fadeCanvas_ == null)
            {
                Debug.Log("Assigning fade overlay canvas variable");
                fadeCanvas_ = GameObject.Find("FadePanel").GetComponent<Image>();
            };
            return fadeCanvas_;
        }
    }
    public Camera worldMapCamera {
        get {
            if (worldMapCamera_ == null) {
                Debug.Log("Assigning world map camera");
                worldMapCamera_ = GameObject.FindGameObjectWithTag("WorldMapCamera").GetComponent<Camera>();
            }
            return worldMapCamera_;
        }
    }
    public TypeWriter typeWriter {
        get {
            if (typeWriter_ == null) {
                Debug.Log("Assigning typewriter");
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

    public float energy {
        get {
            return energy_;
        }
        set {
            energy_ = value;
        }
    }

    public float AddEnergy {
        set {
            energy_ += value;
        }
    }

    void Start()
    {
        

    }

    public void ZoomIntoPing(Ping target) {

        // Zooms world map camera to ping, and displays encounter data
        typeWriter.Write(target.encounter_.flavorText_);
        float currentCameraZoom = worldMapCamera.orthographicSize;
        LeanTweenId_ = LeanTween.value(worldMapCamera.gameObject, SetCameraZoom, currentCameraZoom, 3f, .5f).setEase(LeanTweenType.easeInQuad).id;
        currentPing_ = target;
    }

    public void ZoomOutOfPing() {
        // Zooms back to default position
        typeWriter.ClearWriter();
        float currentCameraZoom = worldMapCamera.orthographicSize;
        LeanTweenId_ = LeanTween.value(worldMapCamera.gameObject, SetCameraZoom, currentCameraZoom, 5f, 2f).setEase(LeanTweenType.easeOutQuad).id;

        currentPing_ = null;
    }

    public void EnableMap(bool enable) {
        worldMap.gameObject.SetActive(enable);
    }

    public void StartEncounter() {

        Debug.Log("Clicked start encounter");
        // After clicking the explore button
        typeWriter.ClearWriter();
        player.allowTravel = false;
        float currentCameraZoom = worldMapCamera.orthographicSize;
        LeanTweenId_ = LeanTween.value(worldMapCamera.gameObject, SetCameraZoom, currentCameraZoom, 1f, 2f).setEase(LeanTweenType.easeOutQuad).id;
        FadeIn(2f, delegate { EnableMap(false); LoadScene(); });
    }

    public void LoadScene() {
        // Grab the data from the current ping encounter
        string sceneToLoad = currentPing_.encounter_.sceneToLoad_;
        Scene scene = SceneManager.GetSceneByName(sceneToLoad);
        //Load the scene and fade in
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        currentSceneName_ = sceneToLoad;
        FadeOut(1f,null,1f);
    }

    public void SceneFinished() {
        FadeOut(1f);
        worldMap.gameObject.SetActive(true);
        player.allowTravel = true;
        currentPing_.SetPingState(PING_STATES.EXPLORED);
        ZoomOutOfPing();
    }

    public void ZoomToVisiblePings()
    {
        // Zooms camera to encompass all visible pings
        furthestPing_ = null;
        foreach (Ping ping in allPings)
        {
            if (ping.state_ == PING_STATES.PINGED)
            {
                if (!ping.GetComponentInChildren<RectTransform>().IsVisibleFrom(worldMapCamera))
                {
                    furthestPing_ = ping;
                }
            };
        }
        if (furthestPing_ != null)
        {
            float currentCameraZoom = worldMapCamera.orthographicSize;
            LeanTweenId_ = LeanTween.value(worldMapCamera.gameObject, CanSeePoint, currentCameraZoom, 25f, 4f).setEase(LeanTweenType.easeOutQuad).id;
        }
        else
        {
            //Add a little extra
            float currentCameraZoom = worldMapCamera.orthographicSize;
            float targetZoom = currentCameraZoom * 1.25f;
            if (targetZoom < 10f) {
                LeanTween.value(worldMapCamera.gameObject, SetCameraZoom, currentCameraZoom, targetZoom, .5f).setEase(LeanTweenType.easeOutQuad);
            };
        }
    }

    public void SetCameraZoom(float value)
    {
        worldMapCamera.orthographicSize = value;
    }

    public void CanSeePoint(float newSize)
    {
        if (furthestPing_ != null)
        {
            if (!furthestPing_.GetComponentInChildren<RectTransform>().IsVisibleFrom(worldMapCamera))
            {
                //Debug.Log("Zooming!");
                worldMapCamera.orthographicSize = newSize;
            }
            else
            {
                LeanTween.cancel(LeanTweenId_);
                //Try again.
                ZoomToVisiblePings();
            }
        }
        else
        {
            worldMapCamera.orthographicSize = newSize;
        }
    }

    public void ShowEncounterButton(bool show = true) {

        if (EncounterUI_ == null) {
            EncounterUI_ = GameObject.Find("EncounterUI");
            GoToEncounterButton_ = EncounterUI_.transform.Find("ExploreButton").GetComponent<Button>();
            GoToEncounterButton_.gameObject.SetActive(true);
            GoToEncounterButton_.onClick.RemoveAllListeners();
            GoToEncounterButton_.onClick.AddListener(()=>WorldMapController.instance_.StartEncounter());
            Debug.Log("Should've added listener now");
            GoToEncounterButton_.gameObject.SetActive(false);
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
        // Fade!
        FadeCanvas.color = Color.black;
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
        //Zoom out!
        SetCameraZoom(10f);

        // Fade out!
        
        FadeOut(2f, ()=> typeWriter.Write("My scanner (button to the left) will pick up places of interest...", true, false));
        LeanTween.value(worldMapCamera.gameObject, SetCameraZoom, 10f, 5f, 2.5f).setEase(LeanTweenType.easeOutQuad).setDelay(2f);
    }

    public void FadeIn(float time, System.Action onComplete = null, float delay = 0f)
    {
        LeanTween.value(FadeCanvas.gameObject, Fader, 0f, 1f, time).setOnComplete(onComplete).setDelay(delay);
    }
    public void FadeOut(float time, System.Action onComplete = null, float delay = 0f)
    {
        LeanTween.value(FadeCanvas.gameObject, Fader, 1f, 0f, time).setOnComplete(onComplete).setDelay(delay);
    }

    void Fader(float newfade)
    {
        FadeCanvas.color = new Color(0, 0, 0, newfade);
    }

    
}


