using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace SlowHand
{
    public class InfoPopUpComponent : PopupAnim
    {
        Action actionOK;
        string message;
        string txtOk;
        public Text messageLbl;
        public Text okLbl;

        public void Init(string message, Action ok, string _ok = "OK")
        {
            this.message = message;
            this.txtOk = _ok;
            actionOK = ok;
            messageLbl.text = this.message;
            okLbl.text = this.txtOk;
        }
        public void OnOkBtnClicked()
        {
            if (actionOK != null)
                actionOK();
            StartCoroutine(FadeOut());
        }
    }
}
