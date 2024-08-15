using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractable
{
    public void Interact(PlayerController player);

    public void OnHoverEnter();

    public void OnHoverExit();
}
