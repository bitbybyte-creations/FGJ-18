using UnityEngine;
using System.Collections;
using System;

namespace BitByByte.Util
{
    public class SimpleLifetimeWatcher : ILifetimeWatcher
    {
        public void Expire(GameObject gObject)
        {
            gObject.SetActive(false);
            GameObject.Destroy(gObject, 30f);
            //Debug.Log( "Lifetime Expired: " + gObject.name + ":" + gObject.GetHashCode() );
        }
    }

    public interface ILifetimeWatcher
    {
        void Expire(GameObject gObject);
    }

    public class LifeExistance : MonoBehaviour
    {
        public GameObject[] detachOnExpire;
        private ILifetimeWatcher m_watcher = new SimpleLifetimeWatcher();
        private bool m_expired = false;
        public event Action OnDetachEvent;

        public void InstallLifeWatcher(ILifetimeWatcher watcher)
        {
            m_watcher = watcher;
        }

        public void Expire()
        {
            handleDetach();
            if (!m_expired)
            {
                m_expired = true;

                if (m_watcher != null)
                    m_watcher.Expire(gameObject);
            }

        }

        public bool Expired { get { return m_expired; } }

        public virtual void Reset()
        {
            m_expired = false;
        }

        private void handleDetach()
        {
            if (detachOnExpire != null)
            {
                foreach (GameObject go in detachOnExpire)
                {
                    go.transform.SetParent(transform.parent, true);
                }
                if (OnDetachEvent != null)
                    OnDetachEvent();
            }

        }

    }

    public class TimedExistence : LifeExistance
    {

        [SerializeField]
        private float m_lifeTime = 10f;
        public float LifeTime
        {
            get
            {
                return m_lifeTime - Elapsed;
            }
            set
            {
                m_lifeTime = value;
                Reset();
            }
        }
        private float m_timeElapsed;

        public float Elapsed
        {
            get { return m_timeElapsed; }
        }

        public override void Reset()
        {
            base.Reset();
            m_timeElapsed = 0f;
        }

        // Use this for initialization
        void Start()
        {
            m_timeElapsed = 0f;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            m_timeElapsed += Time.deltaTime;

            if (Elapsed > LifeTime)
            {
                Expire();
            }
        }

    }
}