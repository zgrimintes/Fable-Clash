using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalTurnBackColor : MonoBehaviour
{
    public GameObject character;

    public void signal()
    {
        character.GetComponent<CharacterManager>().turnBackColor();
    }
}
