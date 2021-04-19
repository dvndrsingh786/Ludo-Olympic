using System;
using LitJson;
using System.IO;
using SimpleJSON;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BetDataScript : MonoBehaviour
{

    [Header("Bet Attribute")]

    string betURL;

    public GameObject onlinebetdataPrefab;
    public Transform betdataPublic;
    //public Transform betdataScheduled;
    public GameObject betscrollPanel;

    [Header("PivateBet Attribute")]

    public GameObject privatebetdataPrefab;
    public Transform privatebetdataPublic;
    public GameObject privatebetscrollPanel;

    public bool isTwoplayer = false;

    bool oneWinnerFilter = false;
    bool threeWinnerFilter = false;
    bool allWinnerFilter = false;

    public enum FilterEnum
    {
        allWinnerFilter=0,
        oneWinnerFilter=1,
        threeWinnerFilter=3,
    }

    public void ApplyFilter(int filterType)
    {
        Debug.LogError("Applying filter: " + filterType);
        bool filterBool = true;
        if (filterType == (int)FilterEnum.oneWinnerFilter)
        {
            oneWinnerFilter = true;
            for (int i = 0; i < betdataPublic.childCount; i++)
            {
                if (!betdataPublic.GetChild(i).gameObject.name.Contains("Ad"))
                {
                    if (betdataPublic.GetChild(i).GetComponent<BetScript>().noOfPlayer != "2")
                    {
                        betdataPublic.GetChild(i).gameObject.SetActive(false);
                    }
                    else
                    {
                        betdataPublic.GetChild(i).gameObject.SetActive(true);
                    }
                }
            }
        }
        else if(filterType == (int)FilterEnum.threeWinnerFilter)
        {
            threeWinnerFilter = true;
            for (int i = 0; i < betdataPublic.childCount; i++)
            {
                if (!betdataPublic.GetChild(i).gameObject.name.Contains("Ad"))
                {
                    if (betdataPublic.GetChild(i).GetComponent<BetScript>().noOfPlayer != "4")
                    {
                        betdataPublic.GetChild(i).gameObject.SetActive(false);
                    }
                    else
                    {
                        betdataPublic.GetChild(i).gameObject.SetActive(true);
                    }
                }
            }
        }
        else if (filterType == (int)FilterEnum.allWinnerFilter)
        {
            for (int i = 0; i < betdataPublic.childCount; i++)
            {
                if (!betdataPublic.GetChild(i).gameObject.name.Contains("Ad"))
                {
                    betdataPublic.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        betURL = GameManager.apiBase1 + "betting";
        Debug.Log("BEt data script: " + gameObject.name);
        StartCoroutine(GetBetting());
        //StartCoroutine(privateBetting());
    }
    public void OnPlan(bool istwo)
    {
        //for (int i = 0; i < betdataPublic.childCount; i++)
        //{
        //    Debug.LogError("Destroying");
        //    Destroy(betdataPublic.GetChild(i).gameObject);
        //}
        //for (int i = 0; i < betdataScheduled.childCount; i++)
        //{
        //    Destroy(betdataScheduled.GetChild(i).gameObject);
        //}
        isTwoplayer = istwo;
        //StartCoroutine(GetBetting());
    }

    public void TestCallBetting()
    {
        for (int i = 0; i < betdataPublic.childCount; i++)
        {
            if (!betdataPublic.GetChild(i).gameObject.name.Contains("Ad"))
            {
                Destroy(betdataPublic.GetChild(i).gameObject);
            }
        }
        StartCoroutine(GetBetting());
    }

    bool finishedFirstCall = true;

    IEnumerator GetBetting()
    {
        if (finishedFirstCall)
        {
            finishedFirstCall = false;
            for (int i = 0; i < betdataPublic.childCount; i++)
            {
                if (!betdataPublic.GetChild(i).gameObject.name.Contains("Ad"))
                {
                    Destroy(betdataPublic.GetChild(i).gameObject);
                }
            }
            string nowTime, nowDate;
            using (WWW www = new WWW(betURL))
            {
                yield return www;
                int twoplayerac = 0;
                nowTime = ReferenceManager.refMngr.GetTime();
                nowDate = ReferenceManager.refMngr.GetDate();
                JsonData jsonvale = JsonMapper.ToObject(www.text);
                string gameId, noOfPlayer, startDate, startTime, endDate, endTime, gamePrice, winPrice, gameDuration,
                firstPrize, secondPrize, thirdPrize, fourthPrize, playerCount;
                for (int i = 0; i < jsonvale["result_push"].Count; i++)
                {
                    gameId = jsonvale["result_push"][i]["game_id"].ToString();
                    noOfPlayer = jsonvale["result_push"][i]["no_of_player"].ToString();
                    startDate = jsonvale["result_push"][i]["game_start_date"].ToString();
                    startTime = jsonvale["result_push"][i]["game_start_time"].ToString();
                    endDate = jsonvale["result_push"][i]["game_end_date"].ToString();
                    endTime = jsonvale["result_push"][i]["game_end_time"].ToString();
                    gamePrice = jsonvale["result_push"][i]["game_price "].ToString();
                    winPrice = jsonvale["result_push"][i]["win_price"].ToString();
                    gameDuration = jsonvale["result_push"][i]["game_duration"].ToString();
                    firstPrize = jsonvale["result_push"][i]["first_prize"].ToString();
                    secondPrize = jsonvale["result_push"][i]["second_prize"].ToString();
                    thirdPrize = jsonvale["result_push"][i]["third_prize"].ToString();
                    fourthPrize = jsonvale["result_push"][i]["fourth_prize"].ToString();
                    playerCount = jsonvale["result_push"][i]["public_table_count"].ToString();
                    int durationToAddddd = ReferenceManager.refMngr.timeToSecondsMnsScs(gameDuration, ':');
                    BetScript betScript;
                    if (true)
                    {
                        if (true)
                        {
                            //if (ReferenceManager.refMngr.CheckDate(date, nowDate))
                            //{
                            //    if (ReferenceManager.refMngr.CheckTime(startTime, endTime, nowTime))
                            //    {
                            bool isSameDate, isRightTime, isLessThanADay;
                            string bidTime = "";
                            isSameDate = ReferenceManager.refMngr.CheckDate(startDate, endDate, nowDate);
                            isRightTime = ReferenceManager.refMngr.CheckTime(startTime, endTime, nowTime);
                            isLessThanADay = ReferenceManager.refMngr.IsLessThanADay(startDate, nowDate, nowTime, startTime, durationToAddddd.ToString());
                            //if (isSameDate && isRightTime)
                            //{
                            if (isLessThanADay)
                            {
                                betScript = Instantiate(onlinebetdataPrefab, betdataPublic).GetComponent<BetScript>();
                                betdataPublic.GetComponent<RectTransform>().sizeDelta = new Vector2(betdataPublic.GetComponent<RectTransform>().sizeDelta.x, betdataPublic.childCount * 325);
                                //}
                                //else if (isSameDate)
                                //{
                                //    betScript = Instantiate(onlinebetdataPrefab, betdataScheduled).GetComponent<BetScript>();
                                //}
                                //else betScript = null;
                                if (betScript != null)
                                {
                                    betScript.gameId = gameId;
                                    betScript.noOfPlayer = noOfPlayer;
                                    betScript.startDate = startDate;
                                    betScript.endDate = endDate;
                                    betScript.startTime = startTime;
                                    betScript.endTime = endTime;
                                    betScript.gamePrice = gamePrice;
                                    betScript.winPrice = winPrice;
                                    betScript.gameDuration = gameDuration;
                                    betScript.firstPrize = firstPrize;
                                    betScript.secondPrize = secondPrize;
                                    betScript.thirdPrize = thirdPrize;
                                    betScript.fourthPrize = fourthPrize;
                                    betScript.totalPlayerJoined = playerCount;
                                    betScript.hr = ReferenceManager.refMngr.hour;
                                    betScript.mns = ReferenceManager.refMngr.minutes;
                                    betScript.secs = ReferenceManager.refMngr.seconds;
                                    betScript.isTablePlaying = ReferenceManager.refMngr.isTablePlaying;
                                    betScript.SetTexts();
                                    //For countdown
                                    //else
                                    //{
                                    //    if (isLessThanADay)
                                    //    {
                                    //        betScript.hr = ReferenceManager.refMngr.hour;
                                    //        betScript.mns = ReferenceManager.refMngr.minutes;
                                    //        betScript.secs = ReferenceManager.refMngr.seconds;
                                    //        bidTime = betScript.hr + ":" + betScript.mns + ":" + betScript.secs;
                                    //        betScript.UpdateClock();
                                    //    }
                                    //    else
                                    //    {
                                    //        bidTime = "Date: " + date + "\n" + "Time: " + startTime;
                                    //    }
                                    //    //betScript.betTime.GetComponent<Text>().fontStyle = FontStyle.Normal;
                                    //    betScript.GetComponent<Button>().interactable = false;
                                    //    betScript.myToggle.interactable = false;
                                    //    betScript.betTime.gameObject.SetActive(true);
                                    //}
                                    betScript.Callingfunction = twoplayerac;
                                }
                                twoplayerac++;
                            }
                            else
                            {
                                //Debug.LogError("Start Time: " + startTime);
                                //Debug.LogError("Start Date: " + startDate);
                            }

                            //    }
                            //}
                        }
                    }
                    //else
                    //{
                    //    if (playerType == "4")
                    //    {
                    //        BetScript betScript = Instantiate(betdataPrefab, betdataPublic).GetComponent<BetScript>();
                    //        betScript.id = jsonvale["result_push"][i]["table_id"].ToString();
                    //        betScript.betType = jsonvale["result_push"][i]["betting type"].ToString();

                    //        float bettingValue = float.Parse(jsonvale["result_push"][i]["betting value "].ToString());
                    //        float winningAmount = float.Parse(jsonvale["result_push"][i]["winning amount"].ToString());
                    //        betScript.betAmount.text = "Entry Fee:" + bettingValue.ToString();
                    //        betScript.winningAmount.text = "Winning Fee:" + winningAmount.ToString();
                    //        betScript.Callingfunction = twoplayerac;
                    //        twoplayerac++;
                    //    }
                    //}
                }
                finishedFirstCall = true;
            }
        }
    }

    bool isprivatetable = false;
    IEnumerator privateBetting()
    {
        using (WWW www = new WWW(betURL))
        {
            yield return www;
            Debug.Log("API Call" + www.text);
            int twoplayerac = 0;
            JsonData jsonvale = JsonMapper.ToObject(www.text);
            for (int i = 0; i < jsonvale["result_push"].Count; i++)
            {
                string playerType = jsonvale["result_push"][i]["betting type"].ToString();
                if (!isprivatetable)
                {
                    if (playerType == "4")
                    {
                        PrivateBetScript betScript = Instantiate(privatebetdataPrefab, privatebetdataPublic).GetComponent<PrivateBetScript>();
                        betScript.id = jsonvale["result_push"][i]["table_id"].ToString();
                        float bettingValue = float.Parse(jsonvale["result_push"][i]["betting value "].ToString());
                        float winningAmount = float.Parse(jsonvale["result_push"][i]["winning amount"].ToString());
                        //betScript.betAmount.text = "Entry Fee:" + bettingValue.ToString();
                        betScript.betAmount.text = bettingValue.ToString();
                        betScript.winningAmount.text = "Winning Fee:" + winningAmount.ToString();
                        betScript.Callingfunction = twoplayerac;
                        twoplayerac++;
                    }
                }
            }
        }
    }
}