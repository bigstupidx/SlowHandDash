using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;

namespace SlowHand
{
    public class GameController : MonoBehaviour
    {
        static GameController _instance;
        public static GameController Instance
        {
            get { return _instance; }
        }
        Camera cameraGame;
        public Action OnGameOver;
        public bool gameOver = false;
        public bool updateGame;

        public CameraShake cameraShake;
        public Vector3 camVel = Vector3.zero;
        public Vector3 distanceRescueVel = new Vector3(0, -25, 0);
        public float maxCalVel;

        public GhostController ghost;
        public Transform startGhost;

        public Transform rootSpike;
        public GameObject[] prefabSpikes;

        public GameObject prefabCheckpoint;

        public Transform topLeft;
        public Transform botRight;

        public Transform beginLevelGap;
        public Transform startSpikeGap;

        bool beginSpawnSpike;
        PlatformController lastSpike;
        PlatformController lastCheckpoint;
        int lastSpikeType;

        int countRescue;
        int currentLevel = 0;
        int score = 0;
        int star = 0;
        public int Score
        {
            get { return score; }
            set
            {
                if (score != value)
                {
                    score = value;
                    textScore.text = "" + score;
                }
            }
        }
        public int Star
        {
            get { return star; }
            set
            {
                if (star != value)
                {
                    star = value;
                    if (textStar != null)
                    {
                        textStar.text = "" + star;
                    }
                }
            }
        }
        public Text textScore;
        public Text textStar;
        public AudioClip checkpointClip;
        public int platformsNb;
        SpikeGenerator spikeGen;

        void Start()
        {
            _instance = this;
            ObjectPoolManager.Init(rootSpike);
            for (int i = 0; i < prefabSpikes.Length; i++)
            {
                ObjectPoolManager.New(prefabSpikes[i], 3);
            }
            ObjectPoolManager.New(prefabCheckpoint, 1);
            cameraGame = GetComponent<Camera>();
            spikeGen = GetComponent<SpikeGenerator>();
        }

        void initVariable()
        {
            Score = 0;
            Star = 0;
            beginSpawnSpike = false;
            lastSpike = null;
            lastCheckpoint = null;
            camVel = GameConstants.Instance.getCammeraVelPerLevel(0);
        }

        void initHUD()
        {
            Score = 0;
            Star = 0;
        }

        public void startGame()
        {
            initVariable();
            UnspawnAllPlatform();

            currentLevel = 0;
            lastSpikeType = 0;
            lastCheckpoint = null;
            UpdateLRBLock();

            platformsNb = 0;
            countRescue = 0;
            initHUD();
            // Reset Ghost
            ghost.transform.position = startGhost.position;
            ghost.init();
            ghost.CanControl = true;
            ghost.OnGhostDie += OnGhostDie;

            gameOver = false;
            updateGame = true;
            isTutorial = false;
            Invoke("BeginSpawnSpike", 1.5f);
        }
        void BeginSpawnSpike()
        {
            beginSpawnSpike = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (updateGame && !gameOver)
            {
                UpdateSpikes();
                UpdateMove();
            }
            else if (updateGame && isTutorial)
            {
                UpdateMove();
            }
        }


        float ySpike;
        void UpdateSpikes()
        {
            if (beginSpawnSpike)
            {
                if (lastSpike == null)
                {
                    if (platformsNb < GameConstants.Instance.getSpikeNumperPerLever(currentLevel))
                    {
                        spawnSpike();
                    }
                }
                else
                {
                    if (lastSpike.transform.position.y < startSpikeGap.position.y)
                    {
                        if (platformsNb < GameConstants.Instance.getSpikeNumperPerLever(currentLevel))
                        {
                            spawnSpike();
                        }
                        else if (lastCheckpoint == null)
                        {
                            spawnCheckpoint();
                        }
                    }
                }
            }
        }
        void spawnSpike()
        {
            int i = spikeGen.GenSpike(currentLevel, lastSpikeType);
            GameObject go = ObjectPoolManager.Spawn(prefabSpikes[i]);
            go.tag = "Spike";
            int newSpikeType = (int)go.GetComponent<SpikeComponent>().spikeType;
            if (lastSpike != null)
            {
                ySpike = lastSpike.transform.position.y + spikeGen.getYDistance(lastSpikeType, newSpikeType, currentLevel);
            }
            else
            {
                ySpike = beginLevelGap.position.y;
            }
            lastSpike = go.GetComponent<PlatformController>();
            lastSpikeType = newSpikeType;
            Vector3 pos = Vector3.zero;
            pos.y = ySpike;
            go.transform.parent = rootSpike;
            go.transform.position = pos;
            platformsNb++;
        }
        void spawnCheckpoint()
        {
            ySpike = lastSpike.transform.position.y + GameConstants.checkPoint_yDistance;
            GameObject go = ObjectPoolManager.Spawn(prefabCheckpoint);
            Vector3 pos = Vector3.zero;
            pos.y = ySpike;
            go.transform.parent = rootSpike;
            go.transform.position = pos;
            lastCheckpoint = go.GetComponent<PlatformController>();
        }
        public float sleepCam = 0.5f;
        public IEnumerator NextLevel()
        {
            isWaitingForNextLevel = true;
            cameraShake.Shake();
            AudioManager.PlaySound(checkpointClip);
            GameConstants.Instance.InitNextTheme();
            yield return new WaitForSeconds(sleepCam);
            currentLevel++;
            platformsNb = 0;
            camVel = GameConstants.Instance.getCammeraVelPerLevel(currentLevel);
            UpdateLRBLock();
            lastCheckpoint = null;
            lastSpike = null;
            isWaitingForNextLevel = false;
        }
        void UpdateLRBLock()
        {
        }

        void UpdateMove()
        {
            if (camVel.y > maxCalVel)
                camVel.y = maxCalVel;
            cameraGame.transform.position += camVel * Time.deltaTime;
        }

        public void OnGhostDie()
        {
            cameraShake.Shake();
            if (countRescue == 0 && Score >= 20 && GameConstants.Instance.gemRescue < GamePreferences.profile.Star)
            {
                pauseGame();
                ShowRescue();

            }
            else
            {
                gameOver = true;
                updateGame = false;
                if (OnGameOver != null)
                    OnGameOver();
            }
        }
        void ShowRescue()
        {
            PopupManager.Instance.InitRescuePopUp(GameConstants.Instance.gemRescue, OnAcceptRescue, OnDeclineRescue);
        }
        void OnDeclineRescue()
        {
            gameOver = true;
            updateGame = false;
            if (OnGameOver != null)
                OnGameOver();
        }
        void OnAcceptRescue()
        {
            // Show Video Here
            Invoke("OnCallbackRescue", 0.5f);
        }
        void OnCallbackRescue()
        {
            // Callback show video
            // safe
            cameraGame.transform.position += distanceRescueVel;
            // Reset Ghost
            ghost.transform.position = startGhost.position;
            ghost.init();
            ghost.CanControl = true;
            ghost.OnGhostDie += OnGhostDie;
            // resume
            countRescue++;
            GamePreferences.profile.updateStar(0 - GameConstants.Instance.gemRescue);
            GamePreferences.saveProfile();
            resumeGame();
        }
        public void UnspawnAllPlatform()
        {
            PlatformController[] plts = rootSpike.GetComponentsInChildren<PlatformController>();
            for (int i = 0; i < plts.Length; i++)
            {
                plts[i].Deactivate();
            }
        }

        void OnEnable()
        {
            //OnGhostDie = UnspawnAllPlatform;
        }

        void Disable()
        {
            ghost.OnGhostDie = null;
        }

        public void addScore()
        {
            if (!gameOver)
                Score++;
        }


        public void onBtnPress()
        {
            ghost.CanControl = false;
        }

        public void onBtnRelease()
        {
            ghost.CanControl = true;
        }

        public void pauseGame()
        {
            updateGame = false;
            ghost.CanControl = false;
        }

        public void resumeGame()
        {
            updateGame = true;
            ghost.CanControl = true;
        }

        bool isWaitingForNextLevel = false;
        public float cpVel = 10f;
        public void OnCheckPoint()
        {
            camVel *= cpVel;
            if (isWaitingForNextLevel == false)
            {
                StartCoroutine("NextLevel");
            }
        }
        /// <summary>
        /// TUTORIAL
        /// </summary>
        public void startTutorial()
        {
            UnspawnAllPlatform();
            isTutorial = true;
            gameOver = true;
            updateGame = false;
            camVel = GameConstants.Instance.getCammeraVelPerLevel(0);
            ghost.InitTutorial();
        }
        //use for control spike in tutorial
        public bool isTutorial = false;
        public void SpawnSpikeTutorial()
        {
            float _offset = 11;
            SpawnSpikeByType(ESpikeType.BigR, beginLevelGap.position.y + 0 * _offset);
            SpawnSpikeByType(ESpikeType.BigL, beginLevelGap.position.y + 1 * _offset);
            SpawnSpikeByType(ESpikeType.CenterBig, beginLevelGap.position.y + 2 * _offset);
            SpawnSpikeByType(ESpikeType.LFSmall, beginLevelGap.position.y + 3 * _offset - 2);
        }
        void SpawnSpikeByType(ESpikeType _type, float _yy)
        {
            GameObject go = ObjectPoolManager.Spawn(prefabSpikes[(int)_type]);
            go.tag = "Spike";
            Vector3 pos = Vector3.zero;
            pos.y = _yy;
            go.transform.parent = rootSpike;
            go.transform.position = pos;
            Utils.setActive(go, true);
        }
    }
}