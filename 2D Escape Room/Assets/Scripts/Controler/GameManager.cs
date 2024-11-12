using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public DialogueManager dialogueManager;
    public GameObject scanObject;
    private Dictionary<int, int> npcDialogueIndexes = new Dictionary<int, int>();
    public Text npcName;
    public Text talkText;
    public Animator talkPanel;
    public GameObject menuSet;
    public GameObject player;
    public bool isAction = false;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GameLoad();
    }

    void Update()
    {
        // sub menu
        if(Input.GetButtonDown("Cancel"))
        {
            if(menuSet.activeSelf) menuSet.SetActive(false);
            else 
            {
                menuSet.SetActive(true);
            }
        }
    }
    
    // Scan Object + Message System
    public void ScanForNPC(GameObject scanObj)
    {
        scanObject = scanObj;
        NPC npc = scanObject.GetComponent<NPC>();
        if (npc != null && Input.GetButtonDown("Jump"))
        {
            if (!npcDialogueIndexes.ContainsKey(npc.npcID))
            {
                npcDialogueIndexes[npc.npcID] = 0; // 초기화
            }
            StartDialogue(npc.npcID, npcDialogueIndexes[npc.npcID]);
        }
    }

    public void StartDialogue(int npcID, int index)
    {
        isAction = true;
        talkPanel.SetBool("isShow", isAction);
          // 인덱스를 확인하여 대화 시작
        if (!npcDialogueIndexes.ContainsKey(npcID))
        {
            npcDialogueIndexes[npcID] = 0; // 인덱스 초기화
        }

        NPCDialogue dialogue = dialogueManager.GetDialogueByIDAndIndex(npcID, index);
        if (dialogue != null)
        {
            Debug.Log(dialogue.npcName + ": " + dialogue.Text);
            npcName.text = dialogue.npcName;
            talkText.text = dialogue.Text;
            npcDialogueIndexes[npcID]++; // 다음 대사로 이동
        }
        else
        {
            npcDialogueIndexes[npcID] = 0; // 대화가 끝났을 때 0번째 인덱스로 초기화
            isAction = false;
            talkPanel.SetBool("isShow", isAction);
            // StartDialogue(npcID, npcDialogueIndexes[npcID]); // 다시 0번째 대사부터 시작
        }
    }

    public int GetCurrentDialogueIndex(int npcID)
    {
        if (!npcDialogueIndexes.ContainsKey(npcID))
        {
            npcDialogueIndexes[npcID] = 0; // NPC가 처음 접근할 때 초기화
        }
        return npcDialogueIndexes[npcID];
    }


    // 게임세이브 함수
    public void GameSave()
    {
        PlayerPrefs.SetFloat("PlayerX",player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY",player.transform.position.y);
        PlayerPrefs.Save();

        menuSet.SetActive(false);
    }

    public void GameLoad()
    {
        if(!PlayerPrefs.HasKey("PlayerX")) return;

        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        
        player.transform.position = new Vector3(x, y, 0);
    }

    // 게임종료 함수
    public void GameExit()
    {
        Application.Quit();
    }
}

