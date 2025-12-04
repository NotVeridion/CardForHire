using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossBlock : MonoBehaviour
{

    [SerializeField] int maxQuest;
    [SerializeField] GameObject block;
    QuestController questController;
    CameraScript cameraScript;

    [SerializeField] GameObject info;
    [SerializeField] Image panel;
    [SerializeField] TextMeshProUGUI text;

    bool informed = false;

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
            if(!informed)
            {
                informed = true;
                StartCoroutine(NotifyPlayer());
            }
        }
        else
        {
            block.SetActive(true);
        }
    }

    IEnumerator NotifyPlayer()
    {
        info.SetActive(true);
        float start = 1f;
        yield return new WaitForSeconds(3f);
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, start);
        text.color = new Color(text.color.r, text.color.g, text.color.b, start);
        start = start - .1f;

        yield return new WaitForSeconds(1f);
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, start);
        text.color = new Color(text.color.r, text.color.g, text.color.b, start);
        start = start - .1f;

        yield return new WaitForSeconds(1f);
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, start);
        text.color = new Color(text.color.r, text.color.g, text.color.b, start);
        start = start - .1f;

        yield return new WaitForSeconds(1f);
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, start);
        text.color = new Color(text.color.r, text.color.g, text.color.b, start);
        start = start - .1f;

        yield return new WaitForSeconds(1f);
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, start);
        text.color = new Color(text.color.r, text.color.g, text.color.b, start);
        start = start - .1f;

        info.SetActive(false);
    }

}
