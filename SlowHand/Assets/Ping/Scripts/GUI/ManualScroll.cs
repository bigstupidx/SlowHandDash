using UnityEngine;
using System.Collections;

public class ManualScroll : MonoBehaviour {

    public float value;
    public float speed;
    public float offset;
    public Transform[] sprite;
    SpriteRenderer[] spriteRenderer;
    public float speedColor;
    public Color formColor;
    public Color toColor;

    void Start()
    {
        sprite[0].gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
        sprite[1].gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
        spriteRenderer = new SpriteRenderer[sprite.Length];
        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i] = sprite[i].GetComponent<SpriteRenderer>();
        }
        iTween.ValueTo(gameObject, iTween.Hash("from", formColor, "to", toColor, "time", speedColor, "looptype", iTween.LoopType.pingPong, "onUpdate", "UpdateColor"));
    }
	void FixedUpdate () {
        if (sprite[0].localPosition.y < -offset)
        {
            sprite[0].localPosition = new Vector2(0, offset + sprite[0].localPosition.y + offset);
        }

        if (sprite[1].localPosition.y < -offset)
        {
            sprite[1].localPosition = new Vector2(0, offset + sprite[1].localPosition.y + offset);
        }
    }
    void UpdateColor(Color newColor)
    {
        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = newColor;
        }
    }
}
