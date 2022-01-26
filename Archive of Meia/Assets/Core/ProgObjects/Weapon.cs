using UnityEngine;
public class Weapon
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public int Atk { get; set; }
    public Sprite Sprite { get; set; }
    public int Qt { get; set; }

    public Weapon(string name, string description, int atk, int price, Sprite sprite)
    {
        Name = name;
        Description = description;
        Price = price;
        Atk = atk;
        Sprite = sprite;
        Qt = 0;
    }

}