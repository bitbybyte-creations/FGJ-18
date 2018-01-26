using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitByByte.Extensions {
    public static class MonobehaviourExtensions {

        /// <summary>
        /// returns first instance of component type T found in any siblings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="monob"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetComponentInSibling<T>(this MonoBehaviour monob) where T : Component
        {
            for (int i=0; i<monob.transform.parent.childCount; i++)
            {
                Transform sibling = monob.transform.parent.GetChild(i);
                T comp = sibling.GetComponent<T>();
                if (comp != null)
                    return comp;
            }
            return null;
        }
    }

    public static class DebugExtensions
    {
        public static void DrawCross(Vector3 position, float size = 10f)
        {
            Debug.DrawLine(position + Vector3.right * -size, position + Vector3.right * size);
            Debug.DrawLine(position + Vector3.up * -size, position + Vector3.up * size);
        }

        public static void Param(string paramName, object paramValue, bool persistent = false, bool showTimer = false)
        {
            try
            {
                if (persistent)
                    DynamicStringContainer.GetContainer("DebugContainer").SetPersistentParameter(paramName, paramValue);
                else
                    DynamicStringContainer.GetContainer("DebugContainer").SetParameter(paramName, paramValue, showTimer);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }
        public static void Param( string paramName, object paramValue, Color color, bool persistent = false, bool showTimer = false)
        {
            try
            {
                if (persistent)
                    DynamicStringContainer.GetContainer("DebugContainer").SetPersistentParameter(paramName, paramValue, color);
                else
                    DynamicStringContainer.GetContainer("DebugContainer").SetParameter(paramName, paramValue, color);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static void DrawCircle(Vector3 position, float radius, int vertices, Color color)
        {
            Vector3 last = position + Vector3.right * radius;
            for (int i = 0; i <= vertices; i++)
            {
                float rad = (Mathf.PI / vertices * 2f) * i;
                Vector3 pos = position + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;
                Debug.DrawLine(last, pos, color);
                last = pos;
            }
        }
    }

    public static class Vector2ext
    {
        /// <summary>
        /// Results positive if DirectionVector is to right of Target direction, negative if to left and zero if they're aligned.
        /// </summary>
        public static float GetTurnDirection(this Vector2 origin, Vector2 target)
        {
            //Forward
            Vector2 A = origin;
            Vector2 B = target;

            float result = (-A.x * B.y + A.y * B.x) * -1;

            return result;
        }
        /// <summary>
        /// Returns the offset in angles between Origin and Target: Positive angle is to the right and negative angle is to the left
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float AngleOffset(this Vector2 origin, Vector2 target)
        {            
            float dir = origin.GetTurnDirection(target);
            float angle = Vector2.Angle(origin, target);
            return angle * Mathf.Sign(dir);
        }

        /// <summary>
        /// uses relative right to determine turn direction, the get angle in 360 degrees
        /// e.g. transform up, targetDir, transform.right
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static float angle360(this Vector2 from, Vector2 to, Vector2 right)
        {
            float angle = Vector2.Angle(from, to);
            return (Vector2.Angle(right, to) > 90f) ? 360f - angle : angle;
        }

    }

    public static class Vector3ext
    {

        /// <summary>
        /// e.g. transform up, targetDir, transform.right
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static float angle360(this Vector3 from, Vector3 to, Vector3 right)
        {
            float angle = Vector3.Angle(from, to);
            return (Vector3.Angle(right, to) > 90f) ? 360f - angle : angle;
        }
    }

}