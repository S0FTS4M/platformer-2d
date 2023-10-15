using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBlueDiamondObject", menuName = "InventorySystem/Items/BlueDiamond")]
public class BlueDiamondObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.BlueDiamond;
    }
}
