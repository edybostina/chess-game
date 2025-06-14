using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;


[RequireComponent(typeof(UIInputReciever))]
public class UIButton : Button
{
    private InputReciever reciever;

    protected override void Awake() 
    {
        base.Awake();
        reciever = GetComponent<UIInputReciever>();
        onClick.AddListener(() => reciever.OnInputRecieved());
    }

}
