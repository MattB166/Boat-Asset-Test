using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public TextMeshProUGUI attemptInfoText;
    private GameObject restartButton;
    public GameEvent onRestartClick;

    // Start is called before the first frame update
    void Start()
    {
        restartButton = GetComponentInChildren<Button>().gameObject;
        attemptInfoText = GetComponentInChildren<TextMeshProUGUI>();
        //hide panel and disable buttons until game over
        Image panelImage = GetComponent<Image>();
        if (panelImage != null)
        {
            panelImage.enabled = false;
        }

        if (restartButton != null)
            {
                restartButton.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayGameOver()
    {
      Image panelImage = GetComponent<Image>();
        if (panelImage != null)
        {
            panelImage.enabled = true;
        }

        if (restartButton != null)
        {
            restartButton.SetActive(true);
        }
    }

    public void SetInfoText(Component sender, object data)
    {
        if(data is object[] array)
        {
            data = array[0];
            if(data is float time)
            {
                int minutes = Mathf.FloorToInt(time / 60F);
                int seconds = Mathf.FloorToInt(time - minutes * 60);
                attemptInfoText.text = string.Format("Attempt Time : {0:0}:{1:00}", minutes, seconds);
            }
        }
        else
        {
            attemptInfoText.text = "Attempt Time : N/A. Object is not data[]";
        }
    }

    public void RestartButton()
    {
       onRestartClick.Announce(this);
    }
}
