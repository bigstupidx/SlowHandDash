using UnityEngine;
using System.Collections;
using System.IO;

public class ScreenShot : MonoBehaviour {

    private bool isProcessing = false;
    IEnumerator onScreenShot() {

        isProcessing = true;
        yield return new WaitForEndOfFrame();
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
        screenTexture.Apply();
        byte[] dataToSave = screenTexture.EncodeToPNG();
        //
        string destination = Path.Combine(Application.persistentDataPath, "(" +  Screen.width + ", " + Screen.height + ")" + System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
        File.WriteAllBytes(destination, dataToSave);
        Debug.Log(destination);
        isProcessing = false;
    }
	void TakeScreenShoot()
    {
        if (!isProcessing)
            StartCoroutine(onScreenShot());
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeScreenShoot();
        }
    }
}
