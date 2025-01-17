﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftResultSlot : MonoBehaviour
{

    CraftSystem craftSystem;
    public int temp = 0;
    GameObject itemGameObject;
    //Inventory inventory;


    // Use this for initialization
    void Start()
    {
        //inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        craftSystem = transform.parent.GetComponent<CraftSystem>();

        itemGameObject = (GameObject)Instantiate(Resources.Load("minsub/Prefabs/Item") as GameObject);
        itemGameObject.transform.SetParent(this.gameObject.transform);
        itemGameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
        itemGameObject.GetComponent<DragItem>().enabled = false;
        itemGameObject.SetActive(false);
        itemGameObject.transform.GetChild(1).GetComponent<Text>().enabled = true;
        //Debug.Log(GameObject.FindGameObjectWithTag("MainInventory").GetComponent<Inventory>().positionNumberX);
        //Debug.Log(GameObject.FindGameObjectWithTag("MainInventory").GetComponent<Inventory>().positionNumberY);
        //itemGameObject.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector2(GameObject.FindGameObjectWithTag("MainInventory").GetComponent<Inventory>().positionNumberX, GameObject.FindGameObjectWithTag("MainInventory").GetComponent<Inventory>().positionNumberY);

    }

    // 프레임당 한번
    void Update()
    {
        if (craftSystem.possibleItems.Count != 0)
        {
            itemGameObject.GetComponent<ItemOnObject>().item = craftSystem.possibleItems[temp];
            itemGameObject.SetActive(true);
        }
        else
            itemGameObject.SetActive(false);

    }


}
