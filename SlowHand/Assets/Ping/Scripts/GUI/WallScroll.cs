using UnityEngine;
using System.Collections;

public class WallScroll : MonoBehaviour {

    public float speed = 0.5f;
	Renderer render;
	// Use this for initialization
	void Start () {
		render = gameObject.GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 offset = new Vector2(0, Time.time * speed);
		render.material.mainTextureOffset = offset;
    }
}
