using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour {

    public Animator pingAnimator_;
    public TypeWriter textWriter_;
    public CanvasGroup maincanvasgroup_;
    public Image blackOutCanvas_;
    [Multiline]
    public string textWritten_;
    public Button startButton_;

	// Use this for initialization
	void Start () {
        WorldMapController.instance_.energy = 100f;
        pingAnimator_.SetTrigger("Ping");
        textWriter_.Write(textWritten_, true, false);
        startButton_.onClick.AddListener(()=>StartGameAnim());
	}

    void StartGame()
    {
        SceneManager.LoadScene("worldmap-dev");
    }
	
	void StartGameAnim()
    {
        startButton_.onClick.RemoveAllListeners();
        LeanTween.alphaCanvas(maincanvasgroup_, 0, 1.5f);
        LeanTween.value(gameObject, BlackOut, 0f, 1f, 2.5f).setDelay(2f);
        LeanTween.move(Camera.main.gameObject, pingAnimator_.transform.position, 5f).setDelay(1f).setOnComplete(()=>StartGame()).setEase(LeanTweenType.easeInOutQuad);
    }
    void BlackOut(float alpha)
    {
        blackOutCanvas_.color = new Color(0,0,0, alpha);
    }

}
