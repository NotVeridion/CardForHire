using UnityEngine;
using TMPro;

public class RewardPopupManager : MonoBehaviour
{
    public static RewardPopupManager Instance;

    [Header("Popup Settings")]
    public GameObject popupPrefab;      
    public Canvas popupCanvas;          
    public float riseDistance = 50f;    
    public float lifetime = 1.5f;     

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowRewardPopup(Vector3 worldPos, int amount)
    {
        if (popupPrefab == null || popupCanvas == null)
        {
            return;
        }
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        GameObject popup = Instantiate(popupPrefab, popupCanvas.transform);
        RectTransform canvasRect = popupCanvas.GetComponent<RectTransform>();
RectTransform popupRect = popup.GetComponent<RectTransform>();

Vector2 anchoredPos;
if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
        canvasRect,
        screenPos,
        popupCanvas.worldCamera,
        out anchoredPos))
{
    popupRect.anchoredPosition = anchoredPos;
}

        TMP_Text text = popup.GetComponentInChildren<TMP_Text>();
        text.text = $"+${amount}";

        // Animate popup
        StartCoroutine(AnimatePopup(popup, text));
    }

    private System.Collections.IEnumerator AnimatePopup(GameObject popup, TMP_Text text)
    {
        float timer = 0f;
        Vector3 startPos = popup.transform.position;
        Color originalColor = text.color;

        while (timer < lifetime)
        {
            float t = timer / lifetime;

            // Rise upward
            popup.transform.position = startPos + Vector3.up * riseDistance * t;

            // Fade out
            Color c = originalColor;
            c.a = 1f - t;
            text.color = c;

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(popup);
    }
}