using UnityEngine;
using System.Collections;
using System;

namespace BitByByte.Camera
{
    public class MoveByDragMouse : MonoBehaviour
    {
        public float speed;
        public float distanceMultiplier = 10f;
        private bool m_dragging;
        private Vector3 m_startPosition;
        private Vector3 m_startCursorPosition;

        public bool Dragging
        {
            get
            {
                return m_dragging;
            }
            set
            {
                m_dragging = value;
                if (value)
                {
                    m_startPosition = transform.position;
                    m_startCursorPosition = Input.mousePosition;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(2))
            {
                Dragging = true;
            }
            if (Input.GetMouseButtonUp(2))
            {
                Dragging = false;
            }
            if(Dragging)
            {
                DragCamera();
            }
        }

        private void DragCamera()
        {
            Vector2 mouseDelta = Input.mousePosition - m_startCursorPosition;
            Vector3 moveDelta = (transform.right * mouseDelta.x + transform.forward * mouseDelta.y) * -1f;
            Vector3 pos = moveDelta * Time.deltaTime * distanceMultiplier + m_startPosition;
            transform.Translate((pos - transform.position) * Time.deltaTime * speed);
        }
    }
}