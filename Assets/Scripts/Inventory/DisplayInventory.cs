using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class DisplayInventory : MonoBehaviour
{
    [SerializeField]
    private InventoryDisplayContainer inventoryDisplayContainerPrefab;

    public InventoryObject inventory;

    private Dictionary<InventorySlot, InventoryDisplayContainer> _itemsDisplayed = new();

    private Dictionary<string,Sprite> tileDict = new();

    void Start()
    {
        InitSprites();
        InitDisplay();
    }

    private void InitSprites()
    {
        var tiles = Resources.LoadAll<Sprite>("Images/tiles");
        foreach (var tile in tiles)
        {
            tileDict.Add(tile.name, tile);
        }
    }

    public void InitDisplay()
    {
        foreach (var inventorySlot in inventory.Slots)
        {
            var inventoryDisplayContainer = CreateContainer(inventorySlot);
            _itemsDisplayed.Add(inventorySlot, inventoryDisplayContainer);
        }
    }
    public void UpdateDisplay(InventorySlot inventorySlot)
    {
        _itemsDisplayed[inventorySlot].GetComponentInChildren<TextMeshProUGUI>().text = inventorySlot.amount.ToString();
    }
    public void CreateDisplay(InventorySlot inventorySlot)
    {
        var inventoryDisplayContainer = CreateContainer(inventorySlot);
        _itemsDisplayed.Add(inventorySlot, inventoryDisplayContainer);
    }
    InventoryDisplayContainer CreateContainer(InventorySlot inventorySlot)
    {
        var inventoryDisplayContainer = Instantiate(inventoryDisplayContainerPrefab, transform);
        inventoryDisplayContainer.amountText.SetText(inventorySlot.amount.ToString());
        inventoryDisplayContainer.icon.sprite = tileDict[inventorySlot.item.type.ToString()];
        return inventoryDisplayContainer;
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
