using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager current;

    private int[] availableLetters;

    public Rigidbody2D rb;
    private Typist typist;

    // Start is called before the first frame update
    void Start()
    {
        current = this;

        availableLetters = new int[26];
        for (int i = 0; i < 26; i++)
        {
            availableLetters[i] = 3;
        }

        TypistManager.current.OnKeyPress += ExpendKey;

        StartCoroutine(PlayerMovement());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator PlayerMovement()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                rb.velocity = new Vector2(5, rb.velocity.y);
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                rb.velocity = new Vector2(-5, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            yield return null;
        }
    }

    private void ExpendKey (Keys key)
    {
        if (availableLetters[(int)key] <= 0)
        {
            Debug.Log("Don't have " + key);
        }
        else
        {
            availableLetters[(int)key] = availableLetters[(int)key] - 1;
        }
    }
}
