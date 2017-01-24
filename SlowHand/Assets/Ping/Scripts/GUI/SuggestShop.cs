using UnityEngine;
using System.Collections;

namespace SlowHand
{
    public class SuggestShop : MonoBehaviour {

        public GameObject _noti;
        void OnEnable() {
            Utils.setActive(_noti, checkAvailable());
        }
        bool checkAvailable()
        {
            DataCustomize data;
            bool _check = false;
            for (int i = 0; i < Utils.MAX_CUSTOMIZE; i++)
            {
                data = Data.Instance.GetCustomize(i);
                if (data != null && data.star >= 0 && GamePreferences.profile.Star >= data.star && GamePreferences.profile.CustomizeInfo[i] != 1)
                {
                    _check = true;
                }
            }
            return _check;
        }
    }
}
