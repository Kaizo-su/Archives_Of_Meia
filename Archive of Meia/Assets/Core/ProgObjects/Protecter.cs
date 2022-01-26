using UnityEngine;
public class Protecter
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public int PV { get; set; }
    public int PM { get; set; }
    public Sprite Sprite { get; set; }
    public int Qt { get; set; }

    public Protecter(string name, string description, int pv, int pm, int price, Sprite sprite)
    {
        Name = name;
        Description = description;
        Price = price;
        PV = pv;
        PM = pm;
        Sprite = sprite;
        Qt = 0;
    }

}