using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BitByByte.UI
{
    public class SliderElement : MonoBehaviour
    {
        private Slider m_slider;
        private Text m_text;        
        private bool m_init = false;
        public static Font FONT;
        private string m_defaultText = "Slider";
        private float m_defaultValue = 0.5f;
        private GridLayoutGroup m_layout;
        private bool m_showValueOnText = true;
        private string m_textString;

        public bool ShowValueOnText
        {
            get
            {
                return m_showValueOnText;
            }
            set
            {
                m_showValueOnText = value;
                Text = m_textString;
            }
        }

        public string Text
        {
            get
            {
                if (m_init)
                {
                    return m_textString;

                }
                return m_defaultText;
            }
            set
            {
                m_textString = value;
                if (m_init)
                {
                    m_textString = value;
                    if (ShowValueOnText)
                    {
                        //Debug.Log("Set Text: Show Value: " + m_textString + " + value: " + Value);
                        m_text.text = m_textString + " (" + Value.ToString("F2") + ")";
                    }
                    else
                    {
                        //Debug.Log("Set Text: " + m_textString);
                        m_text.text = m_textString;
                    }
                }
            }
        }

        public float Value
        {
            get
            {
                if (m_init)
                    return m_slider.value;
                return m_defaultValue;
            }
            set
            {
                if (m_init)
                    m_slider.value = value;
                else
                    m_defaultValue = value;
            }
        }

        private void Awake()
        {
            int h = 30;
            gameObject.AddComponent<LayoutElement>().preferredHeight = h;
            m_layout = gameObject.AddComponent<GridLayoutGroup>();
            m_layout.cellSize = new Vector2((transform.parent as RectTransform).sizeDelta.x/2 - 5, h);

            GameObject text = new GameObject("text");
            m_text = text.AddComponent<Text>();
            m_text.font = FONT;
            text.transform.SetParent(m_layout.transform, false);

            GameObject slider = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Slider"));
            m_slider = slider.GetComponent<Slider>();
            slider.transform.SetParent(m_layout.transform, false);
            m_slider.onValueChanged.AddListener(delegate (float v)
            {
                Text = m_textString;
            });

            m_textString = m_defaultText;

            
        }

        private void Start()
        {
            Debug.Log("Slider Element Start");
            m_init = true;
            Text = m_textString;
            Value = m_defaultValue;
            //last thing to do here!
            
        }


        public void AddOnValueChangedListener(UnityEngine.Events.UnityAction<float> action)
        {
            m_slider.onValueChanged.AddListener(action);            
        }

        public static SliderElement AddTo(LayoutGroup group, string name)
        {
            //GameObject sliderObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Slider"));
            GameObject sliderObject = new GameObject();
            sliderObject.name = "Slider:" + name;
            sliderObject.transform.SetParent(group.gameObject.transform, false);
            sliderObject.transform.SetAsFirstSibling();
            SliderElement e = sliderObject.AddComponent<SliderElement>();
            e.Text = name;
            return e;
        }
 
    }
}