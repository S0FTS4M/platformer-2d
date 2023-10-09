using UnityEngine;

public class Collectable : MonoBehaviour,ICollectable
{
    public void Collect()
    {
        Debug.Log("collected");
        Destroy(gameObject);
    }

}
