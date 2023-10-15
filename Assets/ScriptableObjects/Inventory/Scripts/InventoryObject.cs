using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName ="NewInventory", menuName ="InventorySystem/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();

    public delegate void ItemDelegate(InventorySlot item);
    public event ItemDelegate ItemAdded;
    public event ItemDelegate ItemRemoved;
    public event ItemDelegate ItemUpdated;
    public void AddItem(ItemObject _item, int _amount)
    {
        foreach (var inventorySlot in Container)
        {
            if (inventorySlot.item == _item) 
            {
                inventorySlot.AddAmount(_amount);
                ItemUpdated?.Invoke(inventorySlot);
                return;
            }
        }
        var newSlot = new InventorySlot(_item, _amount);
        Container.Add(newSlot);
        ItemAdded?.Invoke(newSlot);
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;
    public InventorySlot(ItemObject _item, int _amount) 
    {  
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value) 
    {
        amount += value;
    }
}