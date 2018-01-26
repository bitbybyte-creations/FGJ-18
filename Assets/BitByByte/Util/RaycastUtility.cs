using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitByByte.TacticalGame
{
    public static class RaycastUtility
    {

        public static bool debug = false;
        /// <summary>
        /// Fire a number of rays at a random position inside a cone from point a to direction b
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <param name="angle"></param>    
        /// <param name="count"></param>
        /// <returns></returns>
        public static RaycastHit[] FireInsideCone(Vector3 position, Vector3 target, float angle, int count = 1, int layerMask  = -1)
        {
            List<RaycastHit> hits = new List<RaycastHit>();            
            for (int i = 0; i < count; i++)
            {                
                RaycastHit hit = new RaycastHit();
                Color hitColor = Color.yellow;
                Ray ray = GetRayInsideCone(position, target, angle);
                if (Physics.Raycast(ray, out hit, 100f, layerMask))
                {
                    hits.Add(hit);
                    hitColor = Color.green;
                }
                if (debug)
                {
                    float d = (target - position).magnitude;
                    Debug.DrawRay(position, ray.direction*d, hitColor, 1f);
                }
            }
            return hits.ToArray();
        }

        public static RaycastHit[] FireBetweenCones(Vector3 position, Vector3 a, Vector3 b, float angle, int count, int layerMask = -1)
        {
            List<RaycastHit> hits = new List<RaycastHit>();
            for (int i = 0; i < count; i++)
            {
                RaycastHit hit = new RaycastHit();
                Color hitColor = Color.yellow;
                Ray ray = GetRayBetweenCones(position, a, b, angle);
                if (Physics.Raycast(ray, out hit, 100f, layerMask))
                {
                    hits.Add(hit);
                    hitColor = Color.green;
                }
                if (debug)
                {
                    float d = (a - position).magnitude;
                    Debug.DrawRay(position, ray.direction*d, hitColor, 1f);
                }
            }
            return hits.ToArray();
        }

        public static Ray GetRayInsideCone(Vector3 position, Vector3 target, float angle)
        {
            Vector3 dir = target - position;
            Quaternion q = Quaternion.LookRotation(dir, Vector3.up);
            Vector3 rdir = RandomInsideCone(angle);// * dir.magnitude;
            //if (debug)
            //    Debug.DrawRay(position, rdir, Color.magenta);
            //right this far! now just to rotate the rdir to same space as dir...            

            Vector3 randomDir = q * rdir * dir.magnitude * 2f;
            Ray ray = new Ray(position, randomDir);
            return ray;
        }

        /// <summary>
        /// get a ray between two cones, extrapolating positions between first and second by numPositions. Defaults to automatic numPositions count
        /// </summary>
        /// <param name="position"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="angle"></param>
        /// <param name="numPositions"></param>
        /// <returns></returns>
        public static Ray GetRayBetweenCones(Vector3 position, Vector3 a, Vector3 b, float angle, int numPositions = -1)
        {
            float dist = (a - b).magnitude;
            int count = numPositions;
            if (numPositions == -1)            
                count = Mathf.Max(2, Mathf.CeilToInt(dist*3f));

            int idx = UnityEngine.Random.Range(0, count);
            Vector3 randTarget = (b - a).normalized * idx * (dist / count) + a;
            //if (debug)
            //{
            //    //Debug.Log("Ray Between Cones: count " + count + " randIndex: " + idx + " randTarget: " + randTarget);
            //}
            return GetRayInsideCone(position, randTarget, angle);
        }

        public static Vector3 RandomInsideCone(float radius)
        {
            //(sqrt(1 - z^2) * cosϕ, sqrt(1 - z^2) * sinϕ, z)
            float radradius = radius * Mathf.PI / 360;
            float z = UnityEngine.Random.Range(Mathf.Cos(radradius), 1);
            float t = UnityEngine.Random.Range(0, Mathf.PI * 2);
            return new Vector3(Mathf.Sqrt(1 - z * z) * Mathf.Cos(t), Mathf.Sqrt(1 - z * z) * Mathf.Sin(t), z);
        }

        //public static int FireCone(Vector3 position, Vector3 target, float angle, int count = 1)
        //{
        //    Debug.LogError("Method not implemeented");
        //    return 0;

        //    //Vector3 dir = (direction + offset.x * muzzle.right + offset.y * muzzle.up).normalized;
        //}

    }
}