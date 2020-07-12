using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterDrop : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            PlayerManager.current.GainLetter((Letter)Enum.Parse(typeof(Letter), transform.parent.gameObject.name), gameObject);
        }
    }
}
