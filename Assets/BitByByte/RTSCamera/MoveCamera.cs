using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitByByte.Camera
{
    public class MoveCamera : MonoBehaviour
    {
        public float elevateStep = 10f;
        public float speed = 10f;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime * speed;
            float height = Input.mouseScrollDelta.y * elevateStep;
            transform.Translate(move.x, height, move.y, Space.Self);
        }
    }
}