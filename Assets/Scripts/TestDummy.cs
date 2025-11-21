using TMPro;
using UnityEngine;

public class TestDummy : MonoBehaviour
{
    private TextMeshPro dmgText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dmgText = GetComponentInChildren<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float dmg)
    {
        dmgText.text = "Damage: " + dmg.ToString();;
    }
}
