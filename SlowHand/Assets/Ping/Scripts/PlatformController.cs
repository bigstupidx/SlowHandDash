using UnityEngine;
using System.Collections;

namespace SlowHand
{
    public class PlatformController : MonoBehaviour
    {
        bool setScore = false;
        public int score = 1;
        void OnEnable()
        {
            setScore = false;
        }

        public void Deactivate()
        {
            ObjectPoolManager.Unspawn(this.gameObject);
        }
        void Update()
        {
            if (setScore == false && transform.position.y < GameController.Instance.ghost.transform.position.y)
            {
                setScore = true;
                if (score > 0)
                    GameController.Instance.addScore();
                else
                {
                    GameController.Instance.OnCheckPoint();
                }
            }
            else
            {
                if (transform.position.y < (GameController.Instance.botRight.position.y - 2))
                {
                    Deactivate();
                }
            }
        }
    }

}
