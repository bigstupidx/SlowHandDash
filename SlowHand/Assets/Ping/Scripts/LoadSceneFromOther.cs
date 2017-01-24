using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneFromOther : MonoBehaviour {
    public static bool isLoad = false;
	void Start () {
        if (LoadSceneFromOther.isLoad == false)
        {
            LoadSceneFromOther.isLoad = true;
            SceneManager.LoadScene(0);
        }
	}
}
