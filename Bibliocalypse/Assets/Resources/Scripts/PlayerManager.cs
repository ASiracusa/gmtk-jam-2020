using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager current;

    private int[] availableLetters;
    private Queue<Keys> expendedLetters;

    public Rigidbody2D rb;
    public TMP_Text commandBar;
    private Creature body;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
        body = gameObject.GetComponent<Creature>();

        availableLetters = new int[26];
        for (int i = 0; i < 26; i++)
        {
            availableLetters[i] = 3;
        }
        expendedLetters = new Queue<Keys>();

        TypistManager.current.OnKeyPress += ExpendLetter;
        TypistManager.current.OnSpacePress += ExecuteCommand;

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

    private void ExpendLetter (Keys key)
    {
        if (availableLetters[(int)key] <= 0)
        {
            Debug.Log("Don't have " + key);
        }
        else
        {
            availableLetters[(int)key] = availableLetters[(int)key] - 1;
            expendedLetters.Enqueue(key);

            commandBar.text = commandBar.text + key;
        }
    }

    private void ExecuteCommand ()
    {
        if (body.CheckAction(commandBar.text))
        {

        }
        else {
            while (expendedLetters.Count > 0)
            {
                Keys key = expendedLetters.Dequeue();
                availableLetters[(int)key] = availableLetters[(int)key] + 1;
            }
        }
        
        commandBar.text = "";
    }
}
