using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrojanHorse: MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################
    [Header("플레이어 번호 (마스터 서버 들어온 순서)")]
    [SerializeField]
    private int playerNumber;

    [Header("타워 카드 고유 ID")]
    [SerializeField]
    private List<int> cardId = new List<int>();

    [Header("타워 장착된 카드 인덱스")]
    [SerializeField]
    private List<int> cardIndex = new List<int>();

    [Header("타워 장착한 카드 명")]
    [SerializeField]
    private List<string> cardName = new List<string>();

    [Header("타워 장착한 프리펩")]
    [SerializeField]
    private List<GameObject> cardPrefab  = new List<GameObject>();

    private ItemOnObject itemOnObject;

    private GameObject EquipmentItemInventory;

    // ----------- 승완이 에게 보낼 추가 정보---------------
    [Header("스킬 카드 고유 ID")]
    [SerializeField]
    private List<int> skillId = new List<int>();

    [Header("스킬 장착된 카드 인덱스")]
    [SerializeField]
    private List<int> skillIndex = new List<int>();

    [Header("스킬 장착한 카드 명")]
    [SerializeField]
    private List<string> skillCName = new List<string>();

    [Header("스킬 공격력")]
    [SerializeField]
    private List<int> skillATK = new List<int>();

    [Header("스킬 사거리")]
    [SerializeField]
    private List<int> skillCrossroad = new List<int>();

    [Header("스킬 쿨타임")]
    [SerializeField]
    private List<int> skillCoolTime = new List<int>();

    // 공격력, 사거리, 쿨타임
    [SerializeField]
    private ItemDataBaseList itemDatabase;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            PlayerTrojanInfo();
        }
    }

    public void PlayerTrojanInfo()
    {
        // PlayerNumber 받기
        playerNumber = GetComponent<PlayerStorage>().playerNumber;

        //ItemOnObject에서 가져온다.

        // 장착 슬롯 오브젝트 가져오기
        EquipmentItemInventory = GameObject.FindGameObjectWithTag("Inventory").transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetChild(1).GetChild(1).gameObject;

        // 하위 오브젝트들을 리스트에 넣는다.
        List<GameObject> TrojanDataList= new List<GameObject>();
        for (int i = 0; i < EquipmentItemInventory.transform.childCount; i++)
        {
            TrojanDataList.Add(EquipmentItemInventory.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < TrojanDataList.Count; i++)
        {
            // count가 1이면 인덱스 저장(아이템이 들어 있다는 것)
            if (TrojanDataList[i].transform.childCount == 1)
            {
                if (i <= 3) // 0, 1, 2, 3
                {
                    skillIndex.Add(i);
                    // 그리고 데이터를 가져옴
                    ItemOnObject itemOnObject = TrojanDataList[i].gameObject.transform.GetChild(0).GetComponent<ItemOnObject>();
                    skillId.Add(itemOnObject.item.itemID);
                    skillCName.Add(itemOnObject.item.itemName);
                }

                // 장착하고 있는 아이템의 고유 ID를 아이템 베이스에서 찾아서 데이터를 불러온다.
                if (i == 0)
                    Debug.Log(itemDatabase.itemList[i + 1].itemAttributes[0].attributeValue); // 10
                                                                                              //skillATK.Add(itemDatabase.itemList[i + 1].itemAttributes[i].attributeValue);
                if (i == 1)
                    //skillCrossroad.Add(itemDatabase.itemList[i + 1].itemAttributes[i].attributeValue);
                    Debug.Log(itemDatabase.itemList[i + 1].itemAttributes[1].attributeValue);
                if (i == 2)
                    //skillCoolTime.Add(itemDatabase.itemList[i + 1].itemAttributes[i].attributeValue);
                    Debug.Log(itemDatabase.itemList[i + 1].itemAttributes[1].attributeValue);

                if (i > 3) // 4, 5, 6, 7
                {
                    cardIndex.Add(i);
                    // 그리고 데이터를 가져옴
                    ItemOnObject itemOnObject = TrojanDataList[i].gameObject.transform.GetChild(0).GetComponent<ItemOnObject>();
                    cardId.Add(itemOnObject.item.itemID);
                    cardName.Add(itemOnObject.item.itemName);
                    cardPrefab.Add(itemOnObject.item.itemModel);
                }
            }
        }
    }
}
