using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quests")]
public class Quest : ScriptableObject
{
    public string questID;
    public string questName;
    public string description;

    [Header("Quest Giver")]
    public string questGiverID;   

    [Header("Rewards")] 
    public int cashReward = 0; 
    [Header("Reward Audio")] 
    public AudioClip rewardSFX;

    public List<QuestObjective> objectives;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(questID))
            questID = questName + Guid.NewGuid().ToString();
    }

    [System.Serializable]
    public class QuestObjective
    {
        public string objectiveID;
        public string description;
        public ObjectiveType type;
        public int requiredAmount;
        public int currentAmount;
        public bool isCompleted => currentAmount >= requiredAmount;
    }

    public enum ObjectiveType { CollectItem, DefeatEnemy, ReachLocation, TalkNPC, Custom }

    [System.Serializable]
    public class QuestProgress
    {
        public Quest quest;
        public List<QuestObjective> objectives;

        public QuestProgress(Quest quest)
        {
            this.quest = quest;
            objectives = new List<QuestObjective>();
            foreach (var obj in quest.objectives)
            {
                objectives.Add(new QuestObjective
                {
                    objectiveID = obj.objectiveID,
                    description = obj.description,
                    type = obj.type,
                    requiredAmount = obj.requiredAmount,
                    currentAmount = 0
                });
            }
        }

        public bool isCompleted => objectives.TrueForAll(o => o.isCompleted);
        public string QuestID => quest.questID;
    }
}