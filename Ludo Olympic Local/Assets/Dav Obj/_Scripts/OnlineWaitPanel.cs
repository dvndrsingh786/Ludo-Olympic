using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnlineWaitPanel : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI secondsLeft;
    [SerializeField] TextMeshProUGUI headText;
    [SerializeField] int timeToClose;
    int timeLeft;

    private void OnEnable()
    {
        headText.text = "The Game Will Start In";
        timeLeft = timeToClose;
        secondsLeft.text = timeLeft.ToString() + " seconds";
        ReduceTime();
    }

    void ReduceTime()
    {
        timeLeft--;
        secondsLeft.text = timeLeft.ToString() + " seconds";
        if (timeLeft > 0)
        {
            if(gameObject.activeInHierarchy)
            Invoke(nameof(ReduceTime), 1);
        }
        else
        {
            secondsLeft.text = "";
            headText.text = "Loading. . . . .";
            //gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        timeLeft = timeToClose;
        secondsLeft.text = timeLeft.ToString() + " seconds";
    }
}
