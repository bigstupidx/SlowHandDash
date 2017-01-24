using UnityEngine;
using System.Collections;
namespace SlowHand
{
    public class ButtonPause : MonoBehaviour
    {
        public GameObject btnPause;
        public GameObject btnUnpause;
        void OnEnable()
        {
            updateButton();
        }
        void updateButton()
        {
            Utils.setActive(btnPause, GameController.Instance.updateGame);
            Utils.setActive(btnUnpause, !GameController.Instance.updateGame);
        }
        public void onBtnPauseClick()
        {
            if (GameController.Instance.updateGame)
            {
                GameController.Instance.pauseGame();
                updateButton();
            }
            else
            {
                GameController.Instance.resumeGame();
                updateButton();
            }
        }
    }
}