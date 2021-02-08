using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BetScript : MonoBehaviour
{
    public Text gamePriceText;
    public Text winPriceText;
    public TextMeshProUGUI totalJoinedPlayersText;
    public TextMeshProUGUI timeLeftText;
    public Button myJoiningButton;

    
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
    public string totalPlayerJoined;
    public TextMeshProUGUI pubTitle;
    public string myRoomId;

    public bool isJoined = false;

    public string timeLeft;
    
    public int Callingfunction;
    public int hr, mns, secs;

    public void SetTexts()
    {
        string rupeee = FindObjectOfType<InitMenuScript>().rupeeText.text;
        gamePriceText.text = rupeee + gamePrice;
        winPriceText.text = rupeee + winPrice;
        totalJoinedPlayersText.text = "Players Joined: " + totalPlayerJoined;
        //timeLeftText.text = gameDuration;
        if (noOfPlayer == "2")
        {
            pubTitle.text = "1v1 Battle";
        }
        else pubTitle.text = "3 Winners";
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

    public void EnterTable()
    {
        if (!isJoined)
        {
            float tempfloat = float.Parse(gamePrice);
            Debug.LogError("Amount: " + tempfloat);
            if (GameManager.Instance.coinsCount >= tempfloat)
            {
                GameManager.Instance.playfabManager.apiManager.isClickedPubButton = true;
                GameManager.Instance.playfabManager.apiManager.clickedBet = this;
                FindObjectOfType<APIManager>().tablevalue = gameId;
                GameManager.Instance.playfabManager.apiManager.DeductCoins(tempfloat);
            }
        }
        else ReferenceManager.refMngr.ShowError("Already Joined Game", "Error");
    }

    public void StartTable()
    {
        FindObjectOfType<InitMenuScript>().onlineGamePlayButton.onClick.Invoke();
    }

    public void ShowPrizeDIstribution()
    {
        if (noOfPlayer == "2")
        {
            FindObjectOfType<InitMenuScript>().OpenPrizeDistributionPopup(false, gamePrice, noOfPlayer, firstPrize, secondPrize, thirdPrize);
        }
        else
        {
            FindObjectOfType<InitMenuScript>().OpenPrizeDistributionPopup(true, gamePrice, noOfPlayer, firstPrize, secondPrize, thirdPrize);
        }
    }

    private void OnEnable()
    {
        UpdateClock();
    }

    public void UpdateClock()
    {
        if (hr <= 0)
        {
            timeLeftText.text = mns.ToString() + "m:" + secs.ToString() + "s";
        }
        else
        {
            timeLeftText.text = hr.ToString() + "h:" + mns.ToString() + "m:" + secs.ToString() + "s";
        }
        secs--;
        if (secs < 0)
        {
            mns--;
            secs = 59;
            if (mns < 0)
            {
                hr--;
                mns = 59;
            }
        }
        if (hr != 0 || mns != 0 || secs != 0)
        {
            Invoke(nameof(UpdateClock), 1);
        }
        else
        {
            StartTable();
            Debug.LogError("Table Opened");
        }
    }

}
