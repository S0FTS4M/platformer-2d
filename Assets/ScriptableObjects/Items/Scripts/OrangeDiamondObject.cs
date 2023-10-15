using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewOrangeDiamondObject", menuName = "InventorySystem/Items/OrangeDiamond")]
public class OrangeDiamondObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.OrangeDiamond;
    }
}
