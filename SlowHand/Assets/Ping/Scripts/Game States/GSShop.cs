using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace SlowHand
{
    public class GSShop : IState
    {
        static GSShop _instance;
        public GameObject guiShop;
        public GameObject pfItem;
        public Transform gridItem;
        public Text txtStar;
        ShopItem[] listItem;
        
        public static GSShop Instance { get { return _instance; } }

        protected override void Awake()
        {
            base.Awake();
            _instance = this;
            guiShop.SetActive(false);
        }

        void init()
        {
            txtStar.text = GamePreferences.profile.Star.ToString();
            if (listItem == null)
            {
                listItem = new ShopItem[Utils.MAX_CUSTOMIZE];
                LoadShop();
            }
            else
            {
                ReLoadShop();
            }
            Select(GamePreferences.profile.Customize);
        }
        void LoadShop()
        {
            Utils.removeAllChildren(gridItem);
            for (int i = 0; i < Utils.MAX_CUSTOMIZE; i++)
            {
                GameObject item = Utils.Spawn(pfItem.gameObject, gridItem);
                ShopItem shopItem = item.GetComponent<ShopItem>();
                shopItem.Init(i);
                listItem[i] = shopItem;
            }
        }
        void ReLoadShop()
        {
            for (int i = 0; i < Utils.MAX_CUSTOMIZE; i++)
            {
                listItem[i].ReLoad();
            }
        }
        void ReLoadTextStar()
        {
            for (int i = 0; i < Utils.MAX_CUSTOMIZE; i++)
            {
                listItem[i].changeColorStar();
            }
        }
        public void ClickItem(ShopItem shopItem)
        {
            int id = shopItem.data.id;
            if (GamePreferences.profile.CustomizeInfo[id] == 0)
            {
                if (listItem[id].data.star > 0)
                {
                    UnLock(id);
                }
            }
            else
            {
                if (GamePreferences.profile.Customize == id)
                {
                    onBtnOkClick();
                }
                else
                {
                    Select(id);
                }
            }
        }
        void UnLock(int id)
        {
            if (GamePreferences.profile.Star >= listItem[id].data.star)
            {
                GamePreferences.profile.unLockCustomize(id);
                GamePreferences.profile.updateStar(0 - listItem[id].data.star);
                GamePreferences.saveProfile();
                txtStar.text = GamePreferences.profile.Star.ToString();
                listItem[id].UnLock();
                ReLoadTextStar();
            }
            else
            {
                PopupManager.Instance.InitMesage("not enough GEM");
            }
        }
        void Select(int id)
        {
            listItem[GamePreferences.profile.Customize].UnSelect();
            GamePreferences.profile.Customize = id;
            listItem[GamePreferences.profile.Customize].Select();

        }
        public void onBackKey()
        {
            onBtnOkClick();
        }
       
        public override void onSuspend()
        {
            GameStatesManager.OnBackKey = null;
        }

        public override void onResume()
        {
            GameStatesManager.OnBackKey = onBackKey;
        }

        public override void onEnter()
        {
            base.onEnter();
            guiShop.SetActive(true);
            GameStatesManager.Instance.InputProcessor = guiShop;
            init();
            onResume();
        }

        public override void onExit()
        {
            base.onExit();
            guiShop.SetActive(false);
        }

        public void onBtnOkClick()
        {
            GSGamePlay.Instance.UpgradeCharater(GamePreferences.profile.Customize);
            GamePreferences.saveProfile();
            GameStatesManager.Instance.stateMachine.SwitchState(GSGamePlay.Instance);
        }

    }
}