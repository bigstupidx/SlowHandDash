using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class VideoPopupComponent : MonoBehaviour {

    Action actionYes;
    Action actionNo;
    public Text txtGem;
    public Image fill;
    public float timeLive = 4;
    float currentSize;
    public void Init(int _gem, Action _yes, Action _no)
    {
        actionYes = _yes;
        actionNo = _no;
        txtGem.text = "" + _gem;
        currentSize = 1;
    }
    void Update()
    {
        currentSize -= Time.deltaTime / timeLive;
        fill.fillAmount = currentSize;
        if (currentSize <= 0)
        {
            onClosePopup();
        }
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
