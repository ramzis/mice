using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static EventManager;

public class Canvas : MonoBehaviour
{
    public GameObject canvas;
    public TextMeshProUGUI header;
    public TextMeshProUGUI para;
    public Image[] stars;
    public Sprite[] starSprites;
    public Button buttonMenu;
    public Button buttonRestart;
    public Button buttonNext;

    public enum StarSprite
    {
        EMPTY,
        HALF,
        FULL
    }

    private void ToggleCanvas(bool active)
    {
        canvas.SetActive(active);
    }

    private void UpdateCanvas(string headerText, string paraText, int starCount)
    {
        header.text = headerText;
        para.text = paraText;
        for (int i = 0; i < stars.Length; i++)
            stars[i].sprite = starSprites[(int)(i < starCount ? StarSprite.FULL : StarSprite.EMPTY)] ;
    }

    private void OnEnable()
    {
        Subscribe(Events.TOGGLE_CANVAS, ToggleCanvasEvent);
        Subscribe(Events.UPDATE_CANVAS, UpdateCanvasEvent);
    }

    private void OnDisable()
    {
        Unsubscribe(Events.TOGGLE_CANVAS, ToggleCanvasEvent);
        Unsubscribe(Events.UPDATE_CANVAS, UpdateCanvasEvent);
    }

    private void UpdateCanvasEvent(dynamic t)
    {
        if (t.Item1 is string && t.Item2 is string && t.Item3 is int)
        {
            UpdateCanvas(t.Item1, t.Item2, t.Item3);
        }
    }

    private void ToggleCanvasEvent(dynamic t)
    {
        if (t is bool)
        {
            ToggleCanvas(t);
        }
    }
}
