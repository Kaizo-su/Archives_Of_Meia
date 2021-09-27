﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Pause : MonoBehaviour
{
    public static int option = 2;

    private Transform[] Panels;


    // Start is called before the first frame update
    void Start()
    {
        Panels = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            Panels[i] = transform.GetChild(i).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("R"))
            SlidePanel(1);
        if (Input.GetButtonDown("L"))
            SlidePanel(-1);
    }

    private void SlidePanel(int p)
    {
        option += Panels.Length + p;
        option %= Panels.Length;

        for(int i = Panels.Length - 1; i >= 0; i--)
            Panels[i].localPosition = new Vector2((i - option) * 1000 ,Panels[i].localPosition.y);
    }
}