using UnityEngine;
using System.Collections.Generic;
using System;

namespace BitByByte.Util
{
    /// <summary>
    /// A simple prefab recycler utility class. use from unity main thread!
    /// NOTE! You must take care of initializing the recycled object manually, providing the necessary interface yourself.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PrefabRecycler<T> where T : Component
    {
        private string m_resourcePath;
        private int m_prefabLimit = 1000;
        private List<T> m_visible = new List<T>();
        private List<T> m_recycled = new List<T>();

        public PrefabRecycler(string resourcePath, int prefabLimit = 100)
        {
            m_resourcePath = resourcePath;
            m_prefabLimit = prefabLimit;
        }

        public T SpawnPrefabAt(Vector3 position)
        {
            T t = GetRecycled();
            if (t == null)
            {
                t = SpawnOrRecycle();
            }

            t.transform.position = position;
            return t;
            
        }

        private T SpawnOrRecycle()
        {
            if (m_visible.Count >= m_prefabLimit)
            {
                return m_visible[0];
            }
            else
            {
                GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(m_resourcePath));
                T t = go.GetComponent<T>();
                m_visible.Add(t);
                return t;
            }
        }

        public void Recycle(T obj)
        {
            if (m_visible.Contains(obj))
            {
                obj.gameObject.SetActive(false);
                m_visible.Remove(obj);
                m_recycled.Add(obj);
            }
        }

        private T GetRecycled()
        {
            if (m_recycled.Count > 0)
            {
                T t = m_recycled[0];
                t.gameObject.SetActive(true);
                m_recycled.Remove(t);
                m_visible.Add(t);
                return t;
            }
            return null;
        }

        public void RecycleAll()
        {
            foreach (T t in m_visible)
            {
                Recycle(t);
            }
        }

    }
}