﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnterPrivateCodeDialogController : MonoBehaviour
{

    public GameObject inputField;
    public GameObject confirmationText;
    public GameObject joinButton;

    private Button join;
    private InputField field;
    public GameObject GameConfiguration;
    public GameObject failedDialog;
    public GameObject failedDialogSelectBet;

    public TMP_InputField createAmount;

    void OnEnable()
    {
      
        if (field != null && field.gameObject.activeInHierarchy)
            field.text = "";
        if (confirmationText != null)
            confirmationText.SetActive(false);
       
        if (join != null && field.gameObject.activeInHierarchy)
            join.interactable = false;
    }

    // Use this for initialization
    void Awake()
    {
        field = inputField.GetComponent<InputField>();
        join = joinButton.GetComponent<Button>();
        join.interactable = true;
    }



    public void onValueChanged()
    {
        Debug.Log(field.gameObject.activeInHierarchy);
        Debug.Log(field.gameObject.activeSelf);
        if (field.text.Length < 4 )
        {
            confirmationText.SetActive(true);
            join.interactable = false;
        }
        else
        {
            confirmationText.SetActive(false);
            join.interactable = true;
        }
    }


    public void CreateRoom()
    {
        GameManager.Instance.playfabManager.roomOwner = true;
        GameManager.Instance.roomOwner = true;
        GameManager.Instance.opponentsIDs = new List<string>() { null, null, null };
        GameManager.Instance.opponentsAvatars = new List<Sprite>() { null, null, null };
        GameManager.Instance.opponentsNames = new List<string>() { null, null, null };
        GameManager.Instance.opponentsFullNames = new List<string>() { null, null, null };
        if (createAmount.text == "")
        {
            ReferenceManager.refMngr.ShowError("Amount should be more than zero", "Error");
            return;
        }
        else if (int.Parse(createAmount.text) <= 0)
        {
            ReferenceManager.refMngr.ShowError("Amount should be more than zero", "Error");
            return;
        }
        if (!PhotonNetwork.connectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings("1.0");
            ReferenceManager.refMngr.ShowError("Unable to create room, try again later", "Error");
            return;
        }
        FindObjectOfType<GameConfigrationController>().ChangeBettingAmountDav(int.Parse(createAmount.text));
        ReferenceManager.refMngr.isBidSelected = false;
        //Debug.LogError("Is master client: " + PhotonNetwork.isMasterClient) ;
        //Debug.LogError("Is connected: " + PhotonNetwork.connectionState) ;
        //Debug.LogError("Is in room: " + PhotonNetwork.inRoom) ;
        if (GameManager.Instance.myPlayerData.GetCoins() >= GameManager.Instance.currentBetAmount)
        {
            GameManager.Instance.type = MyGameType.Private;
            GameManager.Instance.JoinedByID = false;

            string roomID = Random.Range(1000, 10000).ToString();
            GameManager.Instance.playfabManager.CreatePrivateRoom(roomID);
            GameManager.Instance.matchPlayerObject.GetComponent<SetMyData>().MatchPlayer();
        }
        else
        {
            ReferenceManager.refMngr.ShowError("Insufficient Balance", "Error");
        }

        //GameConfiguration.GetComponent<GameConfigrationController>().startGame();
    }

    public void JoinByRoomID()
    {
        // GameManager.Instance.payoutCoins = 0;
        GameManager.Instance.playfabManager.roomOwner = false;
        GameManager.Instance.roomOwner = false;
        GameManager.Instance.opponentsIDs = new List<string>() { null, null, null };
        GameManager.Instance.opponentsAvatars = new List<Sprite>() { null, null, null };
        GameManager.Instance.opponentsNames = new List<string>() { null, null, null };
        GameManager.Instance.opponentsFullNames = new List<string>() { null, null, null };
        if (!PhotonNetwork.connectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings("1.0");
            ReferenceManager.refMngr.ShowError("Unable to join room, try again later", "Error");
            return;
        }
        string roomID = field.text;

        RoomInfo[] rooms = PhotonNetwork.GetRoomList();


        if (rooms.Length == 0)
        {
            Debug.Log("no rooms!");
            failedDialog.SetActive(true);
        }
        else
        {
            bool foundRoom = false;
            for (int i = 0; i < rooms.Length; i++)
            {
                if (rooms[i].Name.Equals(roomID))
                {
                    foundRoom = true;
                    if (rooms[i].CustomProperties.ContainsKey("pc"))
                    {
                        if (bool.Parse(rooms[i].CustomProperties["privateRoom"].ToString()) == true)
                        {
                            GameManager.Instance.currentBetAmount = float.Parse(rooms[i].CustomProperties["betAmount"].ToString());
                        }
                        else
                        {
                            GameManager.Instance.payoutCoins = float.Parse(rooms[i].CustomProperties["pc"].ToString());
                            GameManager.Instance.currentWinningAmount = GameManager.Instance.payoutCoins;
                            GameManager.Instance.currentBetAmount = GameManager.Instance.payoutCoins / 2;
                        }
                        Debug.Log(rooms[i].CustomProperties["pc"]);
                        //if (GameManager.Instance.myPlayerData.GetCoins() >= GameManager.Instance.payoutCoins/2)
                        if (GameManager.Instance.coinsCount >= GameManager.Instance.currentBetAmount)
                        {
                            GameManager.Instance.type = MyGameType.Private;
                            GameManager.Instance.JoinedByID = true;
                            GameManager.Instance.privateRoomjoiningId = roomID;
                            PhotonNetwork.JoinRoom(roomID);
                            GameConfiguration.GetComponent<GameConfigrationController>().startGame();
                        }
                        else
                        {
                            ReferenceManager.refMngr.ShowError("Insufficient Balance", "Error");
                        }
                    }
                    else
                    {
                        // GameManager.Instance.payoutCoins = int.MaxValue;
                        GameConfiguration.GetComponent<GameConfigrationController>().startGame();
                    }
                }

                if (!foundRoom)
                {
                    failedDialog.SetActive(true);
                }
            }
        }
    }
}
