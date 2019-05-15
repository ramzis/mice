using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiceUpdater : MonoBehaviour
{
    public Mouse[] mice;

    void Start()
    {
        mice = FindObjectsOfType<Mouse>();
    }

    void FixedUpdate()
    {
        foreach(Mouse mouse in mice)
        {
            mouse.UpdateState();
        }
        foreach(Mouse mouse in mice)
        {
            mouse.Act();
        }
    }
}
