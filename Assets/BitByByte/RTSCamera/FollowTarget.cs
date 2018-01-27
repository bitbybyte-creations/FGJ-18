using BitByByte.Camera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitByByte.Camera
{
    public class FollowTarget : MonoBehaviour
    {

        public GameObject target;
        public Vector3 offset = new Vector3(0f, 0f, -10f);
        private MoveByDragMouse m_dragMouse;

        // Use this for initialization
        void Start()
        {
            m_dragMouse = GetComponent<MoveByDragMouse>();

        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (target != null)
            {
                if (m_dragMouse != null && !m_dragMouse.Dragging)
                {
                    transform.Translate(((target.transform.position + offset) - transform.position) * Time.deltaTime);
                }
            }
        }




    }
}