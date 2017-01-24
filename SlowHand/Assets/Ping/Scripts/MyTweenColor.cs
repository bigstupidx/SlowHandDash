using UnityEngine;
using System.Collections;

namespace SlowHand
{
    public class MyTweenColor : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;

        public Color from = Color.white;
        public Color to = Color.white;

        public float duration;
        public bool loop;

        float currentTime;

        // Use this for initialization
        void Start()
        {
            currentTime = 0;
        }

        // Update is called once per frame
        void Update()
        {
            currentTime += Time.deltaTime;

            if (loop)
            {
                currentTime = currentTime % duration;
            }

            Color color = Color.Lerp(from, to, currentTime / duration);

            spriteRenderer.color = color;

            if (!loop)
            {
                if (currentTime > duration)
                {
                    enabled = false;
                }
            }
        }
    }
}