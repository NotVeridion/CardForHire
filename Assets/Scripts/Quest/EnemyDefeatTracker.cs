using UnityEngine;
using System;

public class EnemyDefeatTracker : MonoBehaviour
{
    public static EnemyDefeatTracker Instance;

    public event Action<string> OnEnemyDefeated;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void NotifyEnemyDefeated(string enemyID)
    {
        OnEnemyDefeated?.Invoke(enemyID);
    }
}