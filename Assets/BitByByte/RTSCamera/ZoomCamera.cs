using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitByByte.Camera
{
    public class ZoomCamera : MonoBehaviour
    {

        UnityEngine.Camera m_camera;
        public float zoomStep = 1f;

        // Use this for initialization
        void Start()
        {
            m_camera = GetComponentInChildren<UnityEngine.Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            m_camera.fieldOfView += zoom * zoomStep;
        }
    }
}