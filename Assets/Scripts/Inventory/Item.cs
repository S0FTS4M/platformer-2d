using UnityEngine;

public class Item : MonoBehaviour,IInteractable
{
    [SerializeField]
    private ItemData data;

    public void SetItem(ItemData data)
    {
        this.data = data;
    }

    public void Interact(GameObject interactor)
    {
        var inventoryHolder = interactor.GetComponent<IInventoryHolder>();
        if (inventoryHolder != null)
        {
            inventoryHolder.Inventory.Add(data);
        }

        //TODO: use pooling instead if we are gonna instantiate items in runtime
        Destroy(gameObject);
    }


}
