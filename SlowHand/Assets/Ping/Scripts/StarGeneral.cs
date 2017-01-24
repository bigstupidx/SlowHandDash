using UnityEngine;
using System.Collections;

namespace SlowHand
{
    public class StarGeneral : MonoBehaviour
    {

        public ESpikeType type;
        public GameObject[] star;
        // Use this for initialization
        void OnEnable()
        {
            for (int i = 0; i < star.Length; i++)
            {
                Utils.setActive(star[i], false);
            }
            if (type == ESpikeType.CenterSmall)
            {
                gameObject.tag = "Coin";
                int rand = Random.Range(0, 3);
                if (rand == 0)
                {
                    Utils.setActive(star[0], true);
                    Utils.setActive(star[1], true);
                    star[0].tag = "Coin";
                    star[1].tag = "Coin";
                }
                else if (rand == 1)
                {
                    Utils.setActive(star[2], true);
                    Utils.setActive(star[3], true);
                    star[2].tag = "Coin";
                    star[3].tag = "Coin";
                }
                else if (rand == 2)
                {
                    Utils.setActive(star[0], true);
                    Utils.setActive(star[3], true);
                    star[0].tag = "Coin";
                    star[3].tag = "Coin";
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
