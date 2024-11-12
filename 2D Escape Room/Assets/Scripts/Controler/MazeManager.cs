using UnityEngine;
using System.Collections.Generic;

public class MazeManager : MonoBehaviour
{
    public Transform startPosition; // 처음 위치
    public int currentStage = 0; // 현재 문제의 단계
    public int[] correctDoorsPerStage = {0, 2, 1}; // 각 단계에서 정답 문 인덱스를 설정
    public GameObject signs;
    public GameObject portal;
    public GameObject[] doors;

    private int initialSignNpcID; // 표지판의 초기 npcID
    private List<int> initialDoorNpcIDs = new List<int>(); // 문들의 초기 npcID를 저장하는 리스트

    private void Start()
    {
        // 표지판의 초기 npcID 값 저장
        NPC signNpc = signs.GetComponent<NPC>();
        if (signNpc != null)
        {
            initialSignNpcID = signNpc.npcID;
        }

        // 각 문(Door)의 초기 npcID 값을 리스트에 저장
        Door[] doors = FindObjectsOfType<Door>();
        foreach (Door door in doors)
        {
            NPC doorNpc = door.GetComponent<NPC>();
            if (doorNpc != null)
            {
                initialDoorNpcIDs.Add(doorNpc.npcID); // 각 문의 npcID 초기값을 리스트에 추가
            }
        }
    }

    // 현재 단계에서 정답 문을 선택했는지 확인
    public bool CheckCorrectDoor(int selectedDoor)
    {
        NPC npc = signs.GetComponent<NPC>();
        if (selectedDoor == correctDoorsPerStage[currentStage])
        {
            currentStage++; // 다음 단계로 이동
            if(currentStage == correctDoorsPerStage.Length)
            {
                signs.SetActive(false);
                foreach(GameObject door in doors)
                {
                    door.SetActive(false);
                }
                portal.SetActive(true);
            }
            npc.npcID += 10; // 표지판 npcID 증가
            IncreaseNpcIDs(); // 문들의 npcID 증가
            return true;
        }
        else
        {
            ResetStage(); // 오답인 경우 첫 번째 단계로 초기화
            return false;
        }
    }

    // 모든 문에 있는 NPC 스크립트의 npcID 값을 증가시키는 함수
    private void IncreaseNpcIDs()
    {
        Door[] doors = FindObjectsOfType<Door>(); // 모든 Door 오브젝트를 찾음
        foreach (Door door in doors)
        {
            NPC npcScript = door.GetComponent<NPC>(); // Door 오브젝트의 NPC 스크립트를 찾음
            if (npcScript != null)
            {
                npcScript.npcID += 10; // npcID 값을 10씩 증가
            }
        }
    }

    // 첫 번째 단계로 초기화하는 함수
    public void ResetStage()
    {
        currentStage = 0;

        // 표지판 npcID 초기화
        NPC signNpc = signs.GetComponent<NPC>();
        if (signNpc != null)
        {
            signNpc.npcID = initialSignNpcID;
        }

        // 각 문의 npcID 초기화
        Door[] doors = FindObjectsOfType<Door>();
        for (int i = 0; i < doors.Length; i++)
        {
            NPC npcScript = doors[i].GetComponent<NPC>();
            npcScript.npcID = initialDoorNpcIDs[i]; // 초기화된 npcID 값으로 복원
        }
    }
}
