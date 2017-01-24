using UnityEngine;
using System.Collections;

namespace SlowHand
{
    public class StarController : MonoBehaviour {

        //public ParticleSystem ps;
        public GameObject star;
        bool isAlive;
        void OnEnable()
        {
            isAlive = true;
            Utils.setActive(star, true);

        }
        public void Explode() {
            if (isAlive)
            {
                isAlive = false;
                GameController.Instance.Star++;
                Utils.setActive(star, false);
                //ps.Play();
            }
        }
    }
}
