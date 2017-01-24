using UnityEngine;
using System.Collections;

namespace SlowHand
{
    public class ThemCheckPoint : MonoBehaviour
    {

        public SpriteRenderer[] render;
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
            Color32[] _color = GameConstants.Instance.GetThemeCheckPoint();
            render[0].color = _color[0];
            render[1].color = _color[1];
        }
    }
}
