using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignalTurnBackColor : MonoBehaviour
{
    public GameObject character;

    public void signal()
    {
        if (character == null) return;

        character.GetComponent<CharacterManager>().turnBackColor();
        GetComponentInChildren<TextMeshProUGUI>().fontSize = 3;
    }
}
