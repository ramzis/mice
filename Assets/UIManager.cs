﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public GameObject canvas;
    public TextMeshProUGUI header;
    public TextMeshProUGUI para;
    public Image[] stars;
    public Sprite[] starSprites;

    public enum StarSprite
    {
        EMPTY,
        HALF,
        FULL
    }

    public void ToggleCanvas(bool active)
    {
        canvas.SetActive(active);
    }

    public void UpdateCanvas(string headerText, string paraText, int starCount)
    {
        header.text = headerText;
        para.text = paraText;
        for (int i = 0; i < stars.Length; i++)
            stars[i].sprite = starSprites[(int)(i < starCount ? StarSprite.FULL : StarSprite.EMPTY)] ;
    }

    void Start()
    {
        return;
        var header = "Level completed!";
        var para = "You saved 10/10 mice!\nThe mice thank you for your help\nThe next level awaits you!";
        var stars = 4;
        UpdateCanvas(header, para, stars);
        ToggleCanvas(true);
    }

    void Update()
    {
        
    }
}
