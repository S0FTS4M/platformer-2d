using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField]
    public ItemObject itemObject;

    public void Interact(GameObject interactor)
    {
        var inventoryHolder = interactor.GetComponent<IInventoryHolder>();
        if (inventoryHolder != null)
        {
            inventoryHolder.Inventory.AddItem(itemObject, 1);
            Destroy(gameObject);
        }
    }
}
