using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace BitByByte.UI
{
    class StatText : MonoBehaviour
    {
        private static Dictionary<string, StatText> m_statTexts = new Dictionary<string, StatText>();
        private TMPro.TextMeshProUGUI m_name;
        private TMPro.TextMeshProUGUI m_value;
        public Color nameColor = Color.white;
        public Color valueColorLow = Color.red;
        public Color valueColorMed = Color.yellow;
        public Color valueColorHigh = Color.blue;            //default
        public float valueThresholdLow = 33f;
        public float valueThresholdMed = 67f;
        private string m_nameString;
        private string m_valueString;
        private float m_valueFloat;
        

        public static StatText Obtain(string name)
        {
            string key = name.ToLower();
            if (m_statTexts.ContainsKey(key))
                return m_statTexts[key];
            else
            {
                Debug.LogError("StatText Not Found!");
                return null;
            }
                
        }

        private void Awake()
        {
            m_statTexts.Add(name.ToLower(), this);
            foreach (TextMeshProUGUI textmesh in GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (textmesh.name == "name")
                {
                    m_name = textmesh;
                    StatName = gameObject.name;
                }
                if (textmesh.name == "value")
                {
                    m_value = textmesh;
                    textmesh.text = "";
                }
                
            }
        }

        public string StatName
        {
            get
            {
                return m_nameString;
            }
            set
            {
                m_nameString = value;
                m_name.text = ColorFormatText(nameColor, m_nameString);
            }
        }
        public string StatValue
        {
            get
            {
                return m_valueString;
            }
            set
            {
                m_valueString = value;
                m_value.text = ColorFormatText(valueColorHigh, m_valueString);
            }
        }

        public float ValueFloat
        {
            get
            {
                return m_valueFloat;
            }
            set
            {
                m_valueString = m_valueFloat.ToString("F0");
                m_valueFloat = value;
                m_value.text = ColorFormatText(GetFloatColor(value), m_valueString);
            }
        }

        private Color GetFloatColor(float value)
        {
            if (value <= valueThresholdLow)
                return valueColorHigh;
            else if (value <= valueThresholdMed)
                return valueColorMed;
            return valueColorHigh;
        }

        private string ColorFormatText(Color col, string str)
        {
            string format = "<color=#{0}>{1}</color>";
            return string.Format(format, ColorUtility.ToHtmlStringRGB(col), str);
        }
    }
}
