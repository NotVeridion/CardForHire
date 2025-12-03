using UnityEngine;

public class NPCInteractionRange : MonoBehaviour
{
    private NPCScript npc;

    private void Start()
    {
        npc = GetComponentInParent<NPCScript>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            npc.playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            npc.playerInRange = false;
    }
}