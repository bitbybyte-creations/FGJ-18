using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitByByte.Camera
{
    public class RotateCamera : MonoBehaviour
    {
        public float rotateStep = 10f;
        public bool hold = false;
        public Vector3 rotationGoal;
        public float speed = 10f;

        // Use this for initialization
        void Start()
        {
            rotationGoal = transform.rotation.eulerAngles;
        }



        // Update is called once per frame
        void Update()
        {
            
            if (Input.GetKey(KeyCode.Q) && hold || Input.GetKeyDown(KeyCode.Q))
            {
                Rotate(rotateStep);
            }
            if (Input.GetKey(KeyCode.E) && hold || Input.GetKeyDown(KeyCode.E))
            {
                Rotate(-rotateStep);
            }

            handleRotation();
        }

        private void handleRotation()
        {
            //0-359
            Quaternion target = Quaternion.Euler(rotationGoal);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, Time.deltaTime*speed);
            
        }

        public void Rotate(float rotateStep)
        {
            rotationGoal += new Vector3(0f,rotateStep,0f);
            //if (rotationGoal.y > 360)
            //    rotationGoal.y = 360f - rotationGoal.y;
            //else if (rotationGoal.y < 0f)
            //    rotationGoal.y = rotationGoal.y + 360f;
        }


    }
}