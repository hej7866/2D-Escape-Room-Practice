using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    public TextAsset npcDialogueCSVFile; // Unity Editor에서 NPC 대사 CSV 파일을 연결
    public TextAsset frameDialogueCSVFile; // Unity Editor에서 액자 설명 CSV 파일을 연결

    private List<NPCDialogue> npcDialogues = new List<NPCDialogue>(); // NPC 대사 데이터를 저장할 리스트
    private List<FrameDialogue> frameDialogues = new List<FrameDialogue>(); // 액자 대사 데이터를 저장할 리스트

    void Start()
    {
        LoadNPCDialogueFromCSV(npcDialogueCSVFile);
        LoadFrameDialogueFromCSV(frameDialogueCSVFile);
    }

    void LoadNPCDialogueFromCSV(TextAsset csvFile)
    {
        if (csvFile == null)
        {
            Debug.LogError("NPC CSV 파일이 연결되지 않았습니다.");
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
            NPCDialogue dialogue = new NPCDialogue()
            {
                ID = int.TryParse(values[0], out int idValue) ? idValue : 0,
                index = int.TryParse(values[1], out int indexValue) ? indexValue : 0,
                npcName = values[2],
                Text = values[3],
                NextId = values.Length > 4 && int.TryParse(values[4], out int nextIdValue) ? (int?)nextIdValue : null,
                Choice = values.Length > 5 ? values[5].Split('/') : null
            };
            npcDialogues.Add(dialogue);
        }
    }

    void LoadFrameDialogueFromCSV(TextAsset csvFile)
    {
        if (csvFile == null)
        {
            Debug.LogError("액자 설명 CSV 파일이 연결되지 않았습니다.");
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
            FrameDialogue frameDialogue = new FrameDialogue()
            {
                ID = int.TryParse(values[0], out int idValue) ? idValue : 0,
                index = int.TryParse(values[1], out int indexValue) ? indexValue : 0,
                Text = values[2]
            };
            frameDialogues.Add(frameDialogue);
        }
    }

    public List<NPCDialogue> GetDialoguesByNPCID(int npcID)
    {
        return npcDialogues.FindAll(d => d.ID == npcID);
    }

    public NPCDialogue GetDialogueByIDAndIndex(int npcID, int index)
    {
        return npcDialogues.Find(d => d.ID == npcID && d.index == index);
    }

    public FrameDialogue GetFrameDialogueByID(int frameID)
    {
        return frameDialogues.Find(f => f.ID == frameID);
    }
}

public class NPCDialogue
{
    public int ID;         // NPC ID
    public int index;      // 대사 순서 인덱스
    public string npcName; // NPC 이름
    public string Text;    // 대사 내용
    public int? NextId;    // 다음 대사 ID (선택 사항)
    public string[] Choice; // 선택지
}

public class FrameDialogue
{
    public int ID;         // 액자 ID
    public int index;      // 액자 설명 인덱스
    public string Text;    // 액자 설명 내용
}
