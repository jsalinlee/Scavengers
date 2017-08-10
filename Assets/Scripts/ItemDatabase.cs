using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System;

public class ItemDatabase : MonoBehaviour {
	private List<Item> database = new List<Item>();
	private JsonData itemsData;

	void Start() {
		itemsData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
		ConstructItemDatabase(itemsData);
    }

	public Item FetchItemByID(int id) {
		for (int i = 0; i < database.Count; i++) {
			if (database[i].ID == id) {
                if(database[i].ItemType == ItemType.Weapon) {
                    return database[i] as Weapon;
                } else {
                    return database[i];
                }
			}
		}
		return null;
	}

	void ConstructItemDatabase(JsonData items) {
        foreach (string category in items.Keys) {
            foreach (string subcategory in items[category].Keys) {
                if (category == "weapons") {
                    JsonData weaponCategory = items[category][subcategory];
                    for (int i = 0; i < weaponCategory.Count; i++) {
                        database.Add(new Weapon((int)weaponCategory[i]["id"], weaponCategory[i]["title"].ToString(), (int)weaponCategory[i]["value"], (float)(double)weaponCategory[i]["durability"], weaponCategory[i]["description"].ToString(), (bool)weaponCategory[i]["stackable"], (int)weaponCategory[i]["rarity"], weaponCategory[i]["slug"].ToString(), "melee", (float)(double)weaponCategory[i]["power"], 0, 0, 0));
                    }
                } else if (category =="armor") {
                    JsonData armorCategory = items[category][subcategory];
                    for (int i = 0; i < armorCategory.Count; i++) {
                        
                    }
                }
            }
        }
    }
}

public enum ItemType
{
    Generic,
    Weapon,
    Armor
}

public class Item {
    public int ID { get; set; }
    public ItemType ItemType { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public float Durability { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public int Rarity { get; set; }
    public string Slug { get; set; }
    public Sprite Sprite { get; set; }
    public int Quantity { get; set; }
    public Guid UniqueID { get; set; }

    public Item(int id, string title, int value, float durability, string description, bool stackable, int rarity, string slug) {
        this.ID = id;
        this.ItemType = ItemType.Generic;
        this.Title = title;
        this.Value = value;
        this.Durability = durability;
        this.Description = description;
        this.Stackable = stackable;
        this.Rarity = rarity;
        this.Slug = slug;
        this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + slug);
        this.Quantity = 1;
    }

    public Item() {
        this.ID = -1;
    }

    public Item(Item template) {
        this.ID = template.ID;
        this.ItemType = template.ItemType;
        this.Title = template.Title;
        this.Value = template.Value;
        this.Durability = template.Durability;
        this.Description = template.Description;
        this.Stackable = template.Stackable;
        this.Rarity = template.Rarity;
        this.Slug = template.Slug;
        this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + template.Slug);
        this.Quantity = 1;
        this.UniqueID = template.UniqueID;
    }
}

public class Weapon : Item {
    public string WeaponType { get; set; }
    public float Power { get; set; }
    public int AmmoID { get; set; }
    public int ClipCapacity { get; set; }
    public float ReloadTime { get; set; }

    public Weapon(int id, string title, int value, float durability, string description, bool stackable, int rarity, string slug, string weaponType, float power, int ammoID, int clipCapacity, float reloadTime): base(id, title, value, durability, description, stackable, rarity, slug) {
        this.ItemType = ItemType.Weapon;
        this.WeaponType = weaponType;
        this.Power = power;
        if (this.WeaponType == "melee" || this.WeaponType == "special") {
            this.AmmoID = -1;
            this.ClipCapacity = -1;
            this.ReloadTime = -1;
        } else {
            this.AmmoID = ammoID;
            this.ClipCapacity = clipCapacity;
            this.ReloadTime = reloadTime;
        }
    }

    public Weapon(Weapon template) {
        this.ID = template.ID;
        this.ItemType = template.ItemType;
        this.Title = template.Title;
        this.Value = template.Value;
        this.Durability = template.Durability;
        this.Description = template.Description;
        this.Stackable = template.Stackable;
        this.Rarity = template.Rarity;
        this.Slug = template.Slug;
        this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + template.Slug);
        this.Quantity = 1;
        this.UniqueID = template.UniqueID;
        this.WeaponType = template.WeaponType;
        if (template.WeaponType == "melee" || template.WeaponType == "special")
        {
            this.AmmoID = -1;
            this.ClipCapacity = -1;
            this.ReloadTime = -1;
        }
        else
        {
            this.AmmoID = template.AmmoID;
            this.ClipCapacity = template.ClipCapacity;
            this.ReloadTime = template.ReloadTime;
        }
    }
}

public class Armor : Item {
    
}