using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    private Sprite I_Left;
    private Sprite I_Right;

    private string name;
    private string text;

    public Dialogue(string name, string text, Sprite I_Left, Sprite I_Right)
    {
        this.name = name;
        this.text = text;
        this.I_Left = I_Left;
        this.I_Right = I_Right;
    }

    public string Name { get => name; set => name = value; }
    public string Text { get => text; set => text = value; }
    public Sprite I_Left1 { get => I_Left; set => I_Left = value; }
    public Sprite I_Right1 { get => I_Right; set => I_Right = value; }
}
