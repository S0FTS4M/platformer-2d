using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{
    [SerializeField]
    private ItemType type;

    [SerializeField]
    private int count;

    public ItemType Type { get {  return type; } }
    public int Count { get { return count; } set { count = value; } }
}
