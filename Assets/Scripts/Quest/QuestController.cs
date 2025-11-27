using System.Collections.Generic;
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

    public delegate void QuestAcceptedEvent(string questID);
    public event QuestAcceptedEvent OnQuestAccepted;

    public delegate void QuestProgressEvent(string questID);
    public event QuestProgressEvent OnQuestProgressUpdated;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        questUI = FindObjectOfType<QuestUI>();
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



    // 🔥 Notify listeners that a quest was accepted
    OnQuestAccepted?.Invoke(quest.questID);
    }
    public bool IsQuestActive(string questID) => activateQuests.Exists(q => q.QuestID == questID);
    /*
    public void CheckInventoryForQuests()
    {
        Dictionary<int,int> itemCounts = InventoryController.Instance.GetItemCounts();
        foreach(QuestProgress quest in activateQuests)
        {
            foreach(QuestObjective questObjective in quest.objectives)
            {
                if(questObjective.type != ObjectiveType.CollectItem) continue;
                if(!int.TryParse(questObjective.objectiveID,out int itemID)) continue;
                int newAmount = itemCounts.TryGetValue(itemID, out int count) ? Mathf.Min(count, questObjective.requiredAmount) :0;
                if(questObjective.currentAmount != newAmount)
                {
                    questObjective.currentAmount = newAmount;
                }
            }
        }
        questUI.UpdateQuestUI();
    }
    */
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

    if (quest != null)
    {
        RemoveCompletedQuest(quest);
    }


}

    public bool IsQuestHandedIn(string questID)
    {
        return handinQuestIDs.Contains(questID);
    }

    /*
    public bool RemoveRequiredItemsFromInventory(string questID)
    {
        QuestProgress quest = activateQuests.Find(q => q.QuestID == questID);
        if (quest == null) return false;
        Dictionary<int,int> requiredItems = new();

        foreach(QuestObjective objective in quest.objectives)
        {
            if(objective.type == ObjectiveType.CollectItem && int.TryParse(objective.objectiveID, out int itemID))
            {
                requiredItems[itemID] = objective.requiredAmount;
            }
        }

        Dictionary<int, int> itemCounts = InventoryController.Instance.GetItemCounts();
        foreach(var item in requiredItems)
        {
            if(itemCounts.GetValueOrDefault(item.Key) < item.Value)
            {
                return false;
            }
        }
        foreach(var itemRequirement in requiredItems)
        {
            InventoryController.Instance.RemoveRequiredItemsFromInventory(itemRequirement.Key, itemRequirement.Value);
        }

        return true;
        
    }
    */

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

                Debug.Log($"Quest Progress Updated: {objectiveID} → {objective.currentAmount}/{objective.requiredAmount}");

                // Update UI after change
                questUI.UpdateQuestUI();

                // Notify listeners (NPCs, markers, etc.) that this quest's progress changed
                OnQuestProgressUpdated?.Invoke(quest.QuestID);

                return;
            }
        }
    }

    Debug.LogWarning($"No active quest objective found with ID: {objectiveID}");


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
}

