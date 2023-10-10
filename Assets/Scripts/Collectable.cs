using UnityEngine;

public class Collectable : MonoBehaviour,IInteractable
{
    public void Interact(GameObject interactor)
    {
        //TODO: gain points or somethign like that
        Debug.Log("collected");
        Destroy(gameObject);
    }

}
