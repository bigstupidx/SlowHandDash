using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace SlowHand
{
    public class GSResult : IState
    {
        static IState _instance;
        public GameObject guiResult;
        public GameController gameController;
        public GameObject guiResultDialog;

        public Text resultDialog_lbCurrentScore;
        public Text resultDialog_lbBestScore;
        public Text resultDialog_lbStar;

        public AudioClip musicClip;

        public static IState Instance
        {
            get
            {
                return _instance;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _instance = this;
            guiResult.SetActive(false);
        }

        void init()
        {
            GSGamePlay.Instance.UpgradeCharater(GamePreferences.profile.Customize);
            showResult();
            ///
        }

        void onBackKey()
        {
        }

        public override void onSuspend()
        {
            GameStatesManager.OnBackKey = null;
        }

        public override void onResume()
        {
            GameStatesManager.OnBackKey = onBackKey;
            if (musicClip != null)
            {
                AudioManager.PlayMusic(musicClip, true);
            }
        }

        public override void onEnter()
        {
            base.onEnter();
            guiResult.SetActive(true);
            GameStatesManager.Instance.InputProcessor = guiResult;
            init();
            onResume();
        }

        public override void onExit()
        {
            base.onExit();
            guiResult.SetActive(false);
        }

        void showResult()
        {
            guiResult.gameObject.SetActive(true);
            guiResultDialog.SetActive(true);
            resultDialog_lbBestScore.text = "BEST   " + GamePreferences.profile.HighScore.ToString();
            resultDialog_lbCurrentScore.text = gameController.Score.ToString();
            resultDialog_lbStar.text = GamePreferences.profile.Star.ToString();
        }

        public void onBtnPlayClick()
        {
            GameStatesManager.Instance.stateMachine.SwitchState(GSGamePlay.Instance);

        }
        public void onBtnLikeClick()
        {
            Application.OpenURL("https://www.facebook.com/fukypuzzle/");
        }

        public void onBtnHowToPlay()
        {
            GameStatesManager.Instance.stateMachine.SwitchState(GSTutorial.Instance);
        }
        public void onBtnRateClick()
        {
            Application.OpenURL(Utils.LINK_APP_STORE);
        }
        public void onBtnBoardClick()
        {
        }
        public void onBtnCustomizeClick()
        {
            GameStatesManager.Instance.stateMachine.SwitchState(GSShop.Instance);
        }
    }
}