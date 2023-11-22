using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : CharacterManager
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        LoadPlayer(fighterManager);
        updateText();
    }

}
