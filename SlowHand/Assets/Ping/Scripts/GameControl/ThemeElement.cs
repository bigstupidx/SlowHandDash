using UnityEngine;
using System.Collections;

namespace SlowHand
{
    public enum ThemeElementType
    {
        Spike,
        Wall,
        BG,
        PS,
    }
    public enum ThemeType
    {
        Sprite,
        PS,
    }
    [ExecuteInEditMode]
    public class ThemeElement : MonoBehaviour
    {

        public ThemeElementType elementType;
        public ThemeType themeType;

        void Start()
        {
            UpdateTheme();
        }

        void OnEnable()
        {
            UpdateTheme();
        }

        public void UpdateTheme()
        {
            if (GameConstants.Instance == null)
                return;
            Color32 color = GameConstants.Instance.GetThemeColor(elementType);
            switch (themeType)
            {
                case ThemeType.Sprite:
                    SpriteRenderer sr = GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.color = color;
                    }
                    break;
                case ThemeType.PS:
                    ParticleSystem ps = GetComponent<ParticleSystem>();
                    if (ps != null)
                    {
                        ps.startColor = color;
                    }
                    break;
            }
        }
    }
}