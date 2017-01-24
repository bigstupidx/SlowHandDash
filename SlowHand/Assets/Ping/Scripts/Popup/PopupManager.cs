using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace SlowHand
{
    public class PopupManager : MonoBehaviour
    {
        static PopupManager _instance;
        public static PopupManager Instance { get { return _instance; } }

        public Transform popUpRoot;
        public GameObject YesNoPopUpPrefab;
        public GameObject InfoPopUpPrefab;
        public GameObject RescuePrefab;
        public GameObject MesagePopUpPrefab;
        public GameObject LoadingUI;
        void Awake()
        {
            _instance = this;
            LoadingUI.SetActive(false);
        }
        public void InitYesNoPopUp(string message, Action yes, Action no, string _yes = "YES", string _no = "NO")
        {
            GameObject popup = null;
            popup = GameObject.Instantiate(YesNoPopUpPrefab) as GameObject;
            popup.SetActive(true);
            popup.transform.SetParent(popUpRoot);
            popup.transform.localPosition = Vector3.zero;
            popup.transform.localScale = Vector3.one;
            YesNoPopUpComponent script = popup.GetComponent<YesNoPopUpComponent>();
            script.Init(message, yes, no, _yes, _no);
        }
        
        public void InitInfoPopUp(string message, Action ok, string _ok = "OK")
        {
            GameObject popup = null;
            popup = GameObject.Instantiate(InfoPopUpPrefab) as GameObject;
            popup.SetActive(true);
            popup.transform.SetParent(popUpRoot);
            popup.transform.localPosition = Vector3.zero;
            popup.transform.localScale = Vector3.one;
            InfoPopUpComponent script = popup.GetComponent<InfoPopUpComponent>();
            script.Init(message, ok, _ok);
        }
        public void InitRescuePopUp(int gem, Action yes, Action no)
        {
            GameObject popup = null;
            popup = GameObject.Instantiate(RescuePrefab) as GameObject;
            popup.SetActive(true);
            popup.transform.SetParent(popUpRoot);
            popup.transform.localPosition = Vector3.zero;
            popup.transform.localScale = Vector3.one;
            VideoPopupComponent script = popup.GetComponent<VideoPopupComponent>();
            script.Init(gem, yes, no);
        }
        public void InitMesage(string message)
        {
            GameObject popup = null;
            popup = GameObject.Instantiate(MesagePopUpPrefab) as GameObject;
            popup.SetActive(true);
            popup.transform.SetParent(popUpRoot);
            popup.transform.localPosition = Vector3.zero;
            popup.transform.localScale = Vector3.one;
            MessagePopupComponent script = popup.GetComponent<MessagePopupComponent>();
            script.Init(message);
        }
        bool oldBackKeyState = false;
        bool revertBackKeyState = false;
        public void ShowLoading(bool disableBackKey = true)
        {
            if (disableBackKey && !revertBackKeyState)
            {
                revertBackKeyState = true;
                oldBackKeyState = GameStatesManager.enableBackKey;
                GameStatesManager.enableBackKey = false;
            }
            LoadingUI.SetActive(true);
        }
        public void HideLoading()
        {
            if (revertBackKeyState)
            {
                revertBackKeyState = false;
                GameStatesManager.enableBackKey = oldBackKeyState;
            }
            LoadingUI.SetActive(false);
        }
    }
}