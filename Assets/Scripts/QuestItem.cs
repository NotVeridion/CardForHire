using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public string name;
    public int quantity = 1;
    private TMP_Text quantityText;

    void Awake()
    {
        quantityText = GetComponentInChildren<TMP_Text>(true);
        updateQuantityDisplay();
    }  
    public void updateQuantityDisplay()
    {
        quantityText.text = quantity > 1 ? quantity.ToString(): "";
    }

}
