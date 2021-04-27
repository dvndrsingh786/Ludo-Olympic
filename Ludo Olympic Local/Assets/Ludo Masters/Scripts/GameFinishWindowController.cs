using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFinishWindowController : MonoBehaviour
{

    public GameObject Window;
    public GameObject[] AvatarsMain;
    public GameObject[] AvatarsImage;
    public GameObject[] Names;
    public GameObject[] Backgrounds;
    public GameObject[] PrizeMainObjects;
    public GameObject[] prizeText;
    public GameObject[] placeIndicators;
    
    
    public static string current;
  
    WWW w;
    string status;
    string url;
    private string results;
    public string Results
    {
        get
        {
            return results;
        }
    }
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < AvatarsMain.Length; i++)
        {
            AvatarsMain[i].SetActive(false);
        }
       
    }

    public void showWindow(List<PlayerObject> playersFinished, List<PlayerObject> otherPlayers, float firstPlacePrize, float secondPlacePrize)
    {
        Debug.LogError("Show Windowww");
        if (secondPlacePrize == 0)
        {
            PrizeMainObjects[1].SetActive(false);
        }

        prizeText[0].GetComponent<Text>().text = firstPlacePrize.ToString();
        prizeText[1].GetComponent<Text>().text = secondPlacePrize.ToString();

        if (GameManager.Instance.type == MyGameType.TwoPlayer)
        {
            prizeText[0].GetComponent<Text>().text = ReferenceManager.refMngr.firstPlacePrize;
            prizeText[1].GetComponent<Text>().text = ReferenceManager.refMngr.secondPlacePrize;
            if (ReferenceManager.refMngr.onlineNoOfPlayer == 4)
            {
                prizeText[2].GetComponent<Text>().text = ReferenceManager.refMngr.thirdPlacePrize;
            }
        }

        Window.SetActive(true);
        for (int i = 0; i < playersFinished.Count; i++)
        {
            AvatarsMain[i].SetActive(true);
            AvatarsImage[i].GetComponent<Image>().sprite = playersFinished[i].avatar;
            Names[i].GetComponent<Text>().text = playersFinished[i].name;
            if (playersFinished[i].id.Equals(PhotonNetwork.player.NickName))
            {
                Backgrounds[i].SetActive(true);
            }
            
            //OnAddCoin(firstPlacePrize.ToString());
            
        }

        int counter = 0;
        for (int i = playersFinished.Count; i < playersFinished.Count + otherPlayers.Count; i++)
        {
            if (i == 1)
            {
                PrizeMainObjects[1].SetActive(false);
            }
            AvatarsMain[i].SetActive(true);
            AvatarsImage[i].GetComponent<Image>().sprite = otherPlayers[counter].avatar;
            Names[i].GetComponent<Text>().text = otherPlayers[counter].name;
            if (otherPlayers[counter].id.Equals(PhotonNetwork.player.NickName))
            {
                Backgrounds[i].SetActive(true);
            }
            if (otherPlayers.Count > 1)
                placeIndicators[i].SetActive(false);
            counter++;
        }

    }

    public void showWindowManually(List<PlayerObject> playersFinished)
    {
        Debug.LogError("showWindowManually");
        if (GameManager.Instance.type == MyGameType.TwoPlayer)
        {
            prizeText[0].GetComponent<Text>().text = ReferenceManager.refMngr.firstPlacePrize;
            if (ReferenceManager.refMngr.onlineNoOfPlayer == 4)
            {
                prizeText[1].GetComponent<Text>().text = ReferenceManager.refMngr.secondPlacePrize;
                prizeText[2].GetComponent<Text>().text = ReferenceManager.refMngr.thirdPlacePrize;
                PrizeMainObjects[1].SetActive(true);
                PrizeMainObjects[2].SetActive(true);
            }
            else
            {
                PrizeMainObjects[1].SetActive(false);
                PrizeMainObjects[2].SetActive(false);
                prizeText[1].gameObject.SetActive(false);
                prizeText[2].gameObject.SetActive(false);
            }
        }

        Window.SetActive(true);
        for (int i = 0; i < playersFinished.Count; i++)
        {
            AvatarsMain[i].SetActive(true);
            AvatarsImage[i].GetComponent<Image>().sprite = playersFinished[i].avatar;
            Names[i].GetComponent<Text>().text = playersFinished[i].name;
            if (playersFinished[i].id.Equals(PhotonNetwork.player.NickName))
            {
                //Backgrounds[i].SetActive(true);
            }
        }
        for (int i = 0; i < playersFinished.Count; i++)
        {
            Debug.LogError("NAme::: " + playersFinished[i].name + " POSSS::: " + playersFinished[i].myPosition + " III::: " + i);
        }
        for (int i = 0; i < playersFinished.Count; i++)
        {
            for (int j = 0; j < playersFinished.Count; j++)
            {
                if (playersFinished[i].myPosition == j + 1)
                {
                    AvatarsMain[j].SetActive(true);
                    AvatarsImage[j].GetComponent<Image>().sprite = playersFinished[i].avatar;
                    Names[j].GetComponent<Text>().text = playersFinished[i].name;
                }
            }
        }
    }
}
