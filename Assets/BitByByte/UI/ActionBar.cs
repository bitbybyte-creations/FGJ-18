using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BitByByte.UI
{
    public class ActionBar : MonoBehaviour
    {
        public GameObject elementPrefab;
        private List<ActionBarElement> m_elements = new List<ActionBarElement>();

        public void Configure(ActiveAbility[] actions)
        {
            int dif = m_elements.Count - actions.Length;
            if (dif > 0)
                removeElements(dif);
            else if (dif < 0)
                addElements(Mathf.Abs(dif));

            Debug.Log("ActionBar Size: " + m_elements.Count + " Action Count: " + actions.Length);

            for (int i=0; i<m_elements.Count; i++)
            {
                m_elements[i].Assign(actions[i]);
                m_elements[i].Index = (i + 1);
            }
        }

        internal void RegisterElement(ActionBarElement actionBarElement)
        {
            m_elements.Add(actionBarElement);
        }

        public ActiveAbility GetAction(int index)
        {
            Debug.Log("Retrieve Action for index " + index);
            return m_elements[index].Action;
        }

        private void addElements(int v)
        {
            int index = m_elements.Count;
            for (int i=0; i<v;i++)
            {
                GameObject o = Instantiate<GameObject>(elementPrefab);
                o.name = "Action" + index + i;
                o.transform.SetParent(transform, false);
                RegisterElement(o.GetComponent<ActionBarElement>());
            }
        }

        /// <summary>
        /// Removes one element from the end (should keep indexing ok)
        /// </summary>
        /// <param name="count"></param>
        private void removeElements(int count)
        {
            for (int i=0; i<count;i++)
            {
                int index = m_elements.Count - 1;
                GameObject.Destroy(m_elements[index]);
                m_elements.RemoveAt(index);
            }
        }

        public class ActiveAbility
        {
            public delegate string AbilityDescription();
            private string m_name;            
            public Action Activate;
            public AbilityDescription Description;

            public string Name
            {
                get { return m_name; }
                private set { m_name = value; }
            }

            public ActiveAbility(string name, Action onActivate, AbilityDescription onDescription)
            {
                m_name = name;
                Activate = onActivate;
                Description = onDescription;
            }

            internal static Sprite GetIcon(string name)
            {
                string resource;
                switch (name)
                {
                    case "Assault Rifle": resource = "47";break;
                    case "Pistol": resource = "49";break;
                    case "Grenade": resource = "13";break;
                    case "Super Jump": resource = "arrow3";break;
                    default: resource = "01";break;
                }
                Debug.Log("Load Icon: " + resource + " for action " + name);
                Sprite sprite = Resources.Load<Sprite>("Icons/"+resource);
                return sprite;
            }
        }
    }
}
