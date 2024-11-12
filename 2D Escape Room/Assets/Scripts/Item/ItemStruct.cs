using UnityEngine;

public class ItemStruct
{
    public int ID;              // 아이템 고유 ID - public으로 변경
    private string itemName;    // 아이템 이름
    private string description; // 아이템 설명
    private ItemType itemType;  // 아이템 타입
    private Sprite itemIcon;    // 아이템 이미지

    public enum ItemType
    {
        Consumable,
        Equipment,
        KeyItem
    }

    public ItemStruct(int id, string name, string desc, ItemType type, Sprite icon)
    {
        ID = id;
        itemName = name;
        description = desc;
        itemType = type;
        itemIcon = icon;
    }

    // Getter 메서드들
    public string GetName() { return itemName; }
    public string GetDescription() { return description; }
    public ItemType GetItemType() { return itemType; }
    public Sprite GetIcon() { return itemIcon; }
}
