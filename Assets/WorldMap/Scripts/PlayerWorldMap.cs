using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorldMap : MonoBehaviour {

    public float energyleft_ = 100f;
    public float energyEfficiency_ = 15f;
    public float speed_;
    public float pingStrength_ = 1f;
    public Ping selfPing_;
    public bool travelling_;

    public GameObject travelLine_;

    private RectTransform self_;
    private List<GameObject> spawnedLines_ = new List<GameObject>();
    private float maxEnergy_ = 100f;
    private int LeanTweenMoveID_;
    private int LeanTweenEnergyLossID_;
    private Coroutine DottedLineRoutine_;
    private Transform LineParent_;

    // Calculate energy cost using this
    public float EnergyCost(Vector2 position)
    {
        // Get distance
        float distance = Vector2.Distance(transform.position, position);
        // Divide by player speed
        distance /= energyEfficiency_;
        // Cut decimals
        distance = (int)distance;
        // and return
        return distance;
    }

    public float energyLeft
    {
        get
        {
            return energyleft_;
        }
        set
        {
            energyleft_ = value;
            Mathf.Clamp(energyleft_, 0, maxEnergy_);
        }
    }

    public void NewTravel(RectTransform target)
    {
        CancelTravel();
        float dist = Vector2.Distance(transform.position, target.transform.position);
        float duration = dist/speed_;
        Vector2 localPoint = target.position;
        float oldEnergy = energyLeft;
        float newEnergy = oldEnergy - EnergyCost(target.transform.position);
        

        //Debug.Log("Moving player for " + duration.ToString() + " seconds to position " + transform.position);
        LeanTweenMoveID_ = LeanTween.move(self_.gameObject, localPoint, duration).setOnComplete(()=>StartCoroutine(TravelFinished(target.GetComponent<Ping>()))).id;
        
        LeanTweenEnergyLossID_ = LeanTween.value(gameObject, DrainEnergy, oldEnergy, newEnergy, duration).id;
        DottedLineRoutine_ = StartCoroutine(Travelling(target, duration));
    }

    public void DrainEnergy(float amount)
    {
        //Debug.Log(amount);
        energyLeft = amount;
    }

    public void CancelTravel()
    {
        // Cancels travel, and energy expenditure
        LeanTween.cancel(LeanTweenMoveID_);
        LeanTween.cancel(LeanTweenEnergyLossID_);
        if (DottedLineRoutine_ != null)
        {
            StopCoroutine(DottedLineRoutine_);
        };
        WorldMapController.instance_.ZoomOutOfPing();
    }

    public IEnumerator Travelling(RectTransform target, float duration)
    {
        travelling_ = true;
        
        /*if (spawnedLines_.Count > 0)
        {
            yield return new WaitUntil(()=>spawnedLines_.Count == 0);
        }*/

        while (travelling_)
        {
            Vector2 selfPos = transform.position;
            Vector2 targetPos = target.transform.position;

            GameObject newLine = Instantiate(travelLine_, LineParent_);
            newLine.transform.localPosition = transform.localPosition;
            newLine.transform.SetAsFirstSibling();
            // Make lines face target
            var dir = targetPos - selfPos;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            angle -= 90;
            newLine.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            newLine.SetActive(true);
            spawnedLines_.Add(newLine);
            
            yield return new WaitForSeconds(.5f);
        };
    }
    public IEnumerator TravelFinished(Ping target)
    {
        travelling_ = false;
        // Delete some if the line count is getting too big
        if (spawnedLines_.Count > 100)
        {
            while (spawnedLines_.Count > 100)
            {
                Destroy(spawnedLines_[0]);
                spawnedLines_.RemoveAt(0);
                yield return new WaitForEndOfFrame();
            };
        }
        WorldMapController.instance_.ZoomIntoPing(target);

    }

    public void Ping() {
        Debug.Log("Pinging!");
        // Hide all pings we can't see
        // Query each ping in the scene and their distance, and if it's less than pingstrength, make it visible
        Debug.Log("List count: " + WorldMapController.instance_.allPings.Length.ToString());
        foreach (Ping ping in WorldMapController.instance_.allPings) {
            Debug.Log("Distance Pinged: " + ping.distance.ToString());
            if (ping.distance < 5f) {
                ping.SetPingState(PING_STATES.PINGED);
                Debug.Log("Pinged ping!");
            }
            else {
                ping.SetPingState(PING_STATES.HIDDEN);
            };
            StartCoroutine(PingAnims());
        }
    }
    public IEnumerator PingAnims() {
        // Animate our little pingy box nicely
        selfPing_.animator_.SetTrigger("StopPing");
        yield return new WaitForEndOfFrame();
        selfPing_.size = 800f;
        selfPing_.animator_.SetTrigger("Ping");
        yield return new WaitForSeconds(2f);
        selfPing_.UpdateTypeTexts(PING_TYPES.PLAYER);
    }

	// Use this for initialization
	void Start () {
        self_ = GetComponent<RectTransform>();
        LineParent_ = WorldMapController.instance_.worldMap.Find("LineParent");
        selfPing_.animator_.SetTrigger("Ping");

        // Start the game from here, lol

        WorldMapController.instance_.InitializeWorldMap();

	}
	
	// Update is called once per frame
	
}
