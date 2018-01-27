using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitByByte.Camera
{
    public class RegisterFollower : MonoBehaviour
    {

        private void OnEnable()
        {
            UnityEngine.Camera.main.GetComponentInParent<FollowTarget>().target = gameObject;
        }
    }
}