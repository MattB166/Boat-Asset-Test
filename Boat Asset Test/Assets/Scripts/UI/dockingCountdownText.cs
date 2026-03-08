using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class dockingCountdownText : MonoBehaviour
{

    public TextMeshProUGUI timerText;
    private string countdownTextString;

    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        countdownTextString = " ";
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = countdownTextString;
    }

    public void SetCountdownText(Component sender, object data)
    {
        if (data is object[] array)
        {
            data = array[0];
            if (data is float time)
            {
                countdownTextString = string.Format("Docking Time Left : {0:0.00}", time);
            }
        }

    }

    public void ClearText()
    {
        countdownTextString = " ";
    }
}
