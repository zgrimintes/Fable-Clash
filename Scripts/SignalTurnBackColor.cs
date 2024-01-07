using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignalTurnBackColor : MonoBehaviour
{
    public GameObject character;

    public void signal()
    {
        character.GetComponent<CharacterManager>().turnBackColor();
        GetComponentInChildren<TextMeshProUGUI>().fontSize = 3;
    }
}
