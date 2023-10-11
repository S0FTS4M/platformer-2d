using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemData data;

    public void SetItem(ItemData data)
    {
        this.data = data;
    }
}
