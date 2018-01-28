using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToWorldMap : MonoBehaviour {

	public void FinishScene() {

        WorldMapController.instance_.SceneFinished();
        Scene thisScene = SceneManager.GetSceneByName(WorldMapController.currentSceneName_);
        SceneManager.UnloadSceneAsync(thisScene);

    }
}
