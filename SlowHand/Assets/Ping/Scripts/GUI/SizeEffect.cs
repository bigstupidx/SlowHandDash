using UnityEngine;
using UnityEngine.UI;

public class SizeEffect : MonoBehaviour
{
    public float speed;
    public float currentSize;
    public int maxSize;
    RectTransform rect;
    Image image;
    void Start()
    {
        rect = gameObject.GetComponent<RectTransform>();
        image = gameObject.GetComponent<Image>();
    }
    void Update()
    {
        currentSize = Mathf.Repeat(currentSize + speed * Time.deltaTime, maxSize);
        rect.sizeDelta = new Vector2(currentSize, currentSize);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1 - currentSize / (maxSize * 2));
    }
}
