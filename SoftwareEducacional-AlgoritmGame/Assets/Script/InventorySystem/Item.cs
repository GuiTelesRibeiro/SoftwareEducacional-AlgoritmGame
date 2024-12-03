using UnityEngine;

public enum SlotTag { None, ID1, ID2, ID3, ID4 , ID5, ID6, ID7, ID8, ID9}
[CreateAssetMenu(menuName = "RPG 2D/Item")]
public class Item : ScriptableObject
{
    public Sprite sprite;
    public int itemID;
    public SlotTag itemTag;
    public string nomeItem;
    public string descricao;

}
