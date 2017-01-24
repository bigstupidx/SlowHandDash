using UnityEngine;
using System.Collections;

namespace SlowHand
{
    public class SpikeComponent : MonoBehaviour
    {
        public ESpikeType spikeType;
    }

    public enum ESpikeType
    {
        BigL,
        BigR,
        CenterBig,
        CenterSmall,
        LFSmall,
        TotalCount,
    }
}

