using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Data", menuName = "New World Map Encounter", order = 1)]

public class WorldMapEncounter : ScriptableObject
{
    public string id_ = "New Encounter";
    public PING_TYPES type_ = PING_TYPES.ENERGY;
    [Multiline]
    public string flavorText_;

    public string sceneToLoad_;

}
