using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsRegen : MonoBehaviour
{
    [SerializeField] private float timeToGainMana = 10f;
    [SerializeField] private float timeToGainStamina = 3f;
    private float lastManaGain;
    private float lastStaminaGain;
    private int mana;
    private int stamina;

    void Update()
    {
        if (GetComponent<CharacterManager>().fighterManager.isBoss)
        {
            timeToGainMana = 6.5f;
        }

        mana = GetComponent<CharacterManager>().mana;
        stamina = GetComponent<CharacterManager>().stamina;

        //if (mana >= 4 && stamina >= 5) return;

        if (mana < 4)
        {
            if (Time.time - lastManaGain > timeToGainMana)
            {
                lastManaGain = Time.time;
                gameObject.GetComponent<CharacterManager>().fighterManager.mana_Gain(gameObject);
                GetComponent<CharacterManager>().updateText();
            }
        }
        else lastManaGain = Time.time;

        if (stamina < 5)
        {
            if (Time.time - lastStaminaGain > timeToGainStamina)
            {
                lastStaminaGain = Time.time;
                gameObject.GetComponent<CharacterManager>().fighterManager.stamina_Gain(gameObject);
                GetComponent<CharacterManager>().updateText();
            }
        }
        else lastStaminaGain = Time.time;
    }
}
