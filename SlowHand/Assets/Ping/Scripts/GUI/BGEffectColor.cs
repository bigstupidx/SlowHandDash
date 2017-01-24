using UnityEngine;
using System.Collections;

public class BGEffectColor : MonoBehaviour {
    const float brightness = 0.235f;
    const float saturation = 0.353f;
    public float hue;
    public float speed = 0.000156f;
    public SpriteRenderer _renderer;
    // Use this for initialization
    void Start () {
        hue = 0.6f;
        if (_renderer == null)
        {
            _renderer = gameObject.GetComponent<SpriteRenderer>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        hue += speed;
        if (hue > 1)
        {
            hue -= 1;
        }
        _renderer.color = Color.HSVToRGB(hue, saturation, brightness);
	}
}
