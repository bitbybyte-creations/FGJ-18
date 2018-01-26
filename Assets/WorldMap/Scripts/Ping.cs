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
}

public class Ping : MonoBehaviour {

    [Header("Various components")]
    public Animator animator_;
    public Image pingImage_;
    public Text typeText_;
    public Text costText_;
    public Transform pingTransform;
    public Transform pingInteractTransform_;

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
            switch (type_)
            {
                case PING_TYPES.ENERGY:
                    {
                        color = Color.white;
                        typeText_.text = "TYPE: ENERGY";
                        costText_.text = string.Format("Distance: {0}", ((int)distance));
                        break;
                    }
                case PING_TYPES.ARTIFACT:
                    {
                        color = Color.yellow;
                        typeText_.text = "TYPE: ARTIFACT";
                        costText_.text = string.Format("Distance: {0}", ((int)distance));
                        break;
                    }
                case PING_TYPES.SIGNAL:
                    {
                        color = Color.red;
                        typeText_.text = "TYPE: UNKNOWN";
                        costText_.text = string.Format("Distance: {0}", ((int)distance));
                        break;
                    }
                case PING_TYPES.PLAYER:
                    {
                        color = Color.blue;
                        typeText_.text = "TYPE: SELF";
                        costText_.text = string.Format("LIFE SIGNS: STABLE");
                        break;
                    }
                default:
                    {
                        break;
                    }
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
            pingInteractTransform_.gameObject.SetActive(display);
        };

    }

    void Update()
    {
        if (display_)
        {
            pingInteractTransform_.position = Input.mousePosition;
        };
    }

	
	
}
