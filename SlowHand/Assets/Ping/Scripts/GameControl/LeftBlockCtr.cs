using UnityEngine;
using System.Collections;

namespace SlowHand
{
    public class LeftBlockCtr : BlockCtr
    {
        static LeftBlockCtr _instance;
        public static LeftBlockCtr Instance { get { return _instance; } }
        void Awake()
        {
            _instance = this;
        }
        float vel;
        public override void updatePosition()
        {
            if (GhostController.Instance.moveLeft)
            {
                targetX = _leftPivot.x + 0.5f;
            }
            else if (GhostController.Instance.moveRight)
            {
                targetX = _rightPivot.x - 1.5f;
            }
            else
            {
                targetX = _centerPivot.x - 0.5f;
            }
            Vector3 p = transform.position;
            p.x = Mathf.SmoothDamp(p.x, targetX, ref vel, smoothTime);
            if (!Mathf.Approximately(p.x, transform.position.x))
                playMoveSound();
            else
                stopMoveSound();
            transform.position = p;
            base.updatePosition();
        }
    }
}