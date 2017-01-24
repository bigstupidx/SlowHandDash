using UnityEngine;
using System.Collections;
using System;

namespace SlowHand
{
    public class GhostController : MonoBehaviour
    {
        static GhostController _instance;
        public static GhostController Instance
        {
            get { return _instance; }
        }

        public bool isAlive;
        bool canControl;
        public bool godMode;

        public BlockCtr leftBlock;
        public BlockCtr rightBlock;

        public BlockConnection blockConn;

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }
        public bool CanControl
        {
            get { return canControl; }
            set { canControl = value; }
        }

        void Awake()
        {
            _instance = this;
        }

        public bool moveLeft;
        public bool moveRight;

        bool MoveLeft
        {
            get { return moveLeft; }
            set { moveLeft = value; }
        }
        bool MoveRight
        {
            get { return moveRight; }
            set { moveRight = value; }
        }
        public void init()
        {
            canControl = false;
            isAlive = true;

            MoveLeft = MoveRight = false;

            OnGhostDie = leftBlock.OnGhostDie;
            OnGhostDie += rightBlock.OnGhostDie;

            leftBlock.Init();
            rightBlock.Init();

            blockConn.gameObject.SetActive(true);

            leftBlock.OnBlockExplode = OnBlockDie;
            rightBlock.OnBlockExplode = OnBlockDie;
        }

        public void InitTutorial()
        {
            isAlive = true;
            canControl = true;
            leftBlock.Init();
            rightBlock.Init();
            leftBlock.isGod = true;
            rightBlock.isGod = true;
            blockConn.gameObject.SetActive(true);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!IsAlive)
                return;

            updateGhost();
        }
        void updateGhost()
        {
            moveLeft = moveRight = false;
            if (canControl)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Input.touches[i].position.y < 0.80f * Screen.height)
                    {
                        if (Input.touches[i].position.x < 0.5f * Screen.width)
                            moveLeft = true;
                        else
                            moveRight = true;
                    }
                }
#if UNITY_EDITOR
                if (Input.GetMouseButton(0))
                {
                    Vector3 po = Input.mousePosition;
                    if (po.y < 0.80f * Screen.height)
                    {
                        if (po.x < 0.5f * Screen.width)
                            moveLeft = true;
                        else
                            moveRight = true;
                    }
                }
#endif
                if (Input.GetButton("Right"))
                    moveRight = true;
                if (Input.GetButton("Left"))
                    moveLeft = true;
            }

            leftBlock.updatePosition();
            rightBlock.updatePosition();
            blockConn.UpdateConnection(leftBlock.transform.localPosition, rightBlock.transform.localPosition);
        }
        public Action OnGhostDie;

        void OnBlockDie()
        {
            isAlive = false;
            canControl = false;
            blockConn.gameObject.SetActive(false);
            if (OnGhostDie != null)
            {
                OnGhostDie();
            }
        }
    }
}