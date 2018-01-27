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

public class Ping : MonoBehaviour {

    [Header("Various components")]
    public Animator animator_;
    public Image pingImage_;
    public Text typeText_;
    public Text distanceText_;
    public Text costText_;
    public Transform pingTransform;
    public Transform pingInteractTransform_;
    public Button pingButton_;

    [Header("Actual variables")]
    public float size_ = 1f;
    public Color color_ = Color.white;
    public PING_TYPES type_ = PING_TYPES.ENERGY;

    private bool display_ = false;
    private PlayerWorldMap player_;

	// Use this for initialization
	void Start () {

        size = size_;
        type = type_;
        
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
            pingTransform.localScale = new Vector3(value, value);
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
                    typeText_.text = "TYPE: ENERGY";
                    distanceText_.text = string.Format("Distance: {0}", ((int)distance));
                    costText_.text = WorldMapController.instance_.player.EnergyCost(transform.position).ToString();
                    break;
                }
            case PING_TYPES.ARTIFACT:
                {
                    color = Color.yellow;
                    typeText_.text = "TYPE: ARTIFACT";
                    distanceText_.text = string.Format("Distance: {0}", ((int)distance));
                    costText_.text = WorldMapController.instance_.player.EnergyCost(transform.position).ToString();
                    break;
                }
            case PING_TYPES.SIGNAL:
                {
                    color = Color.red;
                    typeText_.text = "TYPE: UNKNOWN";
                    distanceText_.text = string.Format("Distance: {0}", ((int)distance));
                    costText_.text = WorldMapController.instance_.player.EnergyCost(transform.position).ToString();
                    break;
                }
            case PING_TYPES.PLAYER:
                {
                    color = Color.blue;
                    typeText_.text = "TYPE: SELF";
                    distanceText_.text = string.Format("ENERGY LEFT: {0}", (int)WorldMapController.instance_.player.energyLeft);
                    costText_.text = WorldMapController.instance_.player.EnergyCost(transform.position).ToString();
                    break;
                }
            case PING_TYPES.END:
                {
                    color = Color.white;
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
        // Displays the info text box on top of the cursor
        if (type_ != PING_TYPES.NONE)
        {
            display_ = display;
            LeanTween.alphaCanvas(pingInteractTransform_.GetComponent<CanvasGroup>(), display?1f:0f, .1f);
            Cursor.visible = !display;
        };
        // Checks distance and compares to player energy, and makes the button interactable or no based on that
        if (WorldMapController.instance_.player.EnergyCost(transform.position) < WorldMapController.instance_.player.energyLeft && type_ != PING_TYPES.PLAYER)
        {
            pingButton_.interactable = true;
        }
        else
        {
            pingButton_.interactable = false;
            // We don't need the button if this is the player!
            if (type_ == PING_TYPES.PLAYER)
            {
                pingButton_.gameObject.SetActive(false);
            }
        }
        costText_.text = WorldMapController.instance_.player.EnergyCost(transform.position).ToString();

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

    void Update()
    {
        if (display_)
        {
            pingInteractTransform_.position = Input.mousePosition;
            UpdateTypeTexts(type_);
        };
    }

	
	
}
