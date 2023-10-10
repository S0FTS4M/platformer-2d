using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour, IInteractable
{
    public void Interact(GameObject inteactor)
    {
        Destroy(inteactor);
        //TODO: Game is over so start again
    }
}
