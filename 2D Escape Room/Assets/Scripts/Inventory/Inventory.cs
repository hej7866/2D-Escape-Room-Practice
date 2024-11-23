using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    ItemDatabase itemDatabase;
    public List<ItemStruct> inventory = new List<ItemStruct>(); // public으로 변경하여 디버깅 시 확인 가능

    private Item itemInRange; // 상호작용 가능한 아이템
    private bool isPlayerInRange = false;

    // UI 관련 변수들
    public GameObject inventoryUIPanel; // 인벤토리 UI 패널
    public Transform inventoryGrid; // 아이템 슬롯들이 배치될 인벤토리 그리드
    public GameObject inventorySlotPrefab; // 아이템 슬롯 프리팹
    private bool isInventoryOpen = false; // 인벤토리 창 열림 여부

    // 아이템 이름 및 설명 UI
    public Text itemNameText; // 아이템 이름을 표시할 UI Text
    public Text itemDescriptionText; // 아이템 설명을 표시할 UI Text

    void Start()
    {
        // 게임 내에 있는 ItemDatabase 컴포넌트를 찾습니다.
        itemDatabase = FindObjectOfType<ItemDatabase>();
    }

    void Update()
    {
        // 플레이어가 아이템의 범위 안에 있고, 'E' 키를 눌렀을 때 아이템 획득
        if (isPlayerInRange && itemInRange != null && Input.GetKeyDown(KeyCode.E))
        {
            CollectItem();
        }

        // 'I' 키를 눌러 인벤토리 UI 열고 닫기
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryUI();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 GameObject의 태그가 "Item"인지 확인
        if (other.CompareTag("Item"))
        {
            Item itemComponent = other.GetComponent<Item>();
            if (itemComponent != null)
            {
                itemInRange = itemComponent;
                isPlayerInRange = true;
                Debug.Log($"아이템 {itemInRange.itemID}와 상호작용 가능: 'E' 키를 눌러 획득하세요.");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // 플레이어가 아이템 범위를 벗어났을 때
        if (other.CompareTag("Item") && itemInRange != null && other.GetComponent<Item>() == itemInRange)
        {
            itemInRange = null;
            isPlayerInRange = false;
            Debug.Log("아이템 범위를 벗어났습니다.");
        }
    }

    void CollectItem()
    {
        // ItemDatabase에서 해당 ID에 해당하는 아이템을 가져옴
        ItemStruct item = itemDatabase.GetItemByID(itemInRange.itemID);
        if (item != null)
        {
            // 인벤토리에 아이템 추가
            inventory.Add(item);
            Debug.Log($"{item.GetName()}을(를) 인벤토리에 추가했습니다.");

            // 인벤토리 UI 업데이트
            if (isInventoryOpen)
            {
                UpdateInventoryUI();
            }

            // 아이템 획득 후 해당 GameObject 제거
            Destroy(itemInRange.gameObject);

            // 상태 초기화
            itemInRange = null;
            isPlayerInRange = false;
        }
        else
        {
            Debug.LogWarning($"ID {itemInRange.itemID}에 해당하는 아이템을 데이터베이스에서 찾을 수 없습니다.");
        }
    }

    void ToggleInventoryUI()
    {
        if (inventoryUIPanel != null)
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryUIPanel.SetActive(isInventoryOpen);
            if (isInventoryOpen)
            {
                UpdateInventoryUI();
            }
        }
    }

    public void UpdateInventoryUI()
    {
        // 기존에 있던 아이템 슬롯을 모두 삭제합니다.
        foreach (Transform child in inventoryGrid)
        {
            Destroy(child.gameObject);
        }

        // 인벤토리에 있는 모든 아이템에 대해 새로운 슬롯을 생성합니다.
        foreach (ItemStruct item in inventory)
        {
            GameObject newSlot = Instantiate(inventorySlotPrefab, inventoryGrid);
            InventorySlot slotComponent = newSlot.GetComponent<InventorySlot>();

            if (slotComponent != null)
            {
                slotComponent.item = item;
                slotComponent.itemNameText = itemNameText; // 아이템 이름 텍스트 연결
                slotComponent.itemDescriptionText = itemDescriptionText; // 아이템 설명 텍스트 연결
                slotComponent.inventory = this; // Inventory 참조 전달
            }

            Image itemIcon = newSlot.GetComponentInChildren<Image>();

            if (itemIcon != null && item.GetIcon() != null)
            {
                itemIcon.sprite = item.GetIcon();
            }
        }
    }

    // 아이템을 인벤토리에서 제거하는 메서드
    public void RemoveItemFromInventory(ItemStruct item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            Debug.Log($"{item.GetName()}을(를) 인벤토리에서 제거했습니다.");

            // 인벤토리 UI 업데이트
            if (isInventoryOpen)
            {
                UpdateInventoryUI();
            }
        }
        else
        {
            Debug.LogWarning($"{item.GetName()}은(는) 인벤토리에 존재하지 않습니다.");
        }
    }
}
