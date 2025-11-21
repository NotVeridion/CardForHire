using System.Collections.Generic;
using UnityEngine;

public class QuestScript : MonoBehaviour
{
    public bool HasQuestInProgress => activateQuests.Count > 0;
    public static QuestScript Instance { get; private set; }
    public List<Quest.QuestProgress> activateQuests = new();
    private QuestUI questUI;
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
        activateQuests.Add(new Quest.QuestProgress(quest));
        questUI.UpdateQuestUI();
    }
    public bool IsQuestActive(string questID) => activateQuests.Exists(q => q.QuestID == questID);
}