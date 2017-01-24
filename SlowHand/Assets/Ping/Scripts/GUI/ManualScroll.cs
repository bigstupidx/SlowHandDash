using UnityEngine;
using System.Collections;

public class ManualScroll : MonoBehaviour {

    public float value;
    public float speed;
    public float offset;
    public Transform[] sprite;

    void Start()
    {
        sprite[0].gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
        sprite[1].gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
    }
	void FixedUpdate () {
        //value -= Time.smoothDeltaTime * speed;
        //if (value < 0)
        //{
        //    value = offset;
        //}
        ////Vector3.MoveTowards()
        ////Mathf.Lerp()
        //sprite[0].localPosition = new Vector3(0, value, 0);
        //sprite[1].localPosition = new Vector3(0, value - offset, 0);
        if (sprite[0].localPosition.y < -offset)
        {
            sprite[0].localPosition = new Vector2(0, offset);
        }

        if (sprite[1].localPosition.y < -offset)
        {
            sprite[1].localPosition = new Vector2(0, offset);
        }
    }
}
