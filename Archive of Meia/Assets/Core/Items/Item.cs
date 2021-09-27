using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {
  
    public string Name { get; set; }
    public string Description { get; set; }

    public int Price { get; set; }

    public Sprite Sprite { get; set; }


    public Item(string name, string description, int price, Sprite sprite)
    {
        Name = name;
        Description = description;
        Price = price;
        Sprite = sprite;
    }

}