using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitByByte.Util
{
    public class AsyncAction<T>
    {
        private AsyncAction<T> m_subAction;
        public Dictionary<string, object> Arguments = new Dictionary<string, object>();
        private bool m_complete;
        private T m_result;

        public AsyncAction<T> SubAction
        {
            get
            {
                return m_subAction;
            }
            set
            {
                m_subAction = value;
            }
        }

        public delegate void OnCompleteAction(T result);
        private event OnCompleteAction OnCompleteEvent;

        public AsyncAction<T> OnComplete(OnCompleteAction action)
        {
            if (m_complete)
                action(m_result);
            else
                OnCompleteEvent += action;

            return this;
        }

        internal void complete(T result)
        {
            if (m_subAction != null)
            {
                //Debug.Log("Completing -> Sub Action");
                m_subAction.complete(result);
            }
            else if (OnCompleteEvent != null)
            {
                //Debug.Log("Completing -> Complete Event");
                OnCompleteEvent(result);
            }
            //else { Debug.LogError("Completing -> NO ACTIONS!"); }

            m_complete = true;
            m_result = result;
        }
    }
}