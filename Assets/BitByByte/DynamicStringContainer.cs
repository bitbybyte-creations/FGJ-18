using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;
using BitByByte.Extensions;


//[RequireComponent(typeof(UnityEngine.UI.Text))]
public class DynamicStringContainer : MonoBehaviour
{
    public bool SortedList = false;
    public String Delimiter = ":";
    public float defaultLifeTime = 10f;
    public Color DefaultStringColor = Color.white;
    public Color DefaultValueColor = Color.green;
    private Color m_stringColor;
    private float m_lastUpdate = 0;
    public float UpdateFrequency = 0.5f;
    public Color StringColor
    {
        get
        {
            return m_stringColor;
        }
        set
        {
            m_stringColor = value;
        }
    }
    public static List<DynamicStringContainer> InstanceList
    {
        get { return m_instanceList; }
    }
    private static List<DynamicStringContainer> m_instanceList = new List<DynamicStringContainer>();
    private static DynamicStringContainer m_dummy;
    private Dictionary<string, ExpiringObjectWrapper> m_parameters = new Dictionary<string, ExpiringObjectWrapper>();
    private List<string> m_sortedKeys;
    private UnityEngine.UI.Text m_unityText;
    private TMPro.TextMeshProUGUI m_TMPRO_text;

    //private SortedDictionary<string, ExpiringObjectWrapper> m_parameters = new SortedDictionary<string, ExpiringObjectWrapper>();
    /// <summary>
    /// Returns the first Dynamic String Container, or creates a new one if none exists.
    /// </summary>
    public static DynamicStringContainer Dummy
    {
        get
        {
            if (m_dummy == null)
            {
                GameObject dummy = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Hud/DummyDSC"));
                dummy.transform.SetParent(GameObject.Find("UI Max").transform, false);
                m_dummy = dummy.GetComponent<DynamicStringContainer>();
                m_dummy.gameObject.name = "Dummy";
            }

            return m_dummy;
        }
    }

    public IEnumerable<string> KeysList
    {
        get
        {
            if (SortedList)
                return m_sortedKeys;
            else
                return m_parameters.Keys.ToList<string>();
        }
    }

    // Use this for initialization
    void Awake()
    {
        if (!m_instanceList.Contains(this))
        {
            //Debug.Log( "this is null" + ( this == null ) );
            m_instanceList.Add(this);
            Debug.Log("Added " + name + ". DynamicStringContainer instances: " + m_instanceList.Count);
        }
        m_stringColor = DefaultStringColor;
        m_unityText = GetComponent<Text>();
        m_TMPRO_text = GetComponent<TMPro.TextMeshProUGUI>();

    }

    void Update()
    {
        float time = Time.realtimeSinceStartup;
        if (time - m_lastUpdate >= UpdateFrequency)
        {
            m_lastUpdate = time;
            //Debug.Log("Should Refresh " + name);
            refreshText();

        }
    }

    void Start()
    {
        //StartCoroutine(refreshText());
    }

    // Update is called once per frame
    public String ColoredString(String s, Color color)
    {
        string htmlcolor = "#" + ColorUtility.ToHtmlStringRGBA(color);
        return "<color=" + htmlcolor + ">" + s + "</color>";
    }
    //private System.Collections.IEnumerator refreshText()
    private void refreshText()
    {
        float start = Time.realtimeSinceStartup;
        //Debug.Log("Refresh: " + name);
        //TODO: Use SortedDicionary and Dictionary in parallel
        string text = "";
        if (m_parameters.Count > 0)
        {
            List<string> removelist = new List<string>();
            //List<string> keys = (m_parameters.Keys.ToList();
            //if (SortedList)
            //    keys.Sort();
            foreach (String key in KeysList)
            {
                try
                {
                    ExpiringObjectWrapper value = m_parameters[key];                    
                    if (value != null && value.TimeToExpire > 0)
                    {
                        Color stringcolor = StringColor;
                        stringcolor.a = value.ColorAlpha;
                        text += (ColoredString(key, stringcolor) + (value != null ? ColoredString(Delimiter + " ", stringcolor) + value.ToString() : "") + "\n");
                    }
                    else
                        removelist.Add(key);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
                //yield return null;
            }
            if (removelist.Count > 0)
                foreach (string rm in removelist)
                {
                    m_parameters.Remove(rm);
                    m_sortedKeys.Remove(rm);
                }
        }
        if (m_unityText != null)
        {
            m_unityText.text = text;
        }
        else if (m_TMPRO_text != null)
        {
            m_TMPRO_text.SetText(text);
        }
        
        //DebugExtensions.Param(name + " refresh", new UnitValueWrapper((Time.realtimeSinceStartup - start)*1000, "ms"));
        //yield return new WaitForSeconds( 0.5f );
        //Debug.Log(name + " Done!");

    }
    public void Clear()
    {
        m_parameters.Clear();
    }

    internal static DynamicStringContainer GetContainer()
    {
        if (m_instanceList.Count > 0)
        {
            return m_instanceList[0];
        }
        else
            return Dummy;
    }
    public void SetParameter(string name, System.Object value, Color color, float expireTime, bool showTimer = false)
    {
        if (m_parameters.ContainsKey(name))
        {
            ExpiringObjectWrapper wrap = ((ExpiringObjectWrapper)m_parameters[name]);
            wrap.Object = value;
            wrap.Color = color;
            wrap.ResetTimer();
            wrap.ShowTimer = showTimer;
        }
        else
        {
            m_parameters.Add(name, new ExpiringObjectWrapper(value, color, expireTime, showTimer));
            m_sortedKeys = m_parameters.Keys.ToList();
            m_sortedKeys.Sort();
        }
    }

    public void SetParameter(string name, System.Object value, Color color, bool showTimer = false)
    {
        SetParameter(name, value, color, defaultLifeTime, showTimer);
    }
    public void SetPersistentParameter(string name, System.Object value, Color color)
    {
        SetParameter(name, value, color, Mathf.Infinity, false);
    }
    public void SetPersistentParameter(string name, System.Object value)
    {
        SetPersistentParameter(name, value, DefaultValueColor);
    }
    public void SetParameter(string name, System.Object value, bool showTimer = false)
    {
        SetParameter(name, value, DefaultValueColor);
    }

    //public void SetParameter( string name, float value, bool showTimer = false )
    //{
    //    SetParameter( name, new Decimal( value ), showTimer );
    //}
    public void RemoveParameter(string v)
    {
        if (m_parameters.ContainsKey(v))
        {
            m_parameters.Remove(v);
        }
    }

    public static DynamicStringContainer GetContainer(string name)
    {
        foreach (DynamicStringContainer dc in InstanceList)
        {
            if (dc.name == name)
                return dc;
        }
        return null;
    }
    void OnDestroy()
    {
        Debug.LogError(name + "getting destroyed!");

        m_instanceList.Remove(this);
    }
}

public class UnitValueWrapper
{
    public string Unit;
    public object Value;

    public UnitValueWrapper(object value, string unit)
    {
        Value = value;
        Unit = unit;
    }

    public override string ToString()
    {
        return Value.ToString() + " " + Unit;
    }
}

public class ObjectColorWrapper
{
    public System.Object Object
    {
        get
        {
            return m_object;
        }
        set
        {
            m_object = value;
        }
    }
    public virtual Color Color
    {
        get
        {
            return m_color;
        }
        set
        {
            m_color = value;
        }
    }


    private Color m_color;
    private System.Object m_object;

    public ObjectColorWrapper(System.Object obj, Color color)
    {
        m_color = color;
        m_object = obj;
    }

    public virtual string ObjectString()
    {
        return Object.ToString();
    }
    public override string ToString()
    {
        if (Object != null)
        {
            string color = "#" + ColorUtility.ToHtmlStringRGBA(Color);
            return "<color=" + color + ">" + ObjectString() + "</color>";
        }
        return "";
    }
}

public class ExpiringObjectWrapper : ObjectColorWrapper
{
    public bool ShowTimer = true;
    public float ColorAlpha
    {
        get
        {
            if (m_lifeSetting == Mathf.Infinity)
                return 1f;
            return Mathf.Min(1f, Mathf.Max(0f, TimeToExpire / (m_lifeSetting * 0.5f)));
        }
    }
    public override Color Color
    {
        get
        {
            Color c = base.Color;
            c.a = ColorAlpha;
            return c;
        }

        set
        {
            base.Color = value;
        }
    }
    public static float DefaultLifeTimeSeconds = 10;
    private float m_lifeSetting = DefaultLifeTimeSeconds;
    /// <summary>
    /// Time to expire in seconds
    /// </summary>
    public float TimeToExpire
    {
        get
        {
            return m_timeExpire - Time.realtimeSinceStartup;
        }

        set
        {
            m_timeExpire = Time.realtimeSinceStartup + value;
        }
    }

    public void ResetTimer()
    {
        TimeToExpire = m_lifeSetting;
    }

    private float m_timeExpire;

    public ExpiringObjectWrapper(System.Object obj, Color color) : base(obj, color)
    {
        TimeToExpire = DefaultLifeTimeSeconds;
    }
    public ExpiringObjectWrapper(System.Object obj, Color color, float expire, bool showTimer) : base(obj, color)
    {
        TimeToExpire = expire;
        m_lifeSetting = expire;
        ShowTimer = showTimer;
    }

    public override string ObjectString()
    {
        return base.ObjectString() + (ShowTimer ? String.Format("({0:F0})", TimeToExpire) : "");
    }
}
