using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace SlowHand
{
    public class GSTutorial : IState
    {
        public float timeDelay = 1.0f;
        float timeCount = 0.0f;
        static IState _instance;
        public GameObject guiHUD;
        public GameController gameController;
        public GhostController ghostController;
        public Text lbTutorialText;
        public GameObject guiPlayBtn;
        public GameObject handLeft;
        public GameObject handRight;
        public AudioClip musicClip;

        public static IState Instance
        {
            get
            {
                return _instance;
            }
        }

        Dictionary<ETutorialAction, string> actions;

        ETutorialAction[] tutorialActions = new ETutorialAction[]
        {
            ETutorialAction.TouchLeft,
            ETutorialAction.TouchRight,
            ETutorialAction.TouchLeftRight,
            ETutorialAction.ReleaseTouch,
        };
        int currentTutorialActionIndex = -1;
        bool isWaitingForNextTutorial = false;


        protected override void Awake()
        {
            base.Awake();
            _instance = this;
            guiHUD.SetActive(false);
        }

        void init()
        {
            actions = new Dictionary<ETutorialAction, string>();
            actions.Add(ETutorialAction.TouchLeft, "HOLD THE LEFT");
            actions.Add(ETutorialAction.TouchRight, "HOLD THE RIGHT");
            actions.Add(ETutorialAction.TouchLeftRight, "HOLD THE BOTH\nOF SIDES");
            actions.Add(ETutorialAction.ReleaseTouch, "RELEASE\nTO RE-CENTER");

            lbTutorialText.gameObject.SetActive(false);
            guiPlayBtn.SetActive(false);

            gameController.startTutorial();

            currentTutorialActionIndex = -1;

            Invoke("startTutorial", 1);
        }
        void startTutorial()
        {
            gameController.SpawnSpikeTutorial();
            gameController.updateGame = true;
            StartCoroutine("showNextTutorial");
        }
        // Code nhình nhiều và rối, Nhưng chạy nhẹ và chính xác
        void Update()
        {
            if (0 <= currentTutorialActionIndex && currentTutorialActionIndex < tutorialActions.Length)
            {
                switch (tutorialActions[currentTutorialActionIndex])
                {
                    case ETutorialAction.TouchLeft:
                        checkTimeDelay();
                        if (ghostController.moveLeft && !ghostController.moveRight)
                        {
                            if (LeftBlockCtr.Instance.isEnterWall && !isWaitingForNextTutorial)
                            {
                                gameController.updateGame = true;
                                if (timeCount == 0)
                                {
                                    ghostController.leftBlock.fixLeftPivot();
                                    ghostController.rightBlock.fixLeftPivot();
                                }
                                if (timeCount < timeDelay * _tile)
                                {
                                    timeCount += Time.deltaTime;
                                }
                            }
                        }

                        break;

                    case ETutorialAction.TouchRight:
                        checkTimeDelay();
                        if (!ghostController.moveLeft && ghostController.moveRight)
                        {
                            if (RightBlockCtr.Instance.isEnterWall && !isWaitingForNextTutorial)
                            {
                                gameController.updateGame = true;
                                if (timeCount == 0)
                                {
                                    ghostController.leftBlock.fixRightPivot();
                                    ghostController.rightBlock.fixRightPivot();
                                }
                                if (timeCount < timeDelay * _tile)
                                {
                                    timeCount += Time.deltaTime;
                                }
                            }
                        }

                        break;

                    case ETutorialAction.TouchLeftRight:
                        checkTimeDelay();
                        if (ghostController.moveLeft && ghostController.moveRight)
                        {
                            if (LeftBlockCtr.Instance.isEnterWall
                            && RightBlockCtr.Instance.isEnterWall
                            && !isWaitingForNextTutorial)
                            {
                                gameController.updateGame = true;
                                if (timeCount == 0)
                                {
                                    ghostController.leftBlock.fixLeftPivot();
                                    ghostController.rightBlock.fixRightPivot();
                                }
                                if (timeCount < timeDelay * _tile)
                                {
                                    timeCount += Time.deltaTime;
                                }
                            }
                        }

                        break;

                    case ETutorialAction.ReleaseTouch:
                        gameController.updateGame = false;
                        if (!ghostController.moveLeft && !ghostController.moveRight)
                        {
                            if (!isWaitingForNextTutorial)
                            {
                                gameController.updateGame = true;
                                if (timeCount == 0)
                                {
                                    ghostController.leftBlock.fixCenterPivot();
                                    ghostController.rightBlock.fixCenterPivot();
                                }
                                timeCount += Time.deltaTime;
                                if (timeCount > timeDelay)
                                {
                                    timeCount = 0;
                                    StartCoroutine("showNextTutorial");
                                }
                            }
                        }
                        break;
                }
            }
        }
        float _tile = 0.7f;
        void checkTimeDelay()
        {
            if (timeCount < timeDelay * _tile)
            {
                gameController.updateGame = false;
            }
            else if (!isWaitingForNextTutorial)
            {
                gameController.updateGame = true;
                timeCount += Time.deltaTime;
                if (timeCount > timeDelay)
                {
                    timeCount = 0;
                    StartCoroutine("showNextTutorial");
                }
            }
        }
        void onBackKey()
        {
            onBtnPlayClick();
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
            guiHUD.SetActive(true);
            GameStatesManager.Instance.InputProcessor = guiHUD;
            init();
            onResume();
        }

        public override void onExit()
        {
            base.onExit();
            guiHUD.SetActive(false);
            gameController.isTutorial = false;
            gameController.updateGame = false;
        }

        void updateTutorialText(string text, bool visible)
        {
            lbTutorialText.text = text;
            if (visible)
            {
                lbTutorialText.gameObject.SetActive(true);
            }
            else
            {
                lbTutorialText.gameObject.SetActive(false);
            }
        }
        void updateTutorialHand(int _curent)
        {
            switch (tutorialActions[_curent])
            {
                case ETutorialAction.TouchLeft:
                    Utils.setActive(handLeft, true);
                    Utils.setActive(handRight, false);
                    break;

                case ETutorialAction.TouchRight:
                    Utils.setActive(handLeft, false);
                    Utils.setActive(handRight, true);
                    break;

                case ETutorialAction.TouchLeftRight:
                    Utils.setActive(handLeft, true);
                    Utils.setActive(handRight, true);
                    break;
                case ETutorialAction.ReleaseTouch:
                    Utils.setActive(handLeft, false);
                    Utils.setActive(handRight, false);
                    break;
                default:
                    Utils.setActive(handLeft, false);
                    Utils.setActive(handRight, false);
                    break;
            }
        }
        public IEnumerator showNextTutorial()
        {
            isWaitingForNextTutorial = true;
            ghostController.leftBlock.resetPivot();
            ghostController.rightBlock.resetPivot();
            if (currentTutorialActionIndex >= 0)
            {
                updateTutorialText(lbTutorialText.text, false);
                Utils.setActive(handLeft, false);
                Utils.setActive(handRight, false);
            }

            yield return new WaitForSeconds(1f);

            currentTutorialActionIndex++;

            if (currentTutorialActionIndex < tutorialActions.Length)
            {
                guiPlayBtn.SetActive(false);
                updateTutorialText(actions[tutorialActions[currentTutorialActionIndex]], true);
                updateTutorialHand(currentTutorialActionIndex);
            }
            else
            {
                guiPlayBtn.SetActive(true);
                updateTutorialText("GET READY!", true);
            }

            isWaitingForNextTutorial = false;
        }

        public void onBtnPlayClick()
        {
            GamePreferences.profile.EnableTutorial = false;
            GamePreferences.saveProfile();
            GameStatesManager.Instance.stateMachine.SwitchState(GSGamePlay.Instance);
        }
    }

    public enum ETutorialAction
    {
        TouchLeft,
        TouchRight,
        TouchLeftRight,
        ReleaseTouch,
    }
}