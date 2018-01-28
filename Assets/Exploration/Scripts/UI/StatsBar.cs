using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsBar : MonoBehaviour
{

    private GameObject blockPrefab;
    private static StatsBar m_instance;
    private Dictionary<string, StatBlock> m_blocks = new Dictionary<string, StatBlock>();

    private void Awake()
    {
        m_instance = this;
    }
    // Use this for initialization
    void Start()
    {

    }

    public StatBlock GetStatBlock(string key)
    {
        if (m_blocks.ContainsKey(key))
            return m_blocks[key];
        StatBlock block = CreateBlock(key);
        return block;
    }

    private StatBlock CreateBlock(string key)
    {
        GameObject block = Instantiate<GameObject>(blockPrefab);
        block.transform.SetParent(transform);
        StatBlock sb = block.GetComponent<StatBlock>();
        m_blocks.Add(key, sb);
        return sb;
    }

    public void SetStat(string key, string value)
    {
        StatBlock block = GetStatBlock(key);
        block.Key = key;
        block.Value = value;
    }

    public static StatsBar Instance
    {
        get
        {
            if (m_instance == null)
            {
                Debug.LogError("Instance Null!");
            }
            return m_instance;
        }
    }
}
