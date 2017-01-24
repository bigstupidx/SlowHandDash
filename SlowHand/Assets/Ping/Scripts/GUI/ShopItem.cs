using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SlowHand
{
    public class ShopItem : MonoBehaviour
    {

        public GameObject infoObject;
        public GameObject lockObject;
        public GameObject EffectChoose;
        public Image iconL;
        public Image iconR;
        public Image iconOffL;
        public Image iconOffR;
        public Text txtStar;
        public Text txtName;
        public Color colorGem;
        public Color colorNoGem;

        public DataCustomize data;
        public void Init(int id)
        {
            data = Data.Instance.GetCustomize(id);
            UnSelect();
            if (data != null)
            {
                txtStar.text = data.star.ToString();
                txtName.text = data.name;
            }
            iconL.sprite = GameConstants.Instance.getBlockBlue(id);
            iconR.sprite = GameConstants.Instance.getBlockRed(id);
            iconOffL.sprite = GameConstants.Instance.getBlockBlack(id);
            iconOffR.sprite = GameConstants.Instance.getBlockBlack(id);
            ReLoad();
        }
        // Update is called once per frame
        public void Select()
        {
            Utils.setActive(EffectChoose, true);
        }
        public void UnSelect()
        {
            Utils.setActive(EffectChoose, false);

        }
        public void UnLock()
        {
            Utils.setActive(lockObject, false);
            Utils.setActive(infoObject, true);
            Utils.setActive(EffectChoose, false);
        }
        public void Lock()
        {
            Utils.setActive(lockObject, true);
            Utils.setActive(infoObject, false);
            Utils.setActive(EffectChoose, false);
        }
        public void changeColorStar()
        {
            if (data.star > 0)
            {
                if (data.star <= GamePreferences.profile.Star)
                {
                    txtStar.color = colorGem;
                }
                else
                {
                    txtStar.color = colorNoGem;
                }
            }
        }
        public void ReLoad()
        {
            int _lock = GamePreferences.profile.CustomizeInfo[data.id];
            if (_lock == 1)
            {
                UnLock();
            }
            else
            {
                Lock();
                changeColorStar();
            }
        }
        public void OnClickItem()
        {
            GSShop.Instance.ClickItem(this);
        }
    }
}
