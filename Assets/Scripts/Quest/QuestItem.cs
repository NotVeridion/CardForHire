using UnityEngine;

public class QuestItem : MonoBehaviour
{
    [Header("Quest this item belongs to")]
    public string questID;

    [Header("Objective this item progresses")]
    public string objectiveID;
    public int amountToAdd = 1;

    [Header("Bounce Animation Settings")]
    public float bounceSpeed = 2f;
    public float bounceHeight = 0.1f;

    private Vector3 startPos;

    private void Start()
    {
        // Hide until the quest is actually active
        if (!QuestController.Instance.IsQuestActive(questID))
            gameObject.SetActive(false);

        // Listen for when quests are accepted
        QuestController.Instance.OnQuestAccepted += HandleQuestAccepted;

        startPos = transform.localPosition; 
    }
    private void Update()
    {
    // Bounce movement
        float offset = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.localPosition = startPos + new Vector3(0, offset, 0);
    }
    private void OnDestroy()
    {
        // Always clean up event subscriptions
        if (QuestController.Instance != null)
            QuestController.Instance.OnQuestAccepted -= HandleQuestAccepted;
    }

    // 🔥 Called when ANY quest is accepted
    private void HandleQuestAccepted(string acceptedQuestID)
    {
        if (acceptedQuestID == questID)
        {
            // This item's quest was just accepted → activate the item
            gameObject.SetActive(true);
        }
    }

    private void OnMouseDown()
    {
        QuestController.Instance.AddProgressToObjective(objectiveID, amountToAdd);
        Destroy(gameObject);
    }
}