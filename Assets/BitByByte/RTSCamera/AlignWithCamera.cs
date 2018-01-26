using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitByByte.Camera
{
    public class AlignWithCamera : MonoBehaviour
    {

        public UnityEngine.Camera camera;
        public bool automaticBaseRotation = true;
        public Vector3 cameraBaseRotation;
        public Vector3 baseRotation;

        // Use this for initialization
        void Start()
        {
            if (camera == null)
                camera = UnityEngine.Camera.main;
            if (automaticBaseRotation)
            {
                cameraBaseRotation = camera.transform.eulerAngles;
                baseRotation = transform.eulerAngles;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (camera != null)
            {
                transform.eulerAngles = camera.transform.eulerAngles - cameraBaseRotation + baseRotation;
            }
        }
    }
}