using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterManager : MonoBehaviour {

    public Button ExploreButton_;
    public Text writerText_;

	// Use this for initialization
	void Start () {

        ExploreButton_.onClick.AddListener(()=>WorldMapController.instance_.StartEncounter());

	}

    public void ShowEncounterButton(bool show = true) {

    }
}
