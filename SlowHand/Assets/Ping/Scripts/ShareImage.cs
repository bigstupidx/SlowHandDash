﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;
using System.Runtime.InteropServices;

public class ShareImage : MonoBehaviour {

    private bool isProcessing = false;
    private string message = "WOW! #BlockDash! Play with me it's free: ";
    private string gameLink = "\nAndroid: " + Utils.LINK_ANDROID_STORE + "\nIOS: " + Utils.LINK_IOS_STORE;
    public void shareImage()
    {
        if (!isProcessing)
            StartCoroutine(ShareScreenshot());
    }
    // Take a Photo and share
    private IEnumerator ShareScreenshot()
    {
        isProcessing = true;
        yield return new WaitForEndOfFrame();
        // take screen shot
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
        screenTexture.Apply();
        byte[] dataToSave = screenTexture.EncodeToPNG();
        // save
        string destination = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
        File.WriteAllBytes(destination, dataToSave);

        ShareImageFromPath(destination);
        isProcessing = false;
    }

#if UNITY_ANDROID
    // Share Image
    private void ShareImageFromPath(string _destination)
    {
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        // Set action for intent
        //EXTRA_EMAIL, EXTRA_CC, EXTRA_BCC, EXTRA_SUBJECT, EXTRA_TITLE, EXTRA_TEXT, EXTRA_STREAM,
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), message + gameLink);
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + _destination);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
        intentObject.Call<AndroidJavaObject>("setType", "image/*");
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
        currentActivity.Call("startActivity", jChooser);
    }
#elif UNITY_IOS
    // Share Image
    private void ShareImageFromPath(string _destination)
    {
        GeneralSharingiOSBridge.ShareTextWithImage(_destination, message + gameLink);
    }
#endif
}
