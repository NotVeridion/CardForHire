using UnityEngine;
using TMPro;

public class RewardPopupAnimator : MonoBehaviour
{
    public float riseSpeed = 1f;
    public float fadeSpeed = 1f;
    public float lifetime = 1.5f;

    private TMP_Text text;
    private Color color;

    void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        color = text.color;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;
        color.a -= fadeSpeed * Time.deltaTime;
        text.color = color;
    }
}