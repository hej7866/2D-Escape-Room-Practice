using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemDatabase : MonoBehaviour
{
    public TextAsset csvFile; // Unity Editor에서 CSV 파일을 연결
    private List<ItemStruct> itemList = new List<ItemStruct>(); // 아이템 리스트

    void Start()
    {
        LoadItemsFromCSV();
    }

    void LoadItemsFromCSV()
    {
        if (csvFile == null)
        {
            Debug.LogError("CSV 파일이 연결되지 않았습니다.");
            return;
        }

        StringReader reader = new StringReader(csvFile.text);
        bool firstLine = true;

        while (reader.Peek() > -1)
        {
            var line = reader.ReadLine();
            if (firstLine)
            {
                firstLine = false;
                continue; // 첫 줄은 열 제목이므로 건너뜁니다.
            }

            var values = line.Split(',');

            // 필드 개수 검사 - 모든 필드가 올바르게 있는지 확인
            if (values.Length < 5)
            {
                Debug.LogWarning($"잘못된 데이터 형식입니다. 예상되는 필드 개수는 5개, 현재 필드 개수: {values.Length}. 줄 번호: {line}");
                continue; // 필드 개수가 부족한 경우 이 줄을 건너뜁니다.
            }

            // 데이터 추출 및 유효성 검사
            ItemStruct newItem = new ItemStruct
            (
                id: int.TryParse(values[0], out int idValue) ? idValue : 0,
                name: values.Length > 1 ? values[1] : "Unknown",
                desc: values.Length > 2 ? values[2] : "No description",
                type: (values.Length > 3 && System.Enum.TryParse(values[3], true, out ItemStruct.ItemType typeValue) && System.Enum.IsDefined(typeof(ItemStruct.ItemType), typeValue)) ? typeValue : ItemStruct.ItemType.KeyItem,
                icon: values.Length > 4 ? Resources.Load<Sprite>(values[4]) : null
            );

            // 아이템 타입 유효성 검사 후 경고 메시지 출력
            if (values.Length > 3 && !System.Enum.IsDefined(typeof(ItemStruct.ItemType), newItem.GetItemType()))
            {
                Debug.LogWarning($"잘못된 아이템 타입 형식: '{values[3]}'. 기본값으로 KeyItem을 사용합니다. 줄 번호: {line}");
            }

            // 아이콘 로드 실패 시 경고 메시지 출력
            if (newItem.GetIcon() == null)
            {
                Debug.LogWarning($"아이콘을 로드하지 못했습니다. 경로: {values[4]}. 줄 번호: {line}");
            }

            // 아이템 리스트에 추가
            itemList.Add(newItem);
        }
    }
    public ItemStruct GetItemByID(int id)
    {
        return itemList.Find(item => item.ID == id);
    }
}
