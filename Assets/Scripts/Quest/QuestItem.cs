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
        if (!QuestController.Instance.IsQuestActive(questID))
            gameObject.SetActive(false);

        QuestController.Instance.OnQuestAccepted += HandleQuestAccepted;

        startPos = transform.localPosition; 
    }
    private void Update()
    {
        float offset = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.localPosition = startPos + new Vector3(0, offset, 0);
    }
    private void OnDestroy()
    {
        if (QuestController.Instance != null)
            QuestController.Instance.OnQuestAccepted -= HandleQuestAccepted;
    }

    private void HandleQuestAccepted(string acceptedQuestID)
    {
        if (acceptedQuestID == questID)
        {
            gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter2D()
    {
        QuestController.Instance.AddProgressToObjective(objectiveID, amountToAdd);
        Destroy(gameObject);
    }
}