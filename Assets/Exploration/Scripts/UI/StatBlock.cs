using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBlock : MonoBehaviour {

    private Text m_text;
    public Color keyColor;
    public Color valueColor;
    public string Delimiter = ": ";

    public string Key
    {
        get
        {
            return m_key;
        }
        set
        {
            m_key = value;
            Refresh();
        }
    }
    public string Value
    {
        get
        {
            return m_value;
        }
        set
        {
            m_value = value;
            Refresh();
        }
    }
    private string m_key;
    private string m_value;
	// Use this for initialization
	void Awake () {
        m_text = GetComponent<Text>();
	}
	
    public void Refresh()
    {
        string text = "<color=#" + ColorUtility.ToHtmlStringRGB(keyColor) + ">" + m_key + Delimiter + "</color><color=#" + ColorUtility.ToHtmlStringRGB(valueColor) + ">" + m_value + "</color>";
        m_text.text = text;
    }
}
