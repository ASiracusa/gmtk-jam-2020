using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypistManager : MonoBehaviour
{
    public static TypistManager current;

    // Start is called before the first frame update
    void Start()
    {
        current = this;

        StartCoroutine(DetectKeyPresses());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DetectKeyPresses()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                OnKeyPress(Keys.Q);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                OnKeyPress(Keys.W);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnKeyPress(Keys.E);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                OnKeyPress(Keys.R);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                OnKeyPress(Keys.T);
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                OnKeyPress(Keys.Y);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                OnKeyPress(Keys.U);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                OnKeyPress(Keys.I);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                OnKeyPress(Keys.O);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                OnKeyPress(Keys.P);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                OnKeyPress(Keys.A);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                OnKeyPress(Keys.S);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                OnKeyPress(Keys.D);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                OnKeyPress(Keys.F);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                OnKeyPress(Keys.G);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                OnKeyPress(Keys.H);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                OnKeyPress(Keys.J);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                OnKeyPress(Keys.K);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                OnKeyPress(Keys.L);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                OnKeyPress(Keys.Z);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                OnKeyPress(Keys.X);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                OnKeyPress(Keys.C);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                OnKeyPress(Keys.V);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                OnKeyPress(Keys.B);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                OnKeyPress(Keys.N);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                OnKeyPress(Keys.M);
            }
            yield return null;
        }
    }

    public event Action<Keys> OnKeyPress;
    public void KeyPress(Keys key)
    {
        OnKeyPress?.Invoke(key);
    }
}
