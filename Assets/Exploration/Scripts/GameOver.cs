using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

    UnityEngine.UI.Text m_text;
	// Use this for initialization
	void Start () {
        
        m_text = GetComponent<UnityEngine.UI.Text>();
        m_text.enabled = false;
        FindObjectOfType<ExplorationInit>().OnInitEvent += Init;
	}

    public void Init()
    {
        Synchronizer.Instance.OnPlayerDiedEvent += OnGameOver;
    }

    private void OnGameOver()
    {        
        m_text.color = new Color(1f, 0f,0f, 0f);
        m_text.enabled = true;
        LeanTween.textColor(m_text.transform as RectTransform, Color.red, 5f);
        Invoke("LoadMainMenu",10f);
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("mainmenu");
        Synchronizer.Reset();
    }
}
