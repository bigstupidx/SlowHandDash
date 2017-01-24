using UnityEngine;
using System.Collections;

namespace SlowHand
{
    public class RightBlockCtr : BlockCtr
    {
        static RightBlockCtr _instance;

        public static RightBlockCtr Instance { get { return _instance; } }

        void Awake()
        {
            _instance = this;
        }
        float vel;
        public override void updatePosition()
        {

            if (GhostController.Instance.moveRight)
            {
                targetX = _rightPivot.x - 0.5f;
            }
            else
                if (GhostController.Instance.moveLeft)
                {
                    targetX = _leftPivot.x + 1.5f;
                }
                else
                {
                    targetX = _centerPivot.x + 0.5f;
                }
            Vector3 p = transform.position;
            p.x = Mathf.SmoothDamp(p.x, targetX, ref vel, smoothTime);
            if (Mathf.Approximately(p.x, transform.position.x) == false)
                playMoveSound();
            else
                stopMoveSound();
            transform.position = p;
            base.updatePosition();
        }
    }
}