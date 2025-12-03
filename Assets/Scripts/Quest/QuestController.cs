using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using static Quest;

public class QuestController : MonoBehaviour
{
    public bool HasQuestInProgress => activateQuests.Count > 0;
    public static QuestController Instance { get; private set; }
    public List<QuestProgress> activateQuests = new();
    public QuestUI questUI;

    public List<string> handinQuestIDs = new();
    public string activeQuestGiverID = "";
    public GameObject rewardPopupPrefab;

    [Header("Global Quest Stats")]
    public int totalQuestsCompleted = 0;
    public delegate void QuestAcceptedEvent(string questID);
    public event QuestAcceptedEvent OnQuestAccepted;

    public delegate void QuestProgressEvent(string questID);
    public event QuestProgressEvent OnQuestProgressUpdated;

    private void Start()
    {
        TrySubscribeToEnemyTracker();
    }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        questUI = FindObjectOfType<QuestUI>();
        TrySubscribeToEnemyTracker();
    }
    public void AcceptQuest(Quest quest)
    {
        if (HasQuestInProgress)
    {
        Debug.Log("Cannot accept new quest: already have a quest in progress.");
        return;
    }
    if (IsQuestActive(quest.questID))
        return;

    activateQuests.Add(new QuestProgress(quest));
    questUI.UpdateQuestUI();

    activeQuestGiverID = quest.questGiverID;

    OnQuestAccepted?.Invoke(quest.questID);
    }
    public bool IsQuestActive(string questID) => activateQuests.Exists(q => q.QuestID == questID);
    public void LoadQuestProgress(List<QuestProgress> savedQuests)
    {
        activateQuests = savedQuests ?? new();
        //CheckInventoryForQuests();
        questUI.UpdateQuestUI();
    }

    public bool IsQuestCompleted(string questID)
    {
        QuestProgress quest = activateQuests.Find(q => q.QuestID == questID);
        return quest != null && quest.objectives.TrueForAll(o => o.isCompleted);
    }

    public void HandInQuest(string questID)
    {
    QuestProgress quest = activateQuests.Find(q => q.QuestID == questID);

    if (quest == null)
    {
        return;
    }

    if (handinQuestIDs.Contains(questID))
    {
        return;
    }

    int rewardAmount = quest.quest.cashReward;
    PlayerScript player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
    if (player != null)
    {
        player.playerCash += rewardAmount;
        if (quest.quest.rewardSFX != null)
        {
            AudioSource.PlayClipAtPoint(quest.quest.rewardSFX, player.transform.position);
        }
        PlayerScript playerObj = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        RewardPopupManager.Instance.ShowRewardPopup(playerObj.transform.position + (Vector3.up * 1.5f), rewardAmount);

    }
    totalQuestsCompleted++;
    Debug.Log($"[QUEST] Total quests completed so far: {totalQuestsCompleted}");
    handinQuestIDs.Add(questID);
    activateQuests.Remove(quest);
    questUI.UpdateQuestUI();
    OnQuestProgressUpdated?.Invoke(questID);

    activeQuestGiverID = "";



}

    public bool IsQuestHandedIn(string questID)
    {
        return handinQuestIDs.Contains(questID);
    }

    

    public void AddProgressToObjective(string objectiveID, int amount)
    {
 foreach (var quest in activateQuests)
    {
        foreach (var objective in quest.objectives)
        {
            if (objective.objectiveID == objectiveID &&
                objective.type == ObjectiveType.CollectItem)
            {
                objective.currentAmount += amount;

                if (objective.currentAmount > objective.requiredAmount)
                    objective.currentAmount = objective.requiredAmount;

                // Update UI after change
                questUI.UpdateQuestUI();
                OnQuestProgressUpdated?.Invoke(quest.QuestID);

                return;
            }
        }
    }


    }

    public void RemoveCompletedQuest(QuestProgress quest)
    {
        if (activateQuests.Contains(quest))
        {
            handinQuestIDs.Add(quest.QuestID);
            activateQuests.Remove(quest);
            questUI.UpdateQuestUI();
        }
    }

    public void NotifyQuestProgressUpdated(string questID)
    {
        OnQuestProgressUpdated?.Invoke(questID);
    }
    //Use this to get the number of quests completed so far
    public int GetTotalCompletedQuests()
    {
        return totalQuestsCompleted;
    }
    private void OnEnemyDefeated(string enemyID)
{
    foreach (var quest in activateQuests)
    {
        foreach (var objective in quest.objectives)
        {
            if (objective.type == ObjectiveType.DefeatEnemy &&
                objective.objectiveID == enemyID &&
                objective.currentAmount < objective.requiredAmount)
            {
                objective.currentAmount++;

                questUI.UpdateQuestUI();

                OnQuestProgressUpdated?.Invoke(quest.QuestID);

                return;
            }
        }
    }
}



private void TrySubscribeToEnemyTracker()
{
    if (EnemyDefeatTracker.Instance != null)
    {
        EnemyDefeatTracker.Instance.OnEnemyDefeated += OnEnemyDefeated;
        return;
    }
}
}

