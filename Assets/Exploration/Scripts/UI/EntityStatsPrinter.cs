using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityStatsPrinter : MonoBehaviour
{

    SynchronizedActor m_actor;
    EnemyController m_enemyController;
    public bool enabled = false;
    private string print;
    private GameObject m_printer;

    // Use this for initialization1
    void Start()
    {
        m_actor = GetComponent<SynchronizedActor>();
        m_enemyController = GetComponent<EnemyController>();
        m_actor.Stats.OnStatsChangedEvent += RefreshString;
        GameObject canvas = GameObject.Find("_canvas");        
        m_printer = Instantiate(Resources.Load<GameObject>("Prefabs/EntityStatsText"));
        m_printer.transform.SetParent(canvas.transform);

        RefreshString();
    }

    private void RefreshString()
    {
        print = m_actor.Stats.Name;
        if (m_enemyController != null)
        {
            print += "\n" + m_enemyController.currentBehaviour.ToString();
        }
        print += "\nHP: " + m_actor.Stats.Health;
        print += "\nSPD: " + m_actor.Stats.SpeedString;
        print += "\nATK: " + m_actor.Stats.AttackString;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            enabled = !enabled;
        }

        if (enabled)
        {
            m_printer.SetActive(true);
            m_printer.transform.position = RectTransformUtility.WorldToScreenPoint(UnityEngine.Camera.main, transform.position);
            m_printer.GetComponentInChildren<Text>().text = print;
        }
        else
        {
            m_printer.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if(m_printer != null)
            m_printer.SetActive(false);
    }
}
