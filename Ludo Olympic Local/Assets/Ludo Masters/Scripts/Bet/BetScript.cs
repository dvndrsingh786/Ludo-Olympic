using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BetScript : MonoBehaviour
{
    public Text gamePriceText;
    public Text winPriceText;
    public Text firstPrizeAmountText;
    public Text secondPrizeAmountText;
    public Text thirdPrizeAmountText;
    public Text fourthPrizeAmountText;
    public TextMeshProUGUI totalJoinedPlayersText;
    public TextMeshProUGUI timeLeftText;

    
    public string gameId;
    public string noOfPlayer;
    public string startDate;
    public string startTime;
    public string endDate;
    public string endTime;
    public string gamePrice;
    public string winPrice;
    public string gameDuration;
    public string firstPrize;
    public string secondPrize;
    public string thirdPrize;
    public string fourthPrize;
    public string playerCount;

    public string timeLeft;
    
    public int Callingfunction;
    public int hr, mns, secs;

    public void SetTexts()
    {
        Debug.LogError("9");
        gamePriceText.text = gamePrice;
        winPriceText.text = winPrice;
        totalJoinedPlayersText.text = playerCount;
        Debug.LogError("10");
        timeLeftText.text = gameDuration;
    }

    public void ToggleButtonPower(Toggle theToggle)
    {
        if (theToggle.IsInteractable()) ButtonPOwer();
    }

    public void ButtonPOwer()
    {
        ReferenceManager.refMngr.isOnlineBidSelected = true;
        FindObjectOfType<GameConfigrationController>().ChangeBettingAmount(Callingfunction);
        ReferenceManager.refMngr.ShowOnlineInvestment(GameManager.Instance.currentBetAmount, GameManager.Instance.currentWinningAmount);
    }

    //public void UpdateClock()
    //{
    //    betTime.text = hr.ToString() + ":" + mns.ToString() + ":" + secs.ToString();
    //    secs--;
    //    if (secs < 0)
    //    {
    //        mns--;
    //        secs = 59;
    //        if (mns < 0)
    //        {
    //            hr--;
    //            mns = 59;
    //        }
    //    }
    //    if (hr != 0 || mns != 0 || secs != 0)
    //    {
    //        Invoke(nameof(UpdateClock), 1);
    //    }
    //    else
    //    {
    //        SetAsOpenTable();
    //    }
    //}

}
