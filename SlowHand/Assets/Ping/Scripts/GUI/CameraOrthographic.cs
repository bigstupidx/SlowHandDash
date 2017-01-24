using UnityEngine;
using System.Collections;

public class CameraOrthographic : MonoBehaviour {

    public float currentOrthorgraphic = 9.0f;
    public float currentRatioHeight = 16;
    public float currentRatioWidth = 9;

    private float orthorgraphicCamera;
	// Use this for initialization
	void Start () {
        orthorgraphicCamera = (Screen.height * currentRatioWidth) / (Screen.width * currentRatioHeight) * currentOrthorgraphic;
        Debug.Log("Orthorgraphic Camera = " + orthorgraphicCamera);
        Camera.main.orthographicSize = orthorgraphicCamera;
	}
	
}
