﻿using System.Collections;
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
            GameObject.Find("ScreenOverlay/Keys/" + (Letter)i + "/Letter").GetComponent<TMP_Text>().text = ((Letter)i).ToString();
        }
        expendedLetters = new Queue<Letter>();
        cutscene = false;

        TypistManager.current.OnKeyPress += ExpendLetter;
        TypistManager.current.OnSpacePress += ExecuteCommand;

        camObject = GameObject.Find("CameraAnchor");
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

            GameObject.Find("ScreenOverlay/Keys/" + key + "/Number").GetComponent<TMP_Text>().text = (availableLetters[(int)key] == 0) ? "" : availableLetters[(int)key].ToString();
            float c = Mathf.Min(0.75f, (availableLetters[(int)key] + 1) * 0.05f);
            GameObject.Find("ScreenOverlay/Keys/" + key).GetComponent<Image>().color = new Color(c, c, c);

            commandBar.text = commandBar.text + key;
        }
    }

    private void ExecuteCommand ()
    {
        if (body.CheckAction(commandBar.text))
        {
            expendedLetters.Clear();
        }
        else {
            while (expendedLetters.Count > 0)
            {
                Letter key = expendedLetters.Dequeue();
                availableLetters[(int)key] = availableLetters[(int)key] + 1;

                GameObject.Find("ScreenOverlay/Keys/" + key + "/Number").GetComponent<TMP_Text>().text = (availableLetters[(int)key] == 0) ? "" : availableLetters[(int)key].ToString();
                float c = Mathf.Min(0.75f, (availableLetters[(int)key] + 1) * 0.05f);
                GameObject.Find("ScreenOverlay/Keys/" + key).GetComponent<Image>().color = new Color(c, c, c);
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

    public void GainLetter (Letter letter, GameObject pickup)
    {
        availableLetters[(int)letter] = availableLetters[(int)letter] + 1;

        GameObject.Find("ScreenOverlay/Keys/" + letter + "/Number").GetComponent<TMP_Text>().text = (availableLetters[(int)letter] == 0) ? "" : availableLetters[(int)letter].ToString();
        float c = Mathf.Min(0.75f, (availableLetters[(int)letter] + 1) * 0.05f);
        GameObject.Find("ScreenOverlay/Keys/" + letter).GetComponent<Image>().color = new Color(c, c, c);

        GameObject pickupParent = pickup.transform.parent.gameObject;
        Destroy(pickup);
        Destroy(pickupParent);
    }

    public IEnumerator ShakeCamera(float intensity, float duration)
    {
        print("shakin'");
        float maxDuration = duration;
        float t = Time.time;
        GameObject cam = GameObject.Find("CameraAnchor/Camera");

        while (Time.time - t < maxDuration)
        {
            duration = Time.time - t;
            print("we in here boys");
            float angle = Random.Range(0, 2 * Mathf.PI);
            cam.transform.localPosition = new Vector3(intensity * Mathf.Cos(angle) * ((maxDuration - duration) / maxDuration), intensity * Mathf.Sin(angle) * ((maxDuration - duration) / maxDuration), cam.transform.localPosition.z);
            yield return null;
        }

        cam.transform.localPosition = Vector3.zero;
        yield return null;
    }
}
