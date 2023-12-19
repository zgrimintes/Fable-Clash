using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OffFinghtManager : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;

    private void Awake()
    {
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        textMeshProUGUI.text = "3";
    }

    private void minusOneCountdown(int n)
    {
        switch (n)
        {
            case 3:
                textMeshProUGUI.text = "2";
                break;
            case 2:
                textMeshProUGUI.text = "1";
                break;
            case 1:
                textMeshProUGUI.text = "Fight!";
                break;
        }
    }

    public void startFight()
    {
        Debug.Log(gameObject);
        GetComponentInParent<Canvas>().gameObject.SetActive(false);
    }
}
