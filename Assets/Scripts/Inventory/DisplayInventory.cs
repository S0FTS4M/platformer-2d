using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
 
    void Start()
    {
        InitDisplay();
    }

    public void InitDisplay()
    {
        foreach (var item in inventory.Container)
        {
            var obj = InstantiateItem(item);
            itemsDisplayed.Add(item, obj);
        }
    }
    public void UpdateDisplay(InventorySlot inventorySlot)
    {
        itemsDisplayed[inventorySlot].GetComponentInChildren<TextMeshProUGUI>().text = inventorySlot.amount.ToString();
    }
    public void CreateDisplay(InventorySlot inventorySlot)
    {
        var obj = InstantiateItem(inventorySlot);
        itemsDisplayed.Add(inventorySlot, obj);
    }
    GameObject InstantiateItem(InventorySlot item) 
    {
        var obj = Instantiate(item.item.prefab, Vector3.zero, Quaternion.identity, transform);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = item.amount.ToString();
        return obj;
    }

    public void OnEnable()
    {
        inventory.ItemAdded += Inventory_ItemAdded;
        inventory.ItemUpdated += Inventory_ItemUpdated;
    }
    public void OnDisable()
    {
        inventory.ItemAdded -= Inventory_ItemAdded;
        inventory.ItemUpdated -= Inventory_ItemUpdated;
    }

    private void Inventory_ItemUpdated(InventorySlot inventorySlot)
    {
        UpdateDisplay(inventorySlot);
    }

    private void Inventory_ItemAdded(InventorySlot inventorySlot)
    {
        CreateDisplay(inventorySlot);
    }
}
