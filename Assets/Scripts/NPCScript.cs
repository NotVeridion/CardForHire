using UnityEngine;
using System.Collections;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;

public class NPCScript : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;

    [Header("NPC Identifier")]
    public string npcID;
    private QuestMarkerController marker;
    private DialogueController dialogueUI;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private enum QuestState { NotStarted, InProgress, Completed, PostCompleted }
    private QuestState questState = QuestState.NotStarted;
    private void Start()
    {
 dialogueUI = DialogueController.Instance;
    marker = GetComponentInChildren<QuestMarkerController>();

    // Ensure marker reflects current state at start
    UpdateMarker();

    // Subscribe to quest progress updates so marker refreshes when items are picked up
    if (QuestController.Instance != null)
        QuestController.Instance.OnQuestProgressUpdated += OnQuestProgressUpdated;
    }

   private void OnMouseDown()
{
    // If player clicks the NPC while dialogue is active
    if (isDialogueActive)
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
            Debug.Log("skip typing");
        }
        else
        {
            NextLine();
        }
    }
    else
    {
        // Start dialogue if clicked and not in dialogue mode
        StartDialogue();
    }
}
    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }
    void StartDialogue()
    {
        SyncQuestState();
    CheckTalkNPCObjectives();
    // 🔥 If quest was completed before talking, hand it in immediately
    if (questState == QuestState.Completed)
    {
        QuestController.Instance.HandInQuest(dialogueData.quest.questID);
        Debug.Log("Quest handed in!");

        // Refresh state after hand-in
        //questState = QuestState.PostCompleted;
        marker.HideMarker();
    }

    // After hand-in or normal state detection, choose dialogue path
    if (questState == QuestState.NotStarted)
    {
        dialogueIndex = 0;
    }
    else if (questState == QuestState.InProgress)
    {
        dialogueIndex = dialogueData.questInProgressIndex;
    }
    else if (questState == QuestState.Completed)
    {
        dialogueIndex = dialogueData.questCompletedIndex;
    }
        else if (questState == QuestState.PostCompleted)
    {
        dialogueIndex = dialogueData.questPostCompletedIndex;
    }
    else
    {
        Debug.Log("Quest State Not Found!");
    }

    isDialogueActive = true;
    dialogueUI.SetNPCInfo(dialogueData.npcName);
    dialogueUI.ShowDialogueUI(true);

    DisplayCurrentLine();
    UpdateMarker();
    }

    private void SyncQuestState()
    {
        if (dialogueData.quest == null)
        return;

    string questID = dialogueData.quest.questID;

    // If quest has been handed-in (turned in already)
    if (QuestController.Instance.IsQuestHandedIn(questID))
    {
        questState = QuestState.Completed;
        return;
    }

    // If quest is active but not completed
    if (QuestController.Instance.IsQuestActive(questID))
    {
        // If the quest is fully completed, move to Completed state
        if (QuestController.Instance.IsQuestCompleted(questID))
        {
            questState = QuestState.Completed;
        }
        else
        {
            questState = QuestState.InProgress;
        }

        return;
    }

    // No quest active yet
    questState = QuestState.NotStarted;
    }

    void DisplayChoices(DialogueChoice choice)
    {
        for (int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            bool givesQuest = choice.givesQuest[i];
            dialogueUI.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex, givesQuest));
        }
    }
    void ChooseOption(int nextIndex, bool givesQuest)
    {
         // If giving quest but player already has one
    if (givesQuest && QuestController.Instance.HasQuestInProgress)
    {
        Debug.Log("Player already has a quest in progress — cannot take another.");
        dialogueUI.SetDialogueText("You must finish your current quest first!");
        return;
    }

    // Accept new quest
    if (givesQuest)
    {
        QuestController.Instance.AcceptQuest(dialogueData.quest);
        questState = QuestState.InProgress;
        Debug.Log("Quest started!");
        UpdateMarker();
    }


    dialogueIndex = nextIndex;
    dialogueUI.ClearChoices();
    DisplayCurrentLine();
    }
    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }
    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);
        //PauseController.SetPause(false);
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueText("");
        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueUI.SetDialogueText(dialogueUI.dialogueText.text += letter);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }
        isTyping = false;
        Debug.Log("done typing");
    }
    public void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
            Debug.Log("skip typing");
            
        }
        dialogueUI.ClearChoices();
        foreach (DialogueChoice dialogueChoice in dialogueData.choices)
        {
            if (dialogueChoice.dialogueIndex == dialogueIndex)
            {
                DisplayChoices(dialogueChoice);
                return;
            }
        }
        if (dialogueData.endDialogueLines.Length > dialogueIndex && dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }

        if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    private void UpdateMarker()
{
    if (dialogueData.quest == null || marker == null)
    {
        marker?.HideMarker();
        return;
    }

    string questID = dialogueData.quest.questID;

    // Player finished quest and handed it in
    if (QuestController.Instance.IsQuestHandedIn(questID))
    {
        marker.HideMarker();
        return;
    }

    // Player has not yet accepted the quest
    if (!QuestController.Instance.IsQuestActive(questID))
    {
        marker.ShowQuestionAvailable();
        return;
    }

    // Player accepted the quest but hasn't completed it yet
    if (QuestController.Instance.IsQuestActive(questID) &&
        !QuestController.Instance.IsQuestCompleted(questID))
    {
        marker.ShowQuestionInProgress();
        return;
    }

    // Quest is completed and ready to turn in
    if (QuestController.Instance.IsQuestCompleted(questID))
    {
        marker.ShowExclamationReady();
        return;
    }

    marker.HideMarker();
}

    private void OnQuestProgressUpdated(string updatedQuestID)
{
    // Only update this NPC if the update was for their quest
    if (dialogueData == null || dialogueData.quest == null) return;
    if (dialogueData.quest.questID == updatedQuestID)
    {
        UpdateMarker();
    }
}
    private void OnDestroy()
{
     if (QuestController.Instance != null)
        QuestController.Instance.OnQuestProgressUpdated -= OnQuestProgressUpdated;
}

private void CheckTalkNPCObjectives()
{
    if (dialogueData == null || dialogueData.quest == null)
        return;

    string questID = dialogueData.quest.questID;

    // Only check for active quests
    if (!QuestController.Instance.IsQuestActive(questID))
        return;

    var questProgress = QuestController.Instance.activateQuests
        .Find(q => q.QuestID == questID);

    if (questProgress == null) return;

    // Find all TalkNPC objectives for this quest
    foreach (var objective in questProgress.objectives)
    {
        if (objective.type == Quest.ObjectiveType.TalkNPC &&
            objective.objectiveID == npcID &&
            objective.currentAmount < objective.requiredAmount)
        {
            // Player has talked to the correct NPC
            objective.currentAmount++;

            Debug.Log($"TalkNPC Objective Updated! NPC: {npcID}  ({objective.currentAmount}/{objective.requiredAmount})");

            // Update Quest UI
            QuestController.Instance.questUI.UpdateQuestUI();

            // Notify system of progress change 
            QuestController.Instance.NotifyQuestProgressUpdated(questID);

            return; // Only update one objective per talk
        }
    }
}
}