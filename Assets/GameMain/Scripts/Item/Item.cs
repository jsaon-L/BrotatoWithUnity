using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Brotato/Item")]
public class Item : ScriptableObject
{

    public enum Tier { COMMON, UNCOMMON, RARE, LEGENDARY, DANGER_4, DANGER_5 }

    public string ItemId;
    public bool Unlocked_by_default = false;
    public Sprite Icon;
    public string ItemName;
    public Tier ItemTier;
    public int Value;
    public string tracking_text;

    public List<Buff> Buffs;
}
