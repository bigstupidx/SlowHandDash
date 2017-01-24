using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class VideoPopupComponent : MonoBehaviour {

    Action actionYes;
    Action actionNo;
    public Text txtGem;
    public void Init(int _gem, Action _yes, Action _no)
    {
        actionYes = _yes;
        actionNo = _no;
        txtGem.text = "" + _gem;
        Invoke("onClosePopup", 3);
    }
    public void onClosePopup()
    {
        if (actionNo != null)
            actionNo();
        Destroy(gameObject);
    }
    public void onRescue()
    {
        if (actionYes != null)
            actionYes();
        Destroy(gameObject);

    }
}
