using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsRegen : MonoBehaviour
{
    [SerializeField] private float timeToGainMana = 15f;
    [SerializeField] private float timeToGainStamina = 3f;
    [SerializeField] private FighterManager fighterManager;
    private float lastManaGain;
    private float lastStaminaGain;
    private int mana;
    private int stamina;


    void Update()
    {
        mana = GetComponent<CharacterManager>().mana;
        stamina = GetComponent<CharacterManager>().stamina;

        if (mana == 4 && stamina == 5) return;

        if (mana != 4)
        {
            if (Time.time - lastManaGain > timeToGainMana)
            {
                lastManaGain = Time.time;
                fighterManager.mana_Gain(gameObject);
                GetComponent<CharacterManager>().updateText();
            }
        }

        if (stamina != 5)
        {
            if (Time.time - lastStaminaGain > timeToGainStamina)
            {
                lastStaminaGain = Time.time;
                fighterManager.stamina_Gain(gameObject);
                GetComponent<CharacterManager>().updateText();
            }
        }
    }
}
