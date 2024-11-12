using Unity.VisualScripting;
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭
        {
            Debug.Log("마우스 버튼 클릭 감지됨");

            // 클릭한 UI 오브젝트를 가져옴
            GameObject clickedObject = GetClickedGameObject();

            // 클릭한 오브젝트가 null이거나, InventorySlot이나 컨텍스트 메뉴가 아니라면 컨텍스트 메뉴 삭제
            if (clickedObject == null || (clickedObject != contextMenuInstance && clickedObject.GetComponentInParent<ContextMenu>() == null && clickedObject.GetComponent<InventorySlot>() == null))
            {
                if (contextMenuInstance != null)
                {
                    Destroy(contextMenuInstance);
                    contextMenuInstance = null;
                    Debug.Log("컨텍스트 메뉴 삭제됨");
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
            
            contextMenuInstance = Instantiate(contextMenuPrefab, Input.mousePosition, Quaternion.identity, FindObjectOfType<Canvas>().transform);
            Debug.Log("컨텍스트 메뉴 인스턴스 생성됨");

            // 위치 설정 (마우스 위치를 기준으로 왼쪽 하단에 배치)
            RectTransform contextMenuRect = contextMenuInstance.GetComponent<RectTransform>();
            Vector2 mousePosition = Input.mousePosition;

            // 화면 좌표를 월드 좌표로 변환
            Vector2 menuPosition = mousePosition;

            // 메뉴 위치를 마우스 위치에서 약간 왼쪽 하단으로 조정 (예: x 축 -100, y 축 -50)
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
            Destroy(gameObject);
        }
        Destroy(contextMenuInstance);
        contextMenuInstance = null;
    }
}
