using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ItemStruct item; // 이 슬롯이 가지고 있는 아이템 데이터
    public Text itemNameText; // 아이템 이름을 표시할 Text
    public Text itemDescriptionText; // 아이템 설명을 표시할 Text
    public GameObject contextMenuPrefab; // 컨텍스트 메뉴 프리팹
    private static GameObject contextMenuInstance; // 생성된 컨텍스트 메뉴 인스턴스 (싱글톤으로 유지)
    private Transform canvasTransform;

    public Inventory inventory; // Inventory 참조 추가

    private void Start()
    {
        canvasTransform = GameObject.Find("Canvas")?.transform;
        if (canvasTransform == null)
        {
            Debug.LogError("Canvas라는 이름의 캔버스가 씬에 없습니다.");
        }

        // Inventory 참조는 Inventory.cs에서 설정해줍니다.
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭
        {
            // 클릭한 UI 오브젝트를 가져옴
            GameObject clickedObject = GetClickedGameObject();

            // 클릭한 오브젝트가 null이거나, InventorySlot이나 컨텍스트 메뉴가 아니라면 컨텍스트 메뉴 삭제
            if (clickedObject == null || (clickedObject != contextMenuInstance && clickedObject.GetComponentInParent<ContextMenu>() == null && clickedObject.GetComponent<InventorySlot>() == null))
            {
                if (contextMenuInstance != null)
                {
                    Destroy(contextMenuInstance);
                    contextMenuInstance = null;
                }
            }
        }
    }

    // 클릭한 UI 요소 가져오기
    private GameObject GetClickedGameObject()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);
        return raycastResults.Count > 0 ? raycastResults[0].gameObject : null;
    }

    // 마우스를 슬롯 위로 올렸을 때 호출되는 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            itemNameText.text = item.GetName();
            itemDescriptionText.text = item.GetDescription();
        }
    }

    // 마우스를 슬롯에서 나갔을 때 호출되는 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        itemNameText.text = "";
        itemDescriptionText.text = "";
    }

    // 슬롯을 클릭했을 때 호출되는 함수
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) // 우클릭 감지
        {
            // 이미 컨텍스트 메뉴가 열려 있는 경우 추가 처리하지 않음
            if (contextMenuInstance != null)
            {
                Destroy(contextMenuInstance);
                contextMenuInstance = null;
                return;
            }

            contextMenuInstance = Instantiate(contextMenuPrefab, Input.mousePosition, Quaternion.identity, canvasTransform);

            // 위치 설정
            RectTransform contextMenuRect = contextMenuInstance.GetComponent<RectTransform>();
            Vector2 mousePosition = Input.mousePosition;

            // 메뉴 위치를 마우스 위치에서 약간 오른쪽 위로 조정
            Vector2 menuPosition = mousePosition;
            menuPosition.x += 100f; // 오른쪽으로 이동
            menuPosition.y += 100f;  // 위로 이동

            // 위치를 설정합니다.
            contextMenuRect.position = menuPosition;

            var contextMenu = contextMenuInstance.GetComponent<ContextMenu>();
            if (contextMenu == null)
            {
                Debug.LogError("contextMenuPrefab에 ContextMenu 컴포넌트가 없습니다!");
                return;
            }
            contextMenu.Setup(item, OnUseItem, OnEquipItem, OnDropItem);
        }
    }

    // 아이템 사용 함수
    private void OnUseItem()
    {
        if (item.GetItemType() == ItemStruct.ItemType.Consumable)
        {
            Debug.Log($"{item.GetName()} 아이템을 사용합니다.");

            // 인벤토리에서 아이템 제거
            inventory.RemoveItemFromInventory(item);

            // 슬롯 삭제
            Destroy(gameObject);
        }
        Destroy(contextMenuInstance);
        contextMenuInstance = null;
    }

    // 아이템 장착 함수
    private void OnEquipItem()
    {
        if (item.GetItemType() == ItemStruct.ItemType.Equipment || item.GetItemType() == ItemStruct.ItemType.KeyItem)
        {
            Debug.Log($"{item.GetName()} 아이템을 장착합니다.");

            // 필요에 따라 인벤토리에서 아이템 제거 또는 상태 변경
            // inventory.RemoveItemFromInventory(item);

            // 슬롯 삭제 (필요한 경우)
            // Destroy(gameObject);
        }
        Destroy(contextMenuInstance);
        contextMenuInstance = null;
    }

    // 아이템 버리기 함수
    private void OnDropItem()
    {
        if (item.GetItemType() != ItemStruct.ItemType.KeyItem) // KeyItem은 버릴 수 없음
        {
            Debug.Log($"{item.GetName()} 아이템을 버립니다.");

            // 인벤토리에서 아이템 제거
            inventory.RemoveItemFromInventory(item);

            // 슬롯 삭제
            Destroy(gameObject);
        }
        Destroy(contextMenuInstance);
        contextMenuInstance = null;
    }
}
