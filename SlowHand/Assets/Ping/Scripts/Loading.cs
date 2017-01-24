using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {

	// Use this for initialization
	void Start () {
        LoadSceneFromOther.isLoad = true;
        Invoke("Finish", 2);
    }
	
	// Update is called once per frame
	void Finish () {
        SceneManager.LoadScene(1);
	}
}
