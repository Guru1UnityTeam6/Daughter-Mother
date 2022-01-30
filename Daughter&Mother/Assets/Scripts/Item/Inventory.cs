using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    /* 변수 */
    // 아이템을 선택 했을 때의 이름
    public Text NameText; 
    // 아이템을 선택 했을 때의 설명 
    public Text DescriptionText; 
    // 선택된 아이템의 번호, 어떤 아이템을 선택했는지?
    private int selectedItem; 

    // 인벤토리 슬롯들
    private InventorySlot[] slots; 
    // 플레이어가 소지한 아이템 리스트 
    private List<Item> inventoryItemList; 
    // Slot의 부모 객체. (그리드 슬롯) 
    // 부모 객체의 자식 객체들 즉, slot. InventorySlot[] 안에 넣음
    public Transform tf; 

    // 인벤토리창
    public GameObject inventoryPanel;
    // 인벤토리창 활성화시 true
    bool activeInventory = false;

    // 중복실행 제한
    bool preventExec;

    /* 함수 */
    // 시작 : 인벤토리창 초기화
    void Start() 
    {
        // 초기화 진행
        inventoryItemList = new List<Item>(); //인벤토리 아이템 리스트 초기화
        slots = tf.GetComponentsInChildren<InventorySlot>(); // 그리드의 자식객체인 slot 프리팹들이 slots에 들어감

        inventoryPanel.SetActive(activeInventory); //인벤토리 UI 활성화 여부 ()

        // TEST : 아이템 획득 한 경우
        inventoryItemList.Add(new Item(1, "사탕", "놀이터 한 가운데에 떨어져 있던 사탕. 딸기맛과 레몬맛이다.", Item.ItemType.Potion));
        inventoryItemList.Add(new Item(2, "장난감방패", "만화영화 핏치피치어벤저스에서 주인공이 사용하는 방패이다.", Item.ItemType.Weapon));
    }

    void Update()
    {

        // 키보드 C를 눌러서 인벤토리 UI를 열고닫는다. 
        if (Input.GetKeyDown(KeyCode.C))
        {
            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);

            // 마우스 왼쪽 버튼으로 slot을 클릭하면 정보를 확인할 수 있다.
            // 더블클릭 혹은 스페이스를 누르면 장착/사용 할 수 있다.  
            // 인벤토리가 활성화 된 경우에
        }
        if (activeInventory)
        {
            ShowItem();

            // 스페이스 혹은 엔터를 떼면
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return))
            {
                // 다시 방향키를 사용할 수 있다. 
                preventExec = false;
            }
        }
    }

    // 아이템 활성화 (invenrotyItemList에 아이템들을 넣어주고, 출력)
    public void ShowItem()
    {
        // 맨 처음 selectedItem은 0번째 
        selectedItem  = 0;
        // 인벤토리 아이템 리스트의 내용을, 인벤토리 슬롯에 추가 
        for(int i = 0; i < inventoryItemList.Count; i++) // 소지한 아이템의 개수만큼
        {
            // 활성화
            slots[i].gameObject.SetActive(true); 
            slots[i].AddItem(inventoryItemList[i]);
        }
        // 선택된 아이템 슬롯의 색상 변경
        SelectedItem();
    }

    // 선택된 아이템 슬롯의 색상 변경
    public void SelectedItem()
    {
        // 선택된 아이템의 이름, 설명 텍스트를 띄움
        NameText.text = inventoryItemList[selectedItem].itemName;
        DescriptionText.text = inventoryItemList[selectedItem].itemDescription;
    }
}
