using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace SlowHand
{
    public class YesNoPopUpComponent : PopupAnim
    {
        Action actionYes;
        Action actionNo;
        string message;
        string txtNo;
        string txtYes;
        public Text messageLbl;
        public Text noLbl;
        public Text yesLbl;

        public void Init(string message, Action yes, Action no, string _yes = "YES", string _no = "NO")
        {
            this.message = message;
            this.txtNo = _no;
            this.txtYes = _yes;
            actionYes = yes;
            actionNo = no;
            messageLbl.text = this.message;
            yesLbl.text = this.txtYes;
            noLbl.text = this.txtNo;
            messageLbl.text = this.message;
        }
        public void OnYesBtnClicked()
        {
            StartCoroutine(FadeOut());
            if (actionYes != null)
                actionYes();
        }
        public void OnNoBtnClicked()
        {
            StartCoroutine(FadeOut());
            if (actionNo != null)
                actionNo();
        }
    }
}
