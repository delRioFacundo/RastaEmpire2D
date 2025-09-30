using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance;  // Singleton básico

    [SerializeField] TMP_Text promptText;  // Texto del prompt

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Show(string text)
    {
        promptText.text = text;
     
    }

    public void Hide()
    {
       

    }
}
