using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingZone : MonoBehaviour {

    public WorldMapEncounter[] types_;

    public Ping Initialize() {

        // Spawn a random type in a random position inside this rect
        Rect bound = GetComponent<RectTransform>().rect;
        Vector3 center = bound.center;
        // Pick a random encounter and grab its type
        int randomencounterindex = Random.Range(0, types_.Length);
        PING_TYPES type = types_[randomencounterindex].type_;

        GameObject pingObj = WorldMapController.instance_.SpawnPingType(type).gameObject;
        Ping thePing = pingObj.GetComponent<Ping>();
        thePing.encounter_ = types_[randomencounterindex];
        pingObj.transform.SetParent(transform, false);
        Debug.Log(pingObj.name);
        Vector3 randomPos = center + new Vector3(
               (Random.value - 0.5f) * bound.x,
               (Random.value - 0.5f) * bound.y
            );

        pingObj.transform.localPosition = randomPos;

        return thePing;
    }
}
