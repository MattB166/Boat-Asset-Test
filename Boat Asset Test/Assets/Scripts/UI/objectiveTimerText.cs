using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class objectiveTimerText : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private string objectiveTextString; 
    private bool shouldDisplayCompletionText = false;

    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        objectiveTextString = " ";
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = objectiveTextString;
    }

    public void SetTimerText(Component sender, object data)
    {
        if(!shouldDisplayCompletionText)
        {
            if (data is object[] array)
            {
                data = array[0];
                if (data is float time)
                {
                    floatToMins(time);
                }
            }
        }
       
        
    }

    private float[] floatToMins(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        objectiveTextString = string.Format("Time : {0:0}:{1:00}", minutes, seconds);
        return new float[] { minutes, seconds };
    }

    public void DisplayCompletionText(Component sender, object data)
    {
        shouldDisplayCompletionText = true;
        float[] completionTime;
        float mins = 0;
        float secs = 0;
        if (data is object[]array)
        {
            data = array[0];
            if(data is float time)
            {
               completionTime = floatToMins(time);
                mins = completionTime[0];
                secs = completionTime[1];
            }
        }
        objectiveTextString = string.Format("Docking Complete in {0:0}:{1:00}!", mins, secs);
    }
}
