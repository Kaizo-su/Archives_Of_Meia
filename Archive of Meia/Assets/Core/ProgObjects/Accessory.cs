using UnityEngine;
public class Accessory
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Sprite Sprite { get; set; }
    public int Qt { get; set; }

    public Accessory(string name, string description, Sprite sprite)
    {
        Name = name;
        Description = description;
        Sprite = sprite;
        Qt = 0;
    }

}