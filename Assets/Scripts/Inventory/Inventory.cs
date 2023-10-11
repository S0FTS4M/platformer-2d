using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
    private List<ItemData> items;

    public List<ItemData> Items { get { return items; } }

    public delegate void ItemDelegate(ItemData item);
    public event ItemDelegate ItemAdded;
    public event ItemDelegate ItemRemoved;
    public event ItemDelegate ItemUpdated;

    public Inventory()
    {
        items = new();
    }
    public Inventory(List<ItemData> items) { this.items = items; }

    public void Add(ItemData item)
    {
        var (existingItem, index) = FindItemByType(item.Type);
        if (existingItem != null)
        {
            existingItem.Count++;
            ItemUpdated?.Invoke(item);
        }
        else
        {
            items.Add(item);
            ItemAdded?.Invoke(item);

        }
    }

    public void Remove(ItemType itemType)
    {
        var (removedItem, index) = FindItemByType(itemType);
        if (removedItem != null)
        {
            removedItem.Count--;
            if (removedItem.Count <= 0)
            {
                items.RemoveAt(index);
                ItemRemoved?.Invoke(removedItem);
            }
            else
            {
                ItemUpdated?.Invoke(removedItem);
            }
        }
    }


    private (ItemData, int) FindItemByType(ItemType itemType)
    {
        var foundItem = items.Where((item) => item.Type == itemType).FirstOrDefault();
        var foundItemIndex = foundItem != null ? items.FindIndex((item) => foundItem.Type == itemType) : -1;
        return (foundItem, foundItemIndex);
    }

}
