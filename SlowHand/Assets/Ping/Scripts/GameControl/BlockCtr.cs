using UnityEngine;
using System.Collections;
using System;

using Random = UnityEngine.Random;

namespace SlowHand
{
    public class BlockCtr : MonoBehaviour
    {
        bool isAlive;
        public Transform sprite;
        public Transform leftPivot;
        public Transform centerPivot;
        public Transform rightPivot;
        protected Vector3 _leftPivot;
        protected Vector3 _centerPivot;
        protected Vector3 _rightPivot;

        protected float targetX;
        public float smoothTime = 0.07f;
        public Transform topLeft;
        public Transform botRight;

        public GameObject trail;

        public ParticleSystem psExplode;
        public ParticleSystem psL;
        public ParticleSystem psH;

        public LayerMask layerMask;

        public AudioClip[] scrapeAudioClips;
        public AudioClip[] moveAudioClips;
        public AudioClip acExplode;

        public Action OnBlockExplode;

        public bool isEnterWall;
        public bool isGod;

        public void Init()
        {
            isAlive = true;
            isGod = false;
            resetPivot();
            sprite.gameObject.SetActive(true);
        }
        public virtual void updatePosition()
        {
            fixedUpdate();
        }
        //Collider2D[] result = new Collider2D[10];
        void fixedUpdate()
        {
            if (!isAlive)
                return;
            isEnterWall = false;
            Collider2D[] result = Physics2D.OverlapAreaAll(topLeft.position, botRight.position, layerMask);
            int cnt = result.Length;
            if (cnt > 0)
            {
                for (int i = 0; i < cnt; i++)
                {
                    if (result[i].CompareTag("Wall"))
                    {
                        isEnterWall = true;
                        ExecuteScrape();
                    }
                    if (result[i].CompareTag("Coin"))
                    {
                        result[i].GetComponent<StarController>().Explode();
                    }
                    if (!isGod && result[i].CompareTag("Spike"))
                    {
                        ExecuteExplode();
                    }
                }
            }
            if (!isEnterWall)
            {
                StopScrape();
            }
        }

        int currentScrapeId = -1;
        void playScrapeSound()
        {
            if (AudioManager.IsPlaying(currentScrapeId) == false && scrapeAudioClips != null && scrapeAudioClips.Length > 0)
            {
                int i = Random.Range(0, scrapeAudioClips.Length);
                //stopScrapeSound();
                currentScrapeId = AudioManager.PlaySound(scrapeAudioClips[i], this.transform, true);
            }
        }

        void stopScrapeSound()
        {
            if (currentScrapeId != -1)
            {
                AudioManager.StopAudioAt(currentScrapeId);
                currentScrapeId = -1;
            }
        }

        int currentMoveId = -1;
        protected void playMoveSound()
        {
            if (AudioManager.IsPlaying(currentMoveId) == false && moveAudioClips != null && moveAudioClips.Length > 0)
            {
                int i = Random.Range(0, moveAudioClips.Length);
                //stopMoveSound();
                currentMoveId = AudioManager.PlaySound(moveAudioClips[i], this.transform, false);
            }
        }

        protected void stopMoveSound()
        {
            if (currentMoveId != -1)
            {
                AudioManager.StopAudioAt(currentMoveId);
                currentMoveId = -1;
            }
        }

        void playExplodeSound()
        {
            AudioManager.PlaySound(acExplode, this.transform, false);
        }

        void ExecuteScrape()
        {
            if (!psL.isPlaying)
            {
                psL.Play();
                playScrapeSound();
            }
            if (!psH.isPlaying)
            {
                psH.Play();
            }
        }

        void StopScrape()
        {
            if (psL.isPlaying)
            {
                stopScrapeSound();
                psL.Stop();
                psH.Stop();
            }
        }
        void ExecuteExplode()
        {
            LeftBlockCtr.Instance.Explode();
            RightBlockCtr.Instance.Explode();
            playExplodeSound();
            if (OnBlockExplode != null)
            {
                OnBlockExplode();
            }
        }
        public void Explode()
        {
            if (isAlive)
            {
                isAlive = false;
                if (psExplode.isPlaying == false)
                {
                    psExplode.Play();
                }
                stopMoveSound();
                StopScrape();
                sprite.gameObject.SetActive(false);

                StopScrape();
                stopMoveSound();
            }
        }

        public void OnGhostDie()
        {
            //isAlive = false;
            //StopScrape();
            //stopMoveSound();
        }

        public Action OnCheckPoint;

        public void resetPivot()
        {
            _centerPivot = centerPivot.position;
            _leftPivot = leftPivot.position;
            _rightPivot = rightPivot.position;
        }
        public void fixLeftPivot()
        {
            _leftPivot = leftPivot.position;
            _centerPivot = _leftPivot;
            _rightPivot = _leftPivot;
            _centerPivot.x = _leftPivot.x + 1.5f;
            _rightPivot.x = _leftPivot.x + 3f;
        }
        public void fixRightPivot()
        {
            _rightPivot = rightPivot.position;
            _centerPivot = _rightPivot;
            _leftPivot = _rightPivot;
            _centerPivot.x = _rightPivot.x - 1.5f;
            _leftPivot.x = _rightPivot.x - 3f;
        }
        public void fixCenterPivot()
        {
            _centerPivot = centerPivot.position;
            _leftPivot = _centerPivot;
            _rightPivot = _centerPivot;
            _leftPivot.x = _centerPivot.x - 1.5f;
            _rightPivot.x = _centerPivot.x + 1.5f;
        }
    }
}