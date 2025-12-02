using UnityEngine;
using System.Collections;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;

public class NPCScript : MonoBehaviour, IInteractable
{
    [HideInInspector] 
public bool playerInRange = false;
    public NPCDialogue dialogueData;
    
    [Header("NPC Identifier")]
    public string npcID;

    [Header("Quest Role")]
    public bool isQuestGiver = false;
    private QuestMarkerController marker;
    private DialogueController dialogueUI;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private enum QuestState { NotStarted, InProgress, Completed, PostCompleted }
    private QuestState questState = QuestState.NotStarted;
    private PlayerScript player;
    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        dialogueUI = DialogueController.Instance;
        marker = GetComponentInChildren<QuestMarkerController>();
        UpdateMarker();

        if (QuestController.Instance != null)
            QuestController.Instance.OnQuestProgressUpdated += OnQuestProgressUpdated;
        dialogueUI.OnDialogueClosed += HandleDialogueClosed;
    }
    void Update()
{
    if (Input.GetKeyDown(KeyCode.Space) && isDialogueActive)
        {
            if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else
        {
            NextLine();
        }
        }

}
   private void OnMouseDown() // For testing, should be changed to button interact using player
{
    // If player clicks the NPC while dialogue is active
    if (isDialogueActive)
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
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
        if (IsBlockedByActiveQuest())
{
    dialogueUI.SetNPCInfo(dialogueData.npcName);
    dialogueUI.ShowDialogueUI(true);
    player.isMovementLocked = true;
    isDialogueActive = true;

    dialogueUI.SetDialogueText("(Complete your current quest first before you can accept another!)");
    return;
}
        player.isMovementLocked = true;
        SyncQuestState();
        dialogueUI.ClearChoices();
        CheckTalkNPCObjectives();
    if (questState == QuestState.Completed)
    {
         if (isQuestGiver)
    {
        QuestController.Instance.HandInQuest(dialogueData.quest.questID);
        Debug.Log("Quest handed in by quest giver NPC!");
        marker.HideMarker();
    }
    else
    {
        Debug.Log("Quest is completed but must be turned in to the original quest giver.");
        
    }
        
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

    if (QuestController.Instance.IsQuestHandedIn(questID))
    {
        questState = QuestState.PostCompleted;
        return;
    }

    //Quest is active
    if (QuestController.Instance.IsQuestActive(questID))
    {
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

    //Quest not started
    questState = QuestState.NotStarted;
    }

    void DisplayChoices(DialogueChoice choice)
    {
          dialogueUI.ClearChoices();

    if (!isQuestGiver)
    {

        return;
    }


    for (int i = 0; i < choice.choices.Length; i++)
    {
        string choiceText = choice.choices[i];
        int nextIndex = choice.nextDialogueIndexes[i];
        bool givesQuest = choice.givesQuest[i];


        if (givesQuest)
        {
            
            if (dialogueIndex != dialogueData.questStartDialogueIndex)
                continue;

           
            if (QuestController.Instance.HasQuestInProgress)
                continue;
        }


        dialogueUI.CreateChoiceButton(choiceText, () => ChooseOption(nextIndex, givesQuest));
    }
    }
    void ChooseOption(int nextIndex, bool givesQuest)
    {
    if (givesQuest && QuestController.Instance.HasQuestInProgress)
    {
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
        player.isMovementLocked = false;
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
    }
    public void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
            
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
    dialogueUI.OnDialogueClosed -= HandleDialogueClosed;
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
            
            objective.currentAmount++;

            Debug.Log($"TalkNPC Objective Updated! NPC: {npcID}  ({objective.currentAmount}/{objective.requiredAmount})");

            
            QuestController.Instance.questUI.UpdateQuestUI();

            
            QuestController.Instance.NotifyQuestProgressUpdated(questID);

            return;
        }
    }
}
    private void HandleDialogueClosed()
{
    isDialogueActive = false;
    isTyping = false;
}

private bool IsBlockedByActiveQuest()
{
     if (!QuestController.Instance.HasQuestInProgress)
        return false;
    if (!isQuestGiver)
        return false;
    if (QuestController.Instance.activeQuestGiverID != npcID)
        return true;
    return false;
}
}