using UnityEngine;

public class BossBlock : MonoBehaviour
{

    [SerializeField] int maxQuest;
    [SerializeField] GameObject block;
    QuestController questController;
    CameraScript cameraScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questController = FindAnyObjectByType<QuestController>();
        cameraScript = FindAnyObjectByType<CameraScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(questController.totalQuestsCompleted >= maxQuest && !cameraScript.inFinalBoss)
        {
            block.SetActive(false);
        }
        else
        {
            block.SetActive(true);
        }
    }
}
