﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToWorldMap : MonoBehaviour {

	public void FinishScene() {

        WorldMapController.instance_.SceneFinished();
        Scene thisScene = SceneManager.GetSceneByName(WorldMapController.currentSceneName_);        

        SynchronizedActor sa = Synchronizer.Instance.Player;
        if (sa != null)
        {
            WorldMapController.instance_.energy = (float)sa.Entity.Stats.Energy;
        }
        Synchronizer.Reset();
        SceneManager.UnloadSceneAsync(thisScene);
    }

    public void AddEnergy(float amount) {

        WorldMapController.instance_.AddEnergy = amount;

    }

}
