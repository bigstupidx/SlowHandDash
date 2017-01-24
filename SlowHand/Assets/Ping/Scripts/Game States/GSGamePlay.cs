using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SlowHand
{
    public class GSGamePlay : IState
    {
        //public AdsController adsController;
        static GSGamePlay _instance;
        public GameObject guiHUD;
        public GameController gameController;
        public SpriteRenderer charLeft;
        public SpriteRenderer charRight;
        bool _isPlaying = false;
        public AudioClip musicClip;

        public bool isPlaying
        {
            get { return _isPlaying; }
        }
        public static GSGamePlay Instance
        {
            get
            {
                return _instance;
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;
            _instance = this;
            guiHUD.gameObject.SetActive(false);
        }

        void init()
        {
            GameConstants.Instance.InitNextTheme();
            UpgradeCharater(GamePreferences.profile.Customize);
            startGame();
        }

        public void UpgradeCharater(int id)
        {
            charLeft.sprite = GameConstants.Instance.getBlockBlue(id);
            charRight.sprite = GameConstants.Instance.getBlockRed(id);
        }

        void onBackKey()
        {
        }

        public override void onSuspend()
        {
            GameStatesManager.OnBackKey = null;
            gameController.OnGameOver = null;
            guiHUD.SetActive(false);
        }

        public override void onResume()
        {
            GameStatesManager.OnBackKey = onBackKey;
            gameController.OnGameOver = OnGameOver;
            guiHUD.SetActive(true);
            if (musicClip != null)
            {
                AudioManager.PlayMusic(musicClip, true);
            }
        }

        public override void onEnter()
        {
            base.onEnter();
            GameStatesManager.Instance.InputProcessor = this.gameObject;
            init();
            onResume();
        }

        public override void onExit()
        {
            base.onExit();
        }

        public IEnumerator toResultState()
        {
            yield return new WaitForSeconds(2f);
            GameStatesManager.Instance.StateMachine.SwitchState(GSResult.Instance);
        }

        void OnGameOver()
        {
            if (gameController.Score > GamePreferences.profile.HighScore)
            {
                GamePreferences.profile.updateHighScore(gameController.Score);
                GamePreferences.submitScore(GamePreferences.profile.HighScore);
            }
            GamePreferences.profile.updateStar(gameController.Star);
            GamePreferences.saveProfile();

            _isPlaying = false;
            StartCoroutine("toResultState");
        }

        public static int countPlaygame = 0;
        void startGame()
        {
            _isPlaying = true;
            countPlaygame++;
            gameController.startGame();
        }
    }
}