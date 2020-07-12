using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager current;

    private int[] availableLetters;
    private Queue<Letter> expendedLetters;
    public bool cutscene;
    
    public TMP_Text commandBar;
    private Creature body;

    private Slider healthBar;

    private GameObject camObject;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
        body = transform.gameObject.GetComponent<Creature>();
        body.AssignCreatureType("Letre", null);

        healthBar = GameObject.Find("ScreenOverlay/HealthBar").GetComponent<Slider>();

        availableLetters = new int[26];
        for (int i = 0; i < 26; i++)
        {
            availableLetters[i] = 3;
        }
        expendedLetters = new Queue<Letter>();
        cutscene = false;

        TypistManager.current.OnKeyPress += ExpendLetter;
        TypistManager.current.OnSpacePress += ExecuteCommand;

        camObject = GameObject.Find("Camera");
        StartCoroutine(MoveCamera());
    }

    private void ExpendLetter (Letter key)
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
                Letter key = expendedLetters.Dequeue();
                availableLetters[(int)key] = availableLetters[(int)key] + 1;
            }
        }
        
        commandBar.text = "";
    }

    private IEnumerator MoveCamera()
    {
        while (true)
        {
            if (!cutscene)
            {
                healthBar.value = body.GetHealth();
                camObject.transform.position = Vector3.Lerp(camObject.transform.position, new Vector3(body.transform.position.x, body.transform.position.y, -10), 0.04f);
            }
            yield return null;
        }
    }
}
