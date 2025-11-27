using UnityEngine;

public class QuestMarkerController : MonoBehaviour
{
    public SpriteRenderer sr;
    public Sprite questionMark;
    public Sprite exclamationMark;

    [Header("Colors")]
    public Color availableColor = Color.yellow;
    public Color inProgressColor = Color.gray;
    public Color hiddenColor = new Color(1,1,1,0);

    [Header("Bounce Settings")]
    public float bounceSpeed = 2f;
    public float bounceHeight = 0.1f;

    private Vector3 startPos;

    void Start()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        startPos = transform.localPosition;
    }

    void Update()
    {
        // Float/bounce animation
        float offset = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.localPosition = startPos + new Vector3(0, offset, 0);
    }

    public void ShowQuestionAvailable()
    {
        sr.sprite = questionMark;
        sr.color = availableColor;
        sr.enabled = true;
    }

    public void ShowQuestionInProgress()
    {
        sr.sprite = questionMark;
        sr.color = inProgressColor; 
        sr.enabled = true;
    }

    public void ShowExclamationReady()
    {
        sr.sprite = exclamationMark;
        sr.color = availableColor;
        sr.enabled = true;
    }

    public void HideMarker()
    {
        sr.enabled = false;
    }
}