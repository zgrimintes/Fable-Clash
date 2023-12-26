using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoseCharacterManager : MonoBehaviour
{
    GameObject player, enemy;

    public FighterManager[] fighterManagers = new FighterManager[9];

    private void Start()
    {
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");
    }

    public void characterChoosed(int c)
    {
        /* c is the parameter recievd from teh buttons to indicate witch character is selected
         * first of all positive integers represent the player and ther negative pairs represent the enemy
         * 1 - Prislea, 2 - Harap-Alb, 3 - Greuceanul,
         * 4 - Spanul, 5 - Capcaunul, 6 - Zmeul,
         * 7 - Zgripturoaica, 8 - Balaurul, 9 - Crisnicul
         * */

        switch (c)
        {
            case 1:
                player.GetComponent<CharacterManager>().fighterManager = fighterManagers[c - 1];
                return;
            case -1:
                enemy.GetComponent<CharacterManager>().fighterManager = fighterManagers[c - 1];
                return;
            case 6:
                player.GetComponent<CharacterManager>().fighterManager = fighterManagers[c - 1];
                return;
            case -6:
                enemy.GetComponent<CharacterManager>().fighterManager = fighterManagers[c - 1];
                return;
        }
    }
}
