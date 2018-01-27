using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PING_TYPES
{
    NONE = 9999,
    ENERGY = 1000,
    ARTIFACT = 2000,
    SIGNAL = 3000,
    PLAYER = 4000,
    END = 5000,
}

public enum PING_STATES {

    NONE = 9999,
    HIDDEN = 1000,
    PINGED = 2000,
    EXPLORED = 3000,
    DEAD = 4000,
}

public class Ping : MonoBehaviour {

    [Header("Various components")]
    public Animator animator_;
    public Image pingImage_;
    public Text typeText_;
    public Text distanceText_;
    public Text costText_;
    public RectTransform pingRectTransform_;
    public Transform pingInteractTransform_;
    public Button pingButton_;

    [Header("Actual variables")]
    public PING_TYPES type_ = PING_TYPES.ENERGY;
    public PING_STATES state_ = PING_STATES.PINGED;

    [Header("Data containers")]
    public WorldMapEncounter encounter_;

    private Color color_ = Color.white;
    private float size_ = 100f;
    private bool display_ = false;
    private PlayerWorldMap player_;
    private float encounterDistance_ = 1f;
    private bool playerIn_ = false;
    private Color colorOriginal_;
    private Camera WorldMapCamera_;
    private bool showPingInfo_;

	// Use this for initialization
	void Start () {

        type = type_;

        //encounterDistance_ = pingRectTransform_.Find("eventSystemImage").GetComponent<RectTransform>().rect.width;
        colorOriginal_ = color;
        WorldMapCamera_ = WorldMapController.instance_.worldMapCamera;
    }

    public Color color
    {
        get
        {
            return color_;
        }
        set
        {
            color_ = value;
            pingImage_.color = value;
        }
    }

    public float size
    {
        get
        {
            return size_;
        }
        set
        {
            pingRectTransform_.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
            pingRectTransform_.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
            size_ = value;
        }
    }

    public PING_TYPES type
    {
        get
        {
            return type_;
        }
        set
        {
            type_ = value;
            UpdateTypeTexts(type);
            
        }
    }

    public void UpdateTypeTexts(PING_TYPES type)
    {
        switch (type)
        {
            case PING_TYPES.ENERGY:
                {
                    color = Color.white;
                    size = 100f;
                    typeText_.text = "TYPE: ENERGY";
                    distanceText_.text = string.Format("Distance: {0}", ((int)distance));
                    costText_.text = WorldMapController.instance_.player.EnergyCost(transform.position).ToString();
                    break;
                }
            case PING_TYPES.ARTIFACT:
                {
                    color = Color.yellow;
                    size = 75f;
                    typeText_.text = "TYPE: ARTIFACT";
                    distanceText_.text = string.Format("Distance: {0}", ((int)distance));
                    costText_.text = WorldMapController.instance_.player.EnergyCost(transform.position).ToString();
                    break;
                }
            case PING_TYPES.SIGNAL:
                {
                    color = Color.red;
                    size = 75f;
                    typeText_.text = "TYPE: UNKNOWN";
                    distanceText_.text = string.Format("Distance: {0}", ((int)distance));
                    costText_.text = WorldMapController.instance_.player.EnergyCost(transform.position).ToString();
                    break;
                }
            case PING_TYPES.PLAYER:
                {
                    color = Color.blue;
                    size = 50f;
                    typeText_.text = "TYPE: SELF";
                    distanceText_.text = string.Format("ENERGY LEFT: {0}", (int)WorldMapController.instance_.player.energyLeft);
                    costText_.text = WorldMapController.instance_.player.EnergyCost(transform.position).ToString();
                    break;
                }
            case PING_TYPES.END:
                {
                    color = Color.white;
                    size = 500f;
                    typeText_.text = "TYPE: MASSIVE ENERGY READING";
                    distanceText_.text = string.Format("Distance: {0}", ((int)distance));
                    costText_.text = WorldMapController.instance_.player.EnergyCost(transform.position).ToString();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public float distance
    {
        get
        {
            return Vector2.Distance(transform.position, WorldMapController.instance_.player.transform.position);
        }
    }

    public void DisplayText(bool display)
    {
        if (showPingInfo_) {
            // Displays the info text box on top of the cursor
            if (type_ != PING_TYPES.NONE) {
                display_ = display;
                LeanTween.alphaCanvas(pingInteractTransform_.GetComponent<CanvasGroup>(), display ? 1f : 0f, .1f);
                Cursor.visible = !display;
                pingInteractTransform_.GetComponent<CanvasGroup>().interactable = display;
            };
            // Checks distance and compares to player energy, and makes the button interactable or no based on that
            if (WorldMapController.instance_.player.EnergyCost(transform.position) < WorldMapController.instance_.player.energyLeft && type_ != PING_TYPES.PLAYER) {
                pingButton_.interactable = true;
            }
            else {
                pingButton_.interactable = false;
                // We don't need the button if this is the player!
                if (type_ == PING_TYPES.PLAYER) {
                    pingButton_.gameObject.SetActive(false);
                }
            }
            costText_.text = WorldMapController.instance_.player.EnergyCost(transform.position).ToString();
        }
        else {
            display_ = false;
            pingInteractTransform_.GetComponent<CanvasGroup>().alpha = 0f;
            pingInteractTransform_.GetComponent<CanvasGroup>().interactable = false;
            Cursor.visible = true;
        }
    }

    public void ClickedPing()
    {
        DisplayText(false);
        // Tell the player to start moving
        if (pingButton_.interactable)
        {
            WorldMapController.instance_.player.NewTravel(GetComponent<RectTransform>());
        };
    }

    public void SetPingState(PING_STATES state) {

        // We don't change the state of the player's ping
        if (type_ != PING_TYPES.PLAYER) {
            // We don't change the state of dead or explored pings
            if (state_ != PING_STATES.DEAD || state_ != PING_STATES.EXPLORED) {

                state_ = state;
                switch (state) {

                    case PING_STATES.HIDDEN: {
                            showPingInfo_ = false;
                            DisplayText(false);
                            animator_.SetTrigger("StopPing");
                            break;
                        }
                    case PING_STATES.DEAD: {
                            showPingInfo_ = false;
                            DisplayText(false);
                            animator_.SetTrigger("StopPing");
                            break;
                        }
                    case PING_STATES.PINGED: {
                            showPingInfo_ = true;
                            animator_.SetTrigger("Ping");
                            break;
                        }
                    case PING_STATES.EXPLORED: {
                            showPingInfo_ = true;
                            animator_.SetTrigger("Ping");
                            break;
                        }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other);
    }

    void Update()
    {
        if (display_)
        {
            pingInteractTransform_.position = (Vector2)WorldMapCamera_.ScreenToWorldPoint(Input.mousePosition);
            UpdateTypeTexts(type_);
        };

        // This has too bad performance to be used.
        /*if (type_ != PING_TYPES.PLAYER) {
            if (state_ == PING_STATES.PINGED) {
                if (playerIn_) {
                    playerIn_ = true;
                    WorldMapController.instance_.ZoomIntoPing(this);
                    //Debug.Log("Player entered!");
                }
                else if (distance > encounterDistance_ && playerIn_) {
                    playerIn_ = false;
                    WorldMapController.instance_.ZoomOutOfPing();
                };
            };
        };*/
    }

	
	
}
