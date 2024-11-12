using UnityEngine;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    public Button useButton;
    public Button equipButton;
    public Button dropButton;

    // 아이템 타입에 따라 버튼을 설정하는 함수
    public void Setup(ItemStruct item, System.Action onUse, System.Action onEquip, System.Action onDrop)
    {
        OnDisable();
        
        // 아이템 타입에 따른 버튼 활성화
        if (item.GetItemType() == ItemStruct.ItemType.Consumable)
        {
            useButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(false);
            dropButton.gameObject.SetActive(true);
            useButton.onClick.AddListener(() => onUse());
        }
        else if (item.GetItemType() == ItemStruct.ItemType.Equipment)
        {
            useButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(true);
            dropButton.gameObject.SetActive(true);
            equipButton.onClick.AddListener(() => onEquip());
        }
        else if (item.GetItemType() == ItemStruct.ItemType.KeyItem)
        {
            useButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(true);
            dropButton.gameObject.SetActive(false); // KeyItem은 버릴 수 없음
            equipButton.onClick.AddListener(() => onEquip());
        }

        dropButton.onClick.AddListener(() => onDrop());
    }

    private void OnDisable()
    {
        // 버튼 리스너 초기화 (메모리 누수 방지)
        useButton.onClick.RemoveAllListeners();
        equipButton.onClick.RemoveAllListeners();
        dropButton.onClick.RemoveAllListeners();
    }
}
