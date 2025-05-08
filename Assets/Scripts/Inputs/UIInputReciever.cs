using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class UIInputReciever : InputReciever
{
    [SerializeField] private UnityEvent clickEvent;

    public override void OnInputRecieved() 
    {
        foreach (var handler in inputHandlers)
        {
            handler.ProcessInput(Input.mousePosition, gameObject, () => clickEvent.Invoke());
        }
    }
}
