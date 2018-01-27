using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using BitByByte.Util;

namespace BitByByte.UI
{
    public class FloatingText : MonoBehaviour
    {
        private float m_birth;
        private Transform m_tracked;
        private TimedExistence m_timer;
        private Text m_text;

        public float Life
        {
            get
            {
                //Timer might not be initialized, so we make sure there is enough life left by default
                if (m_timer != null)
                {
                    if (m_timer.Expired)
                        return 0f;
                    return m_timer.LifeTime;
                }
                return 1000f;

            }
        }

        public float Age
        {
            get { return Time.realtimeSinceStartup - m_birth; }
        }

        public Transform TrackTransform { get { return m_tracked; } internal set { m_tracked = value; } }

        void Awake()
        {
            m_birth = Time.realtimeSinceStartup;



        }
        private void Start()
        {
            m_timer = gameObject.GetComponent<TimedExistence>();
            m_timer.InstallLifeWatcher(getLifeTimeWatcher());
        }

        protected virtual ILifetimeWatcher getLifeTimeWatcher()
        {
            return new TextFadeLifeWatcher();
        }

        void LateUpdate()
        {
            if (m_tracked != null)
            {
                transform.position = RectTransformUtility.WorldToScreenPoint(UnityEngine.Camera.main, m_tracked.position);
            }
        }

        public FloatingText SetLifeTime(float life, bool replace = true)
        {
            if (m_timer != null)
                m_timer.LifeTime = (replace ? life : m_timer.LifeTime + life);
            return this;
        }

        public FloatingText SetText(string text)
        {
            if (m_text == null)
                m_text = gameObject.GetComponentInChildren<Text>();
            m_text.text = text;
            return this;
        }

        private class TextFadeLifeWatcher : ILifetimeWatcher
        {
            public void Expire(GameObject gObject)
            {
                FloatingText t = gObject.GetComponent<FloatingText>();
                Text text = t.GetComponentInChildren<Text>();
                LeanTween.color(text.gameObject, new Color(0f, 0f, 0f, 0f), 1f).setOnComplete(() => Destroy(gObject));
                //text.DOColor(new Color(0f, 0f, 0f, 0f), 1f).OnComplete();
            }
        }
    }

    public class FloatingTextBuilder<T> where T : FloatingText
    {
        private static Font DEFAULT_FONT = Font.CreateDynamicFontFromOSFont("Arial", 12);
        private static float DEFAULT_LIFE_TIME = 5.0f;
        private static int DEFAULT_FONT_SIZE = 12;
        private Vector3 m_acceleration;
        private Color m_color;
        private Font m_font;
        private float m_lifetime;
        private string m_name;
        private Transform m_parent;
        private Vector3 m_position;
        private string m_text;
        private Vector3 m_velocity;
        private Transform m_transform;
        private int m_fontSize;


        public FloatingTextBuilder(Transform transform, Transform parentCanvas)
        {
            Transform = transform;
            Parent = parentCanvas;
            Name = "Floating Text";
            Text = "<NULL>";
            Color = Color.white;
            Font = DEFAULT_FONT;
            FontSize = DEFAULT_FONT_SIZE;
            Velocity = Vector3.up * 10f;
            Acceleration = Vector3.zero;
            LifeTime = DEFAULT_LIFE_TIME;
        }

        public FloatingTextBuilder(Vector3 position, Transform parentCanvas)
        {
            Parent = parentCanvas;
            Position = position;
            Name = "Floating Text";
            Text = "<NULL>";
            Color = Color.white;
            Font = DEFAULT_FONT;
            Velocity = Vector3.up * 10f;
            Acceleration = Vector3.zero;
            LifeTime = DEFAULT_LIFE_TIME;
        }

        public T Build()
        {
            T floatingText = new GameObject().AddComponent<T>();
            floatingText.transform.SetParent(Parent, false);
            floatingText.name = Name;
            floatingText.gameObject.AddComponent<RectTransform>();
            floatingText.TrackTransform = Transform;
            SetPosition(floatingText.transform as RectTransform, Position);
            (floatingText.transform as RectTransform).sizeDelta = new Vector2(256, 256);
            floatingText.gameObject.AddComponent<TimedExistence>().LifeTime = LifeTime;

            Text uiText = new GameObject().AddComponent<UnityEngine.UI.Text>();
            uiText.rectTransform.sizeDelta = new Vector2(300f, 200f);
            uiText.gameObject.transform.SetParent(floatingText.transform, false);
            uiText.alignment = TextAnchor.MiddleCenter;
            uiText.text = Text;
            uiText.font = Font;
            uiText.fontSize = FontSize;
            uiText.color = Color;
            uiText.gameObject.AddComponent<Outline>();
            uiText.gameObject.AddComponent<SmoothMover>().UiMoveDirection(Velocity, Acceleration, LifeTime * 2f);

            return floatingText;

        }

        static void SetPosition(RectTransform rt, Vector2 position)
        {
            Vector2 targetPosition = RectTransformUtility.WorldToScreenPoint(UnityEngine.Camera.main, position);
            rt.position = targetPosition;
        }

        public Vector3 Acceleration { get { return m_acceleration; } set { m_acceleration = value; } }
        public float LifeTime { get { return m_lifetime; } set { m_lifetime = value; } }
        public string Name { get { return m_name; } set { m_name = value; } }

        public Transform Parent { get { return m_parent; } private set { m_parent = value; } }
        public Vector3 Position { get { return Transform == null ? m_position : Transform.position; } private set { m_position = value; } }
        public Transform Transform { get { return m_transform; } private set { m_transform = value; } }

        public string Text { get { return m_text; } set { m_text = value; } }

        public Color Color { get { return m_color; } set { m_color = value; } }
        public Font Font { get { return m_font; } set { m_font = value; } }
        public Vector3 Velocity { get { return m_velocity; } set { m_velocity = value; } }

        public int FontSize { get { return m_fontSize; } set { m_fontSize = value; } }
    }
}