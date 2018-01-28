using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyLossAnimator : MonoBehaviour {

    public Image EnergyLossPanelBase_;
    public Image EnergyLeftPreview_;
    public Image EnergyToLose_;
    public Image EnergyLeft_;

    private bool travelling_;
	// Use this for initialization
	void Start () {

        // Assume full energy to begin
        EnergyLeft_.fillAmount = 1f;
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (WorldMapController.instance_.player.travelling_)
        {
            if (EnergyLeftPreview_.fillAmount > WorldMapController.instance_.player.energyLeft)
            EnergyLeft_.fillAmount = WorldMapController.instance_.player.energyLeft;
        }	
	}
}
