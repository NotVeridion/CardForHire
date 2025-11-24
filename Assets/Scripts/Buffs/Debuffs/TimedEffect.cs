using UnityEngine;

public class TimedEffect : MonoBehaviour
{
    public float duration;
    public float startTime;
    public float repeatTime;

    [HideInInspector]
    public GameObject target;

    void Start()
    {
        // If repeatTime > 0, then we'll apply the effect multiple times
        if (repeatTime > 0)
        {
            InvokeRepeating("ApplyEffect", startTime, repeatTime);
        }
        else
        {
            Invoke("ApplyEffect", startTime);
        }

        Invoke("EndEffect", duration);
    }

    protected virtual void ApplyEffect () {
	}
	
	protected virtual void EndEffect () {
		CancelInvoke();
		Destroy(gameObject);
	}
}