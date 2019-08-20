using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static EventReceiver;

public class UICanvas : MonoBehaviour
{
    public GameObject canvas;
    public TextMeshProUGUI header;
    public TextMeshProUGUI para;
    public Image[] stars;
    public Sprite[] starSprites;
    public Button buttonMenu;
    public Button buttonRestart;
    public Button buttonNext;
    public TextMeshProUGUI time;
    public Timer timer;

    public enum StarSprite
    {
        EMPTY,
        HALF,
        FULL
    }

    private void Update()
    {
        UpdateTime();
    }

    private void ToggleCanvas(bool active)
    {
        canvas.SetActive(active);
    }

    private void UpdateCanvas((string headerText, string paraText, int starCount) vt)
    {
        header.text = vt.headerText;
        para.text = vt.paraText;
        for (int i = 0; i < stars.Length; i++)
            stars[i].sprite = starSprites[(int)(i < vt.starCount ? StarSprite.FULL : StarSprite.EMPTY)];
    }

    private void UpdateTime()
    {
        if (timer != null)
            this.time.text = timer.GetTime().ToString();
    }

    private void OnEnable()
    {
        Subscribe<bool>(Events.TOGGLE_CANVAS, ToggleCanvas);
        Subscribe<(string headerText, string paraText, int starCount)>(Events.UPDATE_CANVAS, UpdateCanvas);
    }
}
