using UnityEngine;
using System.Collections;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;

public class NPCScript : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    private DialogueController dialogueUI;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private enum QuestState { NotStarted, InProgress, Completed }
    private QuestState questState = QuestState.NotStarted;
    private void Start()
    {
        dialogueUI = DialogueController.Instance;
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
            Debug.Log("skip typing");
            }
            else NextLine();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isDialogueActive)
            StartDialogue();
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
        else
            Debug.Log("Quest State Not Found!");
        isDialogueActive = true;
        dialogueUI.SetNPCInfo(dialogueData.npcName);
        dialogueUI.ShowDialogueUI(true);
        //PauseController.SetPause(true);
        DisplayCurrentLine();
    }

    private void SyncQuestState()
    {
        if (dialogueData.quest == null)
            return;
        string questID = dialogueData.quest.questID;
        if (QuestScript.Instance.IsQuestActive(questID))
        {
            questState = QuestState.InProgress;
        }
        else
        {
            questState = QuestState.NotStarted;
        }
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
        if (givesQuest && QuestScript.Instance.HasQuestInProgress)
        {
            Debug.Log("Player already has a quest in progress — cannot take another.");
            dialogueUI.SetDialogueText("You must finish your current quest first!");
            return;
        }
        else if (givesQuest)
        {
            QuestScript.Instance.AcceptQuest(dialogueData.quest);
            questState = QuestState.InProgress;
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
}