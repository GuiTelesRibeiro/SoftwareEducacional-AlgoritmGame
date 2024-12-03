using UnityEngine;

public enum SlotTag { None, Head, Chest, Legs, Feet }
[CreateAssetMenu(menuName = "RPG 2D/Item")]
public class Item : ScriptableObject
{
    public Sprite sprite;
    public int itemID;
    public SlotTag itemTag;
    public string nomeItem;
    public string descricao;

}
