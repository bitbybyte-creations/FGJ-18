using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BitByByte.UI
{
    public class ActionBarElement : MonoBehaviour
    {
        private Button m_button;
        private ActionBar.ActiveAbility m_action;
        private Image m_icon;
        private TMPro.TextMeshProUGUI m_numberText;

        public int Index
        {
            get
            {
                return int.Parse(m_numberText.text);
            }
            set
            {
                m_numberText.text = value.ToString();
            }

        }

        private void Awake()
        {
            m_button = GetComponent<Button>();
            m_button.onClick.AddListener(OnAbilityActivate);
            m_icon = transform.GetChild(0).GetComponent<Image>();
            m_numberText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        }

        private void OnAbilityActivate()
        {
            if (m_action != null)
            {
                Debug.Log("Activating Ability: " + m_action.Name);
                m_action.Activate();
            }
            else
            {
                Debug.LogError("No Action Assigned!");
            }
        }

        public void Assign(ActionBar.ActiveAbility activeAbility)
        {
            m_action = activeAbility;
            m_icon.sprite = ActionBar.ActiveAbility.GetIcon(activeAbility.Name);
        }

        public ActionBar.ActiveAbility Action
        {
            get
            {
                return m_action;
            }
        }
    }
}
