using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingZone : MonoBehaviour {

    public PING_TYPES[] types_;

    public Ping Initialize() {

        // Spawn a random type in a random position inside this rect
        Rect bound = GetComponent<RectTransform>().rect;
        Vector3 center = bound.center;

        GameObject pingObj = WorldMapController.instance_.SpawnPingType(types_[Random.Range(0, types_.Length)]).gameObject;
        pingObj.transform.SetParent(transform, false);
        Debug.Log(pingObj.name);
        Vector3 randomPos = center + new Vector3(
               (Random.value - 0.5f) * bound.x,
               (Random.value - 0.5f) * bound.y
            );

        pingObj.transform.localPosition = randomPos;

        return pingObj.GetComponent<Ping>();
    }
}
