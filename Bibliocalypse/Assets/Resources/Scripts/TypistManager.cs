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
                OnKeyPress(Letter.Q);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                OnKeyPress(Letter.W);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnKeyPress(Letter.E);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                OnKeyPress(Letter.R);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                OnKeyPress(Letter.T);
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                OnKeyPress(Letter.Y);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                OnKeyPress(Letter.U);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                OnKeyPress(Letter.I);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                OnKeyPress(Letter.O);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                OnKeyPress(Letter.P);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                OnKeyPress(Letter.A);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                OnKeyPress(Letter.S);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                OnKeyPress(Letter.D);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                OnKeyPress(Letter.F);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                OnKeyPress(Letter.G);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                OnKeyPress(Letter.H);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                OnKeyPress(Letter.J);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                OnKeyPress(Letter.K);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                OnKeyPress(Letter.L);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                OnKeyPress(Letter.Z);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                OnKeyPress(Letter.X);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                OnKeyPress(Letter.C);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                OnKeyPress(Letter.V);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                OnKeyPress(Letter.B);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                OnKeyPress(Letter.N);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                OnKeyPress(Letter.M);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnSpacePress();
            }

            yield return null;
        }
    }

    public event Action<Letter> OnKeyPress;
    public void KeyPress(Letter key)
    {
        OnKeyPress?.Invoke(key);
    }

    public event Action OnSpacePress;
    public void SpacePress()
    {
        OnSpacePress?.Invoke();
    }
}
