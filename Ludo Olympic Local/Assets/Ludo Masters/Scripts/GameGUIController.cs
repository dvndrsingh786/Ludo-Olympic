using AssemblyCSharp;
using LitJson;
//using Facebook.Unity;
using Photon;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class GameGUIController : PunBehaviour
{
    public GameObject PrizeTopBar;

    public GameObject TIPButtonObject;
    public GameObject TIPObject;
    public GameObject firstPrizeObject;
    public GameObject SecondPrizeObject;
    public GameObject firstPrizeText;
    public Text positionText;
    public GameObject secondPrizeText;
    public GameObject stars;
    public AudioSource WinSound;
    public AudioSource myTurnSource;
    public AudioSource oppoTurnSource;

    public GameObject audioOnIcon;
    public GameObject audioOffIcon;
    // LUDO
    public MultiDimensionalGameObject[] PlayersPawns;
    public GameObject[] PlayersDices;

    public GameObject[] PlayersLocalMultiDices;

    public GameObject[] CompMultiDices;

    public GameObject[] HomeLockObjects;

    public RawImage pImage;

    public GameObject loadingPanel;

    [System.Serializable]
    public class MultiDimensionalGameObject
    {
        public GameObject[] objectsArray;
    }

    public GameObject ludoBoard;//,voiceChatButton;
    public GameObject[] diceBackgrounds;
    public MultiDimensionalGameObject[] playersPawnsColors;
    public MultiDimensionalGameObject[] playersPawnsMultiple;
    private Color colorRed = new Color(250.0f / 255.0f, 12.0f / 255, 12.0f / 255);
    private Color colorBlue = new Color(0, 86.0f / 255, 255.0f / 255);
    private Color colorYellow = new Color(255.0f / 255.0f, 163.0f / 255, 0);
    private Color colorGreen = new Color(8.0f / 255, 174.0f / 255, 30.0f / 255);

    // END LUDO

    public GameObject GameFinishWindow;
    public GameObject ScreenShotController;
    public GameObject invitiationDialog;
    public GameObject addedFriendWindow;
    public GameObject PlayerInfoWindow;
    public GameObject ChatWindow;
    public GameObject ChatButton;
    private bool SecondPlayerOnDiagonal = true;

    private List<string> PlayersIDs;
    public GameObject[] Players;

    public GameObject[] LocalPlayers;

    public GameObject[] ComputerWithOnePlayers;
    public GameObject[] PlayersTimers;
    public GameObject[] PlayersChatBubbles;
    public GameObject[] PlayersChatBubblesText;
    public GameObject[] PlayersChatBubblesImage;
    private GameObject[] ActivePlayers;
    public GameObject[] PlayersAvatarsButton;

    private List<Sprite> avatars;
    private List<string> names;

    public List<PlayerObject> playerObjects;
    public int myIndex;
    private string myId;

    private Color[] borderColors = new Color[4] { Color.yellow, Color.green, Color.red, Color.blue };

    public int currentPlayerIndex;

    private int ActivePlayersInRoom;

    private Sprite[] emojiSprites;

    private string CurrentPlayerID;

    private List<PlayerObject> playersFinished = new List<PlayerObject>();

    private bool iFinished = false;
    private bool FinishWindowActive = false;

    private float firstPlacePrize;
    private int secondPlacePrize;

    public Sprite[] PawnColorsSprite;

    public GameObject PlayerManager, LocalPlayerManager, ComputerManager;

    public GameObject[] AllCompManager;

    public GameObject SavingWindow;

    public Text playerName, playerName2, playerName3, playerName4;

    public TempGameManager tempGame;
    public TextMeshProUGUI gameDuration;

    public bool canPlayGame = true;

    void AddPlayingTable()
    {
        FindObjectOfType<APIManager>().AddStartedTable(FindObjectOfType<APIManager>().startedTableValue);
    }

    // Use this for initialization
    void Start()
    {
        //dfdsDebug.LogError("Background TimeOut: " + PhotonNetwork.BackgroundTimeout);
        // LUDO
        // Rotate board and set colors
        startSeconds = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
        GameManager.exitedPlayers = 0;
        canPlayGame = true;
        if (GameManager.Instance.type != MyGameType.Private)
        {
            AddPlayingTable();
        }
        if (GameManager.Instance.playfabManager.apiManager.joinedOnlineOnTime)
        {
            int abc = 1;
            for (int i = 0; i < GameManager.Instance.opponentsIDs.Count; i++)
            {
                if (GameManager.Instance.opponentsIDs[i] != null)
                {
                    abc++;
                }
            }
            ReferenceManager.refMngr.onlineNoOfPlayer = abc;
            PlayerManager.SetActive(false);
            LocalPlayerManager.SetActive(false);
            ComputerManager.SetActive(false);
            Debug.Log("requiredPlayers   " + GameManager.Instance.requiredPlayers + " isPlayingWithComputer  " + GameManager.Instance.isPlayingWithComputer);

            int rotation = GameManager.Instance.isColorSelected;
            Debug.LogWarning("Old Rotation: " + rotation);
            rotation = 0;
            Debug.LogWarning("Setting color selected to default(zero)");
            StartCoroutine(pic());
            Sprite[] colors = null;

            if (rotation == 0)
            {
                colors = new Sprite[] { PawnColorsSprite[0], PawnColorsSprite[1], PawnColorsSprite[2], PawnColorsSprite[3] };
            }
            else if (rotation == 1)
            {
                ComputerManager = AllCompManager[0];
                colors = new Sprite[] { PawnColorsSprite[3], PawnColorsSprite[0], PawnColorsSprite[1], PawnColorsSprite[2] };
                ludoBoard.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -90.0f);
            }
            else if (rotation == 2)
            {
                ComputerManager = AllCompManager[1];
                colors = new Sprite[] { PawnColorsSprite[2], PawnColorsSprite[3], PawnColorsSprite[0], PawnColorsSprite[1] };
                ludoBoard.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -180.0f);
            }
            else
            {
                ComputerManager = AllCompManager[2];
                colors = new Sprite[] { PawnColorsSprite[1], PawnColorsSprite[2], PawnColorsSprite[3], PawnColorsSprite[0] };
                ludoBoard.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -270.0f);
            }

            if (GameManager.Instance.isLocalMultiplayer)
            {
                if (GameManager.Instance.isPlayingWithComputer)
                {
                    PrizeTopBar.SetActive(true);
                    if (GameManager.Instance.requiredPlayers == 2)
                    {

                        ComputerManager.SetActive(true);
                        CompMultiDices[0] = GameObject.Find("Dice1");
                        CompMultiDices[1] = GameObject.Find("Dice3");

                        ComputerWithOnePlayers[0] = GameObject.Find("Player1");
                        ComputerWithOnePlayers[1] = GameObject.Find("Player3");
                    }
                    else
                        LocalPlayerManager.SetActive(true);
                }
                else
                {
                    PrizeTopBar.SetActive(false);
                    LocalPlayerManager.SetActive(true);
                }

                ChatWindow.SetActive(false);
                // voiceChatButton.SetActive(false);
                ChatButton.SetActive(false);
            }
            else
            {
                PlayerManager.SetActive(true);
            }

            if (GameManager.Instance.type == MyGameType.Private)
            {
                //  voiceChatButton.SetActive(true);
            }
            else
            {
                // voiceChatButton.SetActive(false);
            }

            // for (int i = 0; i < diceBackgrounds.Length; i++)
            // {
            //     diceBackgrounds[i].GetComponent<Image>().color = colors[i];
            // }

            for (int i = 0; i < playersPawnsColors.Length; i++)
            {
                for (int j = 0; j < playersPawnsColors[i].objectsArray.Length; j++)
                {
                    playersPawnsColors[i].objectsArray[j].GetComponent<Image>().sprite = colors[i];
                    //  playersPawnsMultiple[i].objectsArray[j].GetComponent<Image>().color = colors[i];
                }
            }

            // END LUDO

            //        Debug.Log("Done   ");

            currentPlayerIndex = 0;
            emojiSprites = GameManager.Instance.playfabManager.staticGameVariables.emoji;
            myId = GameManager.Instance.playfabManager.PlayFabId;
            playerObjects = new List<PlayerObject>();
            //avatars = GameManager.Instance.opponentsAvatars;
            Debug.LogError(GameManager.Instance.opponentsAvatars.Count);
            avatars = new List<Sprite>();
            Debug.LogError(avatars.Count);
            avatars.AddRange(GameManager.Instance.opponentsAvatars);
            avatars.Insert(0, GameManager.Instance.avatarMy);

            //names = GameManager.Instance.opponentsNames;
            names = new List<string>();
            names.AddRange(GameManager.Instance.opponentsNames);
            names.Insert(0, GameManager.Instance.nameMy);
            PlayersIDs = new List<string>();
            Debug.LogError("OPPONENTS IDS: " + GameManager.Instance.opponentsIDs.Count);

            for (int i = 0; i < GameManager.Instance.opponentsIDs.Count; i++)
            {
                if (GameManager.Instance.opponentsIDs[i] != null)
                {
                    PlayersIDs.Add(GameManager.Instance.opponentsIDs[i]);
                }
            }

            PlayersIDs.Insert(0, GameManager.Instance.playfabManager.PlayFabId);

            for (int i = 0; i < PlayersIDs.Count; i++)
            {
                Debug.Log(PlayersIDs[i]);
                playerObjects.Add(new PlayerObject(names[i], PlayersIDs[i], avatars[i]));
            }

            // Bubble sort
            for (int i = 0; i < PlayersIDs.Count; i++)
            {
                for (int j = 0; j < PlayersIDs.Count - 1; j++)
                {
                    if (string.Compare(playerObjects[j].name, playerObjects[j + 1].name) == 1)
                    {
                        // swaap ids
                        PlayerObject temp = playerObjects[j + 1];
                        playerObjects[j + 1] = playerObjects[j];
                        playerObjects[j] = temp;
                    }
                }
            }

            for (int i = 0; i < PlayersIDs.Count; i++)
            {
                Debug.Log(playerObjects[i].id);
            }

            ActivePlayersInRoom = PlayersIDs.Count;

            if (PlayersIDs.Count == 2)
            {
                Debug.Log("2 Player  ");
                if (SecondPlayerOnDiagonal)
                {
                    ActivePlayers = new GameObject[2];
                    if (!GameManager.Instance.isLocalMultiplayer)
                    {
                        Players[1].SetActive(false);
                        Players[3].SetActive(false);
                        ActivePlayers[0] = Players[0];
                        ActivePlayers[1] = Players[2];
                    }
                    else
                    {
                        if (GameManager.Instance.isPlayingWithComputer)
                        {
                            ActivePlayers = ComputerWithOnePlayers;
                        }
                        else
                        {
                            LocalPlayers[1].SetActive(false);
                            LocalPlayers[3].SetActive(false);
                            ActivePlayers[0] = LocalPlayers[0];
                            ActivePlayers[1] = LocalPlayers[2];
                        }
                    }

                    // LUDO
                    for (int i = 0; i < PlayersPawns[1].objectsArray.Length; i++)
                    {
                        PlayersPawns[1].objectsArray[i].SetActive(false);
                    }

                    for (int i = 0; i < PlayersPawns[3].objectsArray.Length; i++)
                    {
                        PlayersPawns[3].objectsArray[i].SetActive(false);
                    }

                    // END LUDO
                }
                else
                {
                    // LUDO
                    for (int i = 0; i < PlayersPawns[2].objectsArray.Length; i++)
                    {
                        PlayersPawns[2].objectsArray[i].SetActive(false);
                    }

                    for (int i = 0; i < PlayersPawns[3].objectsArray.Length; i++)
                    {
                        PlayersPawns[3].objectsArray[i].SetActive(false);
                    }

                    ActivePlayers = new GameObject[2];
                    if (!GameManager.Instance.isLocalMultiplayer)
                    {
                        Players[2].SetActive(false);
                        Players[3].SetActive(false);
                        ActivePlayers[0] = Players[0];
                        ActivePlayers[1] = Players[1];
                    }
                    else
                    {
                        if (GameManager.Instance.isPlayingWithComputer)
                        {
                            ActivePlayers = ComputerWithOnePlayers;
                        }
                        else
                        {
                            LocalPlayers[2].SetActive(false);
                            LocalPlayers[3].SetActive(false);
                            ActivePlayers[0] = LocalPlayers[0];
                            ActivePlayers[1] = LocalPlayers[1];
                        }
                    }
                }
            }
            else
            {

                if (!GameManager.Instance.isLocalMultiplayer)
                    ActivePlayers = Players;
                else
                {
                    ActivePlayers = LocalPlayers;
                }
                Debug.Log("4 ActivePlayers  " + ActivePlayers);
            }

            int startPos = 0;
            for (int i = 0; i < playerObjects.Count; i++)
            {
                if (playerObjects[i].id == GameManager.Instance.playfabManager.PlayFabId)
                {
                    startPos = i;
                    break;
                }
            }
            int index = 0;
            bool addedMe = false;
            myIndex = startPos;
            GameManager.Instance.myPlayerIndex = myIndex;
            //TempGameManager.tempGM.SetIndexes(myIndex, true);
            //TempGameManager.tempGM.view.RPC("SetIndexes", PhotonTargets.OthersBuffered, myIndex, true);
            for (int i = startPos; ;)
            {
                if (i == startPos && addedMe) break;

                if (PlayersIDs.Count == 2 && SecondPlayerOnDiagonal)
                {
                    if (addedMe)
                    {
                        playerObjects[i].timer = PlayersTimers[2];
                        playerObjects[i].ChatBubble = PlayersChatBubbles[2];
                        playerObjects[i].ChatBubbleText = PlayersChatBubblesText[2];
                        playerObjects[i].ChatbubbleImage = PlayersChatBubblesImage[2];
                        string id = playerObjects[i].id;
                        PlayersAvatarsButton[2].GetComponent<Button>().onClick.RemoveAllListeners();
                        PlayersAvatarsButton[2].GetComponent<Button>().onClick.AddListener(() => ButtonClick(id));

                        // LUDO

                        playerObjects[i].dice = PlayersDices[2];

                        if (GameManager.Instance.isLocalMultiplayer)
                        {
                            if (GameManager.Instance.isPlayingWithComputer && GameManager.Instance.requiredPlayers == 2)
                            {
                                playerObjects[i].dice = CompMultiDices[1];
                                Debug.Log("playerObjects    " + playerObjects[i].dice);
                            }
                            else
                            {
                                playerObjects[i].dice = PlayersLocalMultiDices[2];
                            }
                        }


                        playerObjects[i].pawns = PlayersPawns[2].objectsArray;

                        for (int k = 0; k < playerObjects[i].pawns.Length; k++)
                        {
                            playerObjects[i].pawns[k].GetComponent<LudoPawnController>().setPlayerIndex(i);
                        }
                        playerObjects[i].homeLockObjects = HomeLockObjects[2];

                        // END LUDO
                    }
                    else
                    {
                        GameManager.Instance.myPlayerIndex = i;
                        playerObjects[i].timer = PlayersTimers[index];
                        playerObjects[i].ChatBubble = PlayersChatBubbles[index];
                        playerObjects[i].ChatBubbleText = PlayersChatBubblesText[index];
                        playerObjects[i].ChatbubbleImage = PlayersChatBubblesImage[index];
                        string id = playerObjects[i].id;

                        // LUDO
                        playerObjects[i].dice = PlayersDices[index];
                        // if (GameManager.Instance.isLocalMultiplayer)
                        //     playerObjects[i].dice = PlayersLocalMultiDices[index];

                        if (GameManager.Instance.isLocalMultiplayer)
                        {
                            if (GameManager.Instance.isPlayingWithComputer && GameManager.Instance.requiredPlayers == 2)
                            {
                                playerObjects[i].dice = CompMultiDices[index];
                            }
                            else
                            {
                                playerObjects[i].dice = PlayersLocalMultiDices[index];
                            }
                        }

                        playerObjects[i].pawns = PlayersPawns[index].objectsArray;

                        for (int k = 0; k < playerObjects[i].pawns.Length; k++)
                        {
                            playerObjects[i].pawns[k].GetComponent<LudoPawnController>().setPlayerIndex(i);
                        }
                        playerObjects[i].homeLockObjects = HomeLockObjects[index];
                        // END LUDO
                    }
                }
                else
                {

                    playerObjects[i].timer = PlayersTimers[index];
                    playerObjects[i].ChatBubble = PlayersChatBubbles[index];
                    playerObjects[i].ChatBubbleText = PlayersChatBubblesText[index];
                    playerObjects[i].ChatbubbleImage = PlayersChatBubblesImage[index];

                    // LUDO
                    playerObjects[i].dice = PlayersDices[index];
                    if (GameManager.Instance.isLocalMultiplayer)
                    {
                        if (GameManager.Instance.isPlayingWithComputer && GameManager.Instance.requiredPlayers == 2)
                        {
                            playerObjects[i].dice = CompMultiDices[index];
                        }
                        else
                        {
                            playerObjects[i].dice = PlayersLocalMultiDices[index];
                        }
                    }


                    playerObjects[i].pawns = PlayersPawns[index].objectsArray;

                    for (int k = 0; k < playerObjects[i].pawns.Length; k++)
                    {
                        playerObjects[i].pawns[k].GetComponent<LudoPawnController>().setPlayerIndex(i);
                    }
                    playerObjects[i].homeLockObjects = HomeLockObjects[index];
                    // END LUDO

                    string id = playerObjects[i].id;
                    if (index != 0)
                    {
                        PlayersAvatarsButton[index].GetComponent<Button>().onClick.RemoveAllListeners();
                        PlayersAvatarsButton[index].GetComponent<Button>().onClick.AddListener(() => ButtonClick(id));
                    }
                }
                playerObjects[i].AvatarObject = ActivePlayers[index];
                ActivePlayers[index].GetComponent<PlayerAvatarController>().Name.GetComponent<Text>().text = playerObjects[i].name;
                if (playerObjects[i].avatar != null)
                {
                    if (!GameManager.Instance.isLocalMultiplayer) { }
                    else
                    {
                        ActivePlayers[index].GetComponent<PlayerAvatarController>().Avatar.GetComponent<Image>().sprite = playerObjects[i].avatar;
                    }
                }

                index++;

                if (i < PlayersIDs.Count - 1)
                {
                    i++;
                }
                else
                {
                    i = 0;
                }

                addedMe = true;
            }

            currentPlayerIndex = GameManager.Instance.firstPlayerInGame;
            GameManager.Instance.currentPlayer = playerObjects[currentPlayerIndex];

            SetTurn();

            // if (myIndex == 0)
            // {
            //     SetMyTurn();
            //     playerObjects[0].dice.GetComponent<GameDiceController>().DisableDiceShadow();
            // }
            // else
            // {
            //     SetOpponentTurn();
            //     playerObjects[currentPlayerIndex].dice.GetComponent<GameDiceController>().DisableDiceShadow();
            // }

            GameManager.Instance.playerObjects = playerObjects;

            // Check if all players are still in room - if not deactivate
            for (int i = 0; i < playerObjects.Count; i++)
            {
                bool contains = false;
                if (!playerObjects[i].id.Contains("_BOT"))
                {
                    for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
                    {
                        Debug.Log(PhotonNetwork.playerList[j].NickName);
                        Debug.Log(playerObjects[i].id);
                        Debug.Log(playerObjects[i].name);
                        if (PhotonNetwork.playerList[j].NickName.Equals(playerObjects[i].id))
                        {

                            contains = true;
                            break;
                        }
                    }

                    if (!contains)
                    {
                        setPlayerDisconnected(i);
                    }
                }
            }

            CheckPlayersIfShouldFinishGame();


            firstPlacePrize = GameManager.Instance.currentWinningAmount;


            firstPrizeText.GetComponent<Text>().text = firstPlacePrize + "";
            secondPrizeText.GetComponent<Text>().text = secondPlacePrize + "";

            if (secondPlacePrize == 0)
            {
                SecondPrizeObject.SetActive(false);
                firstPrizeObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(39.9f, firstPrizeObject.GetComponent<RectTransform>().anchoredPosition.y);
            }

            // LUDO

            // Enable home locks

            if (GameManager.Instance.mode == MyGameMode.Quick || GameManager.Instance.mode == MyGameMode.Master)
            {
                for (int i = 0; i < GameManager.Instance.playerObjects.Count; i++)
                {
                    GameManager.Instance.playerObjects[i].homeLockObjects.SetActive(true);
                }
                GameManager.Instance.needToKillOpponentToEnterHome = true;
            }
            else
            {
                GameManager.Instance.needToKillOpponentToEnterHome = false;
            }

            string loadedData = PlayerPrefs.GetString("PlaySavedGame", "null");
            if (!loadedData.Equals("null"))
            {
                LoadPreviousGame(loadedData);
            }

            //GameManager.Instance.needToKillOpponentToEnterHome = false;

            // END LUDO
            if (GameManager.Instance.type != MyGameType.Private)
            {
                ReferenceManager reff = FindObjectOfType<ReferenceManager>();
                int seconds = reff.timeToSecondsMnsScs(GameManager.gameDuration, ':');
                reff.SecondsToTime(seconds);
                hr = reff.hour;
                mns = reff.minutes;
                secs = reff.seconds;
                if (GameManager.Instance.type == MyGameType.TwoPlayer)
                {
                    if (false) Invoke(nameof(UpdateGameDuration), 1);
                    NewGameDuration();
                }
            }
            else
            {
                NewGameDurationPrivate();
            }
        }
        else
        {
            LateJoinedStart();
        }
    }

    int GetCorrespondingPlayerSlot(string nameeee)
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].name == nameeee)
            {
                return i;
            }
        }
        return 0;
    }

    public void SetDesigns(int index)
    {
        PlayersIDs[index] = GameManager.Instance.opponentsIDs[index];
        names[index] = GameManager.Instance.opponentsNames[index];
        avatars[index] = GameManager.Instance.opponentsAvatars[index];
        //Debug.LogError("PHAKKK: " + names[index]);
        //Debug.LogError("PHAKKK2: " + GameManager.Instance.opponentsNames[index]);

        int index1 = GetCorrespondingPlayerSlot(names[index]);

        playerObjects[index1].name = names[index];
        playerObjects[index1].id = PlayersIDs[index];
        playerObjects[index1].isBot = false;
        playerObjects[index1].avatar = avatars[index];
        SendSynchronization();
    }

    void SendSynchronizationOfScore()
    {
        string playersInfo = "";
        for (int i = 0; i < playerObjects.Count; i++)
        {
            UpdatePlayerTimer timerr = playerObjects[i].timer.GetComponent<UpdatePlayerTimer>();
            //if (i != 0) playersInfo += timerr.turnCount + ",";
            playersInfo += "," + timerr.turnCount + ",";
            if (timerr.timer.fillAmount >= 0.86)
            {
                playersInfo += 0.95f + ",";
            }
            else playersInfo += (timerr.timer.fillAmount + 0.1f) + ",";
            playersInfo += timerr.gameObject.activeInHierarchy.ToString() + ",";
            playersInfo += playerObjects[i].dice.GetComponent<GameDiceController>().myScore.text;
            Debug.LogError("SCORE::::::::::::::::::::::::::: " + playerObjects[i].dice.GetComponent<GameDiceController>().myScore.text);
            for (int j = 0; j < 4; j++)
            {
                playersInfo += "," + playerObjects[i].pawns[j].GetComponent<LudoPawnController>().mainInJoint.ToString() + ",";
                playersInfo += playerObjects[i].pawns[j].GetComponent<LudoPawnController>().isOnBoard.ToString() + ",";
                playersInfo += playerObjects[i].pawns[j].GetComponent<LudoPawnController>().currentPosition.ToString();
            }
        }
        string data = currentPlayerIndex.ToString() + "," + gameDuration.text + playersInfo;
        PhotonNetwork.RaiseEvent((int)EnumPhoton.SynchronizeScore, data, true, null);
    }

    void SendSynchronization()
    {
        string playersInfo = "";
        for (int i = 0; i < playerObjects.Count; i++)
        {
            UpdatePlayerTimer timerr = playerObjects[i].timer.GetComponent<UpdatePlayerTimer>();
            //if (i != 0) playersInfo += timerr.turnCount + ",";
            playersInfo += "," + timerr.turnCount + ",";
            if (timerr.timer.fillAmount >= 0.86)
            {
                playersInfo += 0.95f + ",";
            }
            else playersInfo += (timerr.timer.fillAmount + 0.1f) + ",";
            playersInfo += timerr.gameObject.activeInHierarchy.ToString() + ",";
            playersInfo += playerObjects[i].dice.GetComponent<GameDiceController>().myScore.text;
            Debug.LogError("SCORE::::::::::::::::::::::::::: " + playerObjects[i].dice.GetComponent<GameDiceController>().myScore.text);
            for (int j = 0; j < 4; j++)
            {
                playersInfo += "," + playerObjects[i].pawns[j].GetComponent<LudoPawnController>().mainInJoint.ToString() + ",";
                playersInfo += playerObjects[i].pawns[j].GetComponent<LudoPawnController>().isOnBoard.ToString() + ",";
                playersInfo += playerObjects[i].pawns[j].GetComponent<LudoPawnController>().currentPosition.ToString();
            }
        }
        string data = currentPlayerIndex.ToString() + "," + gameDuration.text + playersInfo;
        PhotonNetwork.RaiseEvent((int)EnumPhoton.SynchronizeTurn, data, true, null);
        
        if (GameManager.Instance.roomOwner)
        {
            //if (!IsInvoking(nameof(SendSynchronization))) Invoke(nameof(SendSynchronization), 5);
            if (!IsInvoking(nameof(SendDurationAgain)))
                Invoke(nameof(SendDurationAgain), 5);
        }
    }

    void SendDurationAgain()
    {
        if (canRunTime)
        {
            PhotonNetwork.RaiseEvent((int)EnumPhoton.SetDuration, gameDuration.text, true, null);
            if (!IsInvoking(nameof(SendDurationAgain)))
                Invoke(nameof(SendDurationAgain), 5);
        }
    }

    void LateJoinedStart()
    {
        PlayerManager.SetActive(false);
        LocalPlayerManager.SetActive(false);
        ComputerManager.SetActive(false);
        Debug.Log("requiredPlayers   " + GameManager.Instance.requiredPlayers + " isPlayingWithComputer  " + GameManager.Instance.isPlayingWithComputer);

        int rotation = GameManager.Instance.isColorSelected;
        Debug.LogWarning("Old Rotation: " + rotation);
        rotation = 0;
        Debug.LogWarning("Setting color selected to default(zero)");
        StartCoroutine(pic());

        Sprite[] colors = null;

        if (rotation == 0)
        {
            colors = new Sprite[] { PawnColorsSprite[0], PawnColorsSprite[1], PawnColorsSprite[2], PawnColorsSprite[3] };
        }
        else if (rotation == 1)
        {
            ComputerManager = AllCompManager[0];
            colors = new Sprite[] { PawnColorsSprite[3], PawnColorsSprite[0], PawnColorsSprite[1], PawnColorsSprite[2] };
            ludoBoard.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -90.0f);
        }
        else if (rotation == 2)
        {
            ComputerManager = AllCompManager[1];
            colors = new Sprite[] { PawnColorsSprite[2], PawnColorsSprite[3], PawnColorsSprite[0], PawnColorsSprite[1] };
            ludoBoard.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -180.0f);
        }
        else
        {
            ComputerManager = AllCompManager[2];
            colors = new Sprite[] { PawnColorsSprite[1], PawnColorsSprite[2], PawnColorsSprite[3], PawnColorsSprite[0] };
            ludoBoard.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -270.0f);
        }

        PlayerManager.SetActive(true);

        for (int i = 0; i < playersPawnsColors.Length; i++)
        {
            for (int j = 0; j < playersPawnsColors[i].objectsArray.Length; j++)
            {
                playersPawnsColors[i].objectsArray[j].GetComponent<Image>().sprite = colors[i];
                //  playersPawnsMultiple[i].objectsArray[j].GetComponent<Image>().color = colors[i];
            }
        }

        GameManager.Instance.currentPlayersCount = 1;
        GameManager.Instance.playfabManager.GetOpponentDetails();
        GameManager.Instance.playfabManager.LoadLateBots();

        int abc = 1;
        for (int i = 0; i < GameManager.Instance.opponentsIDs.Count; i++)
        {
            if (GameManager.Instance.opponentsIDs[i] != null)
            {
                abc++;
            }
        }
        ReferenceManager.refMngr.onlineNoOfPlayer = abc;

        for (int i = 0; i < GameManager.Instance.opponentsNames.Count; i++)
        {
            Debug.LogError(GameManager.Instance.opponentsNames[i]);
        }

        currentPlayerIndex = 0;
        emojiSprites = GameManager.Instance.playfabManager.staticGameVariables.emoji;
        myId = GameManager.Instance.playfabManager.PlayFabId;

        playerObjects = new List<PlayerObject>();
        //avatars = GameManager.Instance.opponentsAvatars;
        //Debug.LogError(GameManager.Instance.opponentsAvatars.Count);
        avatars = new List<Sprite>();
        //Debug.LogError(avatars.Count);
        avatars.AddRange(GameManager.Instance.opponentsAvatars);
        avatars.Insert(0, GameManager.Instance.avatarMy);

        //names = GameManager.Instance.opponentsNames;
        names = new List<string>();
        names.AddRange(GameManager.Instance.opponentsNames);
        names.Insert(0, GameManager.Instance.nameMy);
        PlayersIDs = new List<string>();

        for (int i = 0; i < GameManager.Instance.opponentsIDs.Count; i++)
        {
            if (GameManager.Instance.opponentsIDs[i] != null)
            {
                PlayersIDs.Add(GameManager.Instance.opponentsIDs[i]);
            }
        }

        PlayersIDs.Insert(0, GameManager.Instance.playfabManager.PlayFabId);


        for (int i = 0; i < PlayersIDs.Count; i++)
        {
            Debug.Log(PlayersIDs[i]);
            playerObjects.Add(new PlayerObject(names[i], PlayersIDs[i], avatars[i]));
        }

        Debug.LogError("NAMES: " + names.Count);
        Debug.LogError("NAaaMES: " + playerObjects.Count);
        // Bubble sort
        for (int i = 0; i < PlayersIDs.Count; i++)
        {
            for (int j = 0; j < PlayersIDs.Count - 1; j++)
            {
                if (string.Compare(playerObjects[j].name, playerObjects[j + 1].name) == 1)
                {
                    // swaap ids
                    PlayerObject temp = playerObjects[j + 1];
                    playerObjects[j + 1] = playerObjects[j];
                    playerObjects[j] = temp;
                }
            }
        }

        for (int i = 0; i < PlayersIDs.Count; i++)
        {
            Debug.Log(playerObjects[i].id);
        }

        ActivePlayersInRoom = PlayersIDs.Count;
        Debug.LogError("Active players in room: " + ActivePlayersInRoom);
        //        int tempActivePlayer = ActivePlayersInRoom--;


        if (ActivePlayersInRoom == 2)
        {
            Debug.Log("2 Player  ");
            if (SecondPlayerOnDiagonal || true)
            {
                ActivePlayers = new GameObject[2];
                if (!GameManager.Instance.isLocalMultiplayer)
                {
                    Players[1].SetActive(false);
                    Players[3].SetActive(false);
                    ActivePlayers[0] = Players[0];
                    ActivePlayers[1] = Players[2];
                }

                // LUDO
                for (int i = 0; i < PlayersPawns[1].objectsArray.Length; i++)
                {
                    PlayersPawns[1].objectsArray[i].SetActive(false);
                }

                for (int i = 0; i < PlayersPawns[3].objectsArray.Length; i++)
                {
                    PlayersPawns[3].objectsArray[i].SetActive(false);
                }

                // END LUDO
            }

        }
        else
        {
            ActivePlayers = Players;
            Debug.Log("4 ActivePlayers  " + ActivePlayers);
        }

        int startPos = 0;
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id == GameManager.Instance.playfabManager.PlayFabId)
            {
                startPos = i;
                break;
            }
        }
        int index = 0;
        bool addedMe = false;
        myIndex = startPos;
        GameManager.Instance.myPlayerIndex = myIndex;
        //TempGameManager.tempGM.SetIndexes(myIndex, true);
        //TempGameManager.tempGM.view.RPC("SetIndexes", PhotonTargets.OthersBuffered, myIndex, true);
        for (int i = startPos; ;)
        {
            if (i == startPos && addedMe) break;

            if (ActivePlayersInRoom == 2 && (SecondPlayerOnDiagonal || true))
            {
                if (addedMe)
                {
                    playerObjects[i].timer = PlayersTimers[2];
                    playerObjects[i].ChatBubble = PlayersChatBubbles[2];
                    playerObjects[i].ChatBubbleText = PlayersChatBubblesText[2];
                    playerObjects[i].ChatbubbleImage = PlayersChatBubblesImage[2];
                    string id = playerObjects[i].id;
                    PlayersAvatarsButton[2].GetComponent<Button>().onClick.RemoveAllListeners();
                    PlayersAvatarsButton[2].GetComponent<Button>().onClick.AddListener(() => ButtonClick(id));

                    // LUDO

                    playerObjects[i].dice = PlayersDices[2];

                    playerObjects[i].pawns = PlayersPawns[2].objectsArray;

                    for (int k = 0; k < playerObjects[i].pawns.Length; k++)
                    {
                        playerObjects[i].pawns[k].GetComponent<LudoPawnController>().setPlayerIndex(i);
                    }
                    playerObjects[i].homeLockObjects = HomeLockObjects[2];

                    // END LUDO
                }
                else
                {
                    GameManager.Instance.myPlayerIndex = i;
                    playerObjects[i].timer = PlayersTimers[index];
                    playerObjects[i].ChatBubble = PlayersChatBubbles[index];
                    playerObjects[i].ChatBubbleText = PlayersChatBubblesText[index];
                    playerObjects[i].ChatbubbleImage = PlayersChatBubblesImage[index];
                    string id = playerObjects[i].id;

                    // LUDO
                    playerObjects[i].dice = PlayersDices[index];

                    playerObjects[i].pawns = PlayersPawns[index].objectsArray;

                    for (int k = 0; k < playerObjects[i].pawns.Length; k++)
                    {
                        playerObjects[i].pawns[k].GetComponent<LudoPawnController>().setPlayerIndex(i);
                    }
                    playerObjects[i].homeLockObjects = HomeLockObjects[index];
                    // END LUDO
                }
            }
            else
            {
                playerObjects[i].timer = PlayersTimers[index];
                playerObjects[i].ChatBubble = PlayersChatBubbles[index];
                playerObjects[i].ChatBubbleText = PlayersChatBubblesText[index];
                playerObjects[i].ChatbubbleImage = PlayersChatBubblesImage[index];

                // LUDO
                playerObjects[i].dice = PlayersDices[index];

                playerObjects[i].pawns = PlayersPawns[index].objectsArray;

                for (int k = 0; k < playerObjects[i].pawns.Length; k++)
                {
                    playerObjects[i].pawns[k].GetComponent<LudoPawnController>().setPlayerIndex(i);
                }
                playerObjects[i].homeLockObjects = HomeLockObjects[index];
                // END LUDO

                string id = playerObjects[i].id;
                if (index != 0)
                {
                    PlayersAvatarsButton[index].GetComponent<Button>().onClick.RemoveAllListeners();
                    PlayersAvatarsButton[index].GetComponent<Button>().onClick.AddListener(() => ButtonClick(id));
                }
            }
            playerObjects[i].AvatarObject = ActivePlayers[index];
            ActivePlayers[index].GetComponent<PlayerAvatarController>().Name.GetComponent<Text>().text = playerObjects[i].name;
            if (playerObjects[i].avatar != null)
            {
                if (!GameManager.Instance.isLocalMultiplayer) { }
                else
                {
                    ActivePlayers[index].GetComponent<PlayerAvatarController>().Avatar.GetComponent<Image>().sprite = playerObjects[i].avatar;
                }
            }

            index++;

            if (i < PlayersIDs.Count - 1)
            {
                i++;
            }
            else
            {
                i = 0;
            }

            addedMe = true;
        }

        currentPlayerIndex = GameManager.Instance.firstPlayerInGame;
        GameManager.Instance.currentPlayer = playerObjects[currentPlayerIndex];

        GameManager.Instance.playerObjects = playerObjects;

        // Check if all players are still in room - if not deactivate
        //for (int i = 0; i < playerObjects.Count; i++)
        //{
        //    bool contains = false;
        //    if (!playerObjects[i].id.Contains("_BOT"))
        //    {
        //        for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
        //        {
        //            Debug.Log(PhotonNetwork.playerList[j].NickName);
        //            Debug.Log(playerObjects[i].id);
        //            Debug.Log(playerObjects[i].name);
        //            if (PhotonNetwork.playerList[j].NickName.Equals(playerObjects[i].id))
        //            {
        //                contains = true;
        //                break;
        //            }
        //        }

        //        if (!contains)
        //        {
        //            setPlayerDisconnected(i);
        //        }
        //    }
        //}

        //CheckPlayersIfShouldFinishGame();


        firstPlacePrize = GameManager.Instance.currentWinningAmount;

        firstPrizeText.GetComponent<Text>().text = firstPlacePrize + "";
        secondPrizeText.GetComponent<Text>().text = secondPlacePrize + "";

        if (secondPlacePrize == 0)
        {
            SecondPrizeObject.SetActive(false);
            firstPrizeObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(39.9f, firstPrizeObject.GetComponent<RectTransform>().anchoredPosition.y);
        }

        // LUDO

        GameManager.Instance.needToKillOpponentToEnterHome = false;

        //GameManager.Instance.needToKillOpponentToEnterHome = false;

        // END LUDO
        //ReferenceManager reff = FindObjectOfType<ReferenceManager>();
        //int seconds = reff.timeToSeconds(GameManager.gameDuration, ':');
        //reff.SecondsToTime(seconds);
        //hr = reff.hour;
        //mns = reff.minutes;
        //secs = reff.seconds;
        if (GameManager.Instance.type == MyGameType.TwoPlayer)
        {
            //Invoke(nameof(UpdateGameDuration), 1);
            NewGameDuration();
        }
    }


    void SetGameDuration(string duration, char separator)
    {
        if (IsInvoking(nameof(UpdateGameDuration)))
        {
            CancelInvoke(nameof(UpdateGameDuration));
        }
        //Debug.LogError("Duration: " + duration);
        duration = duration.Replace("m", "");
        duration = duration.Replace("s", "");
        duration = duration.Replace("h", "");
        string[] temp = duration.Split(separator);
        if (temp.Length < 3)
        {
            Debug.LogError("TEMP0: " + temp[0]);
            mns = int.Parse(temp[0]);
            secs = int.Parse(temp[1]);
        }
        else
        {
            hr = int.Parse(temp[0]);
            mns = int.Parse(temp[1]);
            secs = int.Parse(temp[2]);
        }
        //if (secs == 1) { mns--; secs = 59; }
        //else if (secs == 0) { mns--; secs = 58; }
        //else secs -= 2;
        if (!IsInvoking(nameof(UpdateGameDuration)))
        {
            canRunTime = true;
            UpdateGameDuration();
        }
    }

    public int hr, mns, secs;
    public void UpdateGameDuration()
    {
        //if()
        if (canRunTime && false)
        {
            if (hr <= 0)
            {
                //gameDuration.text = mns.ToString() + "m:" + secs.ToString() + "s";
            }
            else
            {
                //gameDuration.text = hr.ToString() + "h:" + mns.ToString() + "m:" + secs.ToString() + "s";
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
        }
        if (hr != 0 || mns != 0 || secs != 0)
        {
            Invoke(nameof(UpdateGameDuration), 1);
        }
        else
        {
            CheckIfIWon();
        }
    }


    public void NewGameDuration()
    {
        int startSeconds = ReferenceManager.refMngr.timeToSecondsHrMns(ReferenceManager.refMngr.tableStartTime, ':');
        int currentSeconds = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
        int gameSeconds = ReferenceManager.refMngr.timeToSecondsMnsScs(ReferenceManager.refMngr.gameDuration, ':');
        int secondsLeft = startSeconds + gameSeconds - currentSeconds;
        ReferenceManager.refMngr.SecondsToTime(secondsLeft);
        if (ReferenceManager.refMngr.hour != 0)
        {
            gameDuration.text = ReferenceManager.refMngr.hour.ToString() + "h:" + ReferenceManager.refMngr.minutes.ToString() + "m:" + ReferenceManager.refMngr.seconds.ToString() + "s";
        }
        else
        {
            gameDuration.text = ReferenceManager.refMngr.minutes.ToString() + "m:" + ReferenceManager.refMngr.seconds.ToString() + "s";
        }
        if (ReferenceManager.refMngr.hour > 0 || ReferenceManager.refMngr.minutes > 0 || ReferenceManager.refMngr.seconds > 0)
        {
            Invoke(nameof(NewGameDuration), 1);
        }
        else
        {
            gameDuration.text = "Game Finished";
            CheckIfIWon();
        }
    }

    public int startSeconds;

    public void NewGameDurationPrivate()
    {
        int currentSeconds = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
        int gameSeconds = 600;
        int secondsLeft = startSeconds + gameSeconds - currentSeconds;
        ReferenceManager.refMngr.SecondsToTime(secondsLeft);
        if (ReferenceManager.refMngr.hour != 0)
        {
            gameDuration.text = ReferenceManager.refMngr.hour.ToString() + "h:" + ReferenceManager.refMngr.minutes.ToString() + "m:" + ReferenceManager.refMngr.seconds.ToString() + "s";
        }
        else
        {
            gameDuration.text = ReferenceManager.refMngr.minutes.ToString() + "m:" + ReferenceManager.refMngr.seconds.ToString() + "s";
        }
        if (ReferenceManager.refMngr.hour > 0 || ReferenceManager.refMngr.minutes > 0 || ReferenceManager.refMngr.seconds > 0)
        {
            Invoke(nameof(NewGameDurationPrivate), 1);
        }
        else
        {
            gameDuration.text = "Game Finished";
            CheckIfIWon();
        }
    }


    bool checkedIfWon = false;

    void CheckIfIWon()
    {
        Debug.LogError("Check if i won");
        if (!checkedIfWon)
        {
            //if (GameManager.Instance.roomOwner)
            if (canPlayGame)
                PhotonNetwork.RaiseEvent((int)EnumPhoton.OnlineGameFinished, "true", true, null);
            canPlayGame = false;
            checkedIfWon = true;

            for (int i = 0; i < playerObjects.Count; i++)
            {
                for (int j = playerObjects.Count - 1; j > i; j--)
                {
                    if (int.Parse(playerObjects[i].dice.GetComponent<GameDiceController>().myScore.text) < int.Parse(playerObjects[j].dice.GetComponent<GameDiceController>().myScore.text))
                    {
                        PlayerObject tmp = playerObjects[i];
                        playerObjects[i] = playerObjects[j];
                        playerObjects[j] = tmp;
                    }
                }
            }

            for (int i = 0; i < playerObjects.Count; i++)
            {
                Debug.LogError("Check: " + playerObjects[i].name + ", POS: " + (i + 1));
                if (playerObjects[i].id == PhotonNetwork.player.NickName)
                {
                    SetFinishGameManually(playerObjects[i].id, true, i + 1);
                }
                else
                {
                    SetFinishGameManually(playerObjects[i].id, false, i + 1);
                }
            }
        }
    }

    bool canRunTime = true;

    public bool CanSynchronize = true;

    public bool stopIncreasingScore = false;

    public bool hasSynchronized = true;

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            canRunTime = false;
            CanSynchronize = true;
            hasSynchronized = false;
        }
        else
        {
            hasSynchronized = false;
            stopIncreasingScore = true;
            canRunTime = false;
            //PhotonNetwork.RaiseEvent((int)EnumPhoton.NeedDuration, "No Content", true, null);
            PhotonNetwork.RaiseEvent((int)EnumPhoton.NeedSynchronize, "No Content", true, null);
        }
    }

    void LoadPreviousGame(string key)
    {
        /*
        string jsonData = PlayerPrefs.GetString(StaticStrings.SaveGameString);
        Dictionary<string, string> std = PlayFab.SimpleJson.DeserializeObject<Dictionary<string, string>>(jsonData);
        string Value = std[key];
        GameData _GD = JsonUtility.FromJson<GameData>(Value);
        Debug.Log("_ GD  1111 " + _GD._PD_List.Count);
        Debug.Log("_ GD   " + _GD._PD_List[0].dicePos[1]);
        if (_GD.numberofPlayers == 2)
        {
            for (int rowIndex = 0; rowIndex < 2; rowIndex++)
            {
                PlayerData _PD = _GD._PD_List[rowIndex * 2];
                Debug.Log("_PD  " + _PD.playerIndex);
                for (int index = 0; index < 4; index++)
                {
                    Debug.Log("_ GD  pos   " + _PD.dicePos[index]);
                    playerObjects[_PD.playerIndex].pawns[index].GetComponent<LudoPawnController>().AddInstantly(_PD.dicePos[index]);
                }

            }
        }
        else
        {
            for (int rowIndex = 0; rowIndex < 4; rowIndex++)
            {
                PlayerData _PD = _GD._PD_List[rowIndex];

                for (int index = 0; index < 4; index++)
                {
                    Debug.Log("_ GD  pos   " + _PD.dicePos[index]);
                    playerObjects[_PD.playerIndex].pawns[index].GetComponent<LudoPawnController>().AddInstantly(_PD.dicePos[index]);
                }
            }
        }
        currentPlayerIndex = _GD.currentPlayer;
        for (int i = 0; i < playerObjects.Count; i++)
        {
            playerObjects[i].dice.GetComponent<GameDiceController>().EnableDiceShadow();
        }
        playerObjects[_GD.currentPlayer].dice.GetComponent<GameDiceController>().DisableDiceShadow();
        playerObjects[_GD.currentPlayer].dice.GetComponent<GameDiceController>().EnableShot();

        GameManager.Instance.currentPlayer = playerObjects[_GD.currentPlayer];
        Debug.Log("is Dice  " + _GD.isDiceRolled + "   dice  " + _GD.diceNumber);
        if (_GD.isDiceRolled)
            playerObjects[_GD.currentPlayer].dice.GetComponent<GameDiceController>().LoadPrevDice(_GD.currentPlayer, _GD.diceNumber);


        std.Remove(key);
        string newjsonData = PlayFab.SimpleJson.SerializeObject(std);
        PlayerPrefs.SetString(StaticStrings.SaveGameString, newjsonData);
        PlayerPrefs.SetString("PlaySavedGame", "null");
        */
    }


    public int GetCurrentPlayerIndex()
    {
        return currentPlayerIndex;
    }

    public void TIPButton()
    {
        if (TIPObject.activeSelf)
        {
            TIPObject.SetActive(false);
        }
        else
        {
            TIPObject.SetActive(true);
        }
    }

    public void FacebookShare()
    {

    }


    public void StopAndFinishGame()
    {
        Debug.LogError("Stop And Finish Game");
        StopTimers();

        SetFinishGame(PhotonNetwork.player.NickName, true);
        ShowGameFinishWindow();
    }

    public void ShareScreenShot()
    {

#if UNITY_ANDROID
        string text = StaticStrings.ShareScreenShotText;
        text = text + " " + "https://play.google.com/store/apps/details?id=" + StaticStrings.AndroidPackageName;
        ScreenShotController.GetComponent<NativeShare>().ShareScreenshotWithText(text);
#elif UNITY_IOS
        string text = StaticStrings.ShareScreenShotText;
        text = text + " " + "https://itunes.apple.com/us/app/apple-store/id" + StaticStrings.ITunesAppID;
        ScreenShotController.GetComponent<NativeShare> ().ShareScreenshotWithText (text);
#endif

    }

    public void ShowGameFinishWindow()
    {
        Debug.LogError("Show Game Finish Window");
        // AdsManager.Instance.adsScript.ShowAd(AdLocation.GameFinishWindow);
        FinishWindowActive = true;

        List<PlayerObject> otherPlayers = new List<PlayerObject>();

        for (int i = 0; i < playerObjects.Count; i++)
        {
            PlayerAvatarController controller = playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>();
            if (controller.Active && !controller.finished)
            {
                otherPlayers.Add(playerObjects[i]);
            }
        }
        float finalAmount = firstPlacePrize - (int)(firstPlacePrize * 0.10f);
        // Debug.Log(finalAmount);
        GameFinishWindow.GetComponent<GameFinishWindowController>().showWindow(playersFinished, otherPlayers, firstPlacePrize, secondPlacePrize);
    }

    public void ShowGameFinishedWindowManually()
    {
        Debug.LogError("ShowGameFinishedWindowManually");
        FinishWindowActive = true;
        GameFinishWindow.GetComponent<GameFinishWindowController>().showWindowManually(playersFinished);
    }

    private void ButtonClick(string id)
    {

        int index = 0;

        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id == id)
            {
                index = i;
                break;
            }
        }

        CurrentPlayerID = id;

        if (playerObjects[index].AvatarObject.GetComponent<PlayerAvatarController>().Active)
        {
            PlayerInfoWindow.GetComponent<PlayerInfoController>().ShowPlayerInfo(playerObjects[index].avatar, playerObjects[index].name, playerObjects[index].data);
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.LogError("Finish Game Disabled here");
            //FinishedGame();
        }
        //for (int i = 0; i < playerObjects.Count; i++)
        //{
        //    Debug.LogError("ID: " + playerObjects[i].id);
        //    Debug.LogError("NAME: " + playerObjects[i].name);
        //}
    }



    public void FinishedGame()
    {
        if (GameManager.Instance.currentPlayer.id == PhotonNetwork.player.NickName)
        {
            Debug.LogError("I Called");
            SetFinishGame(GameManager.Instance.currentPlayer.id, true);
        }
        else
        {
            Debug.LogError("I Called");
            SetFinishGame(GameManager.Instance.currentPlayer.id, false);
        }
        // SetFinishGame(PhotonNetwork.player.NickName, true);
    }

    public void FinishedPawnSoFinishGame()
    {
        int alive = 0;
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].finishedPawns != 4)
            {
                alive++;
            }
        }
        if (alive <= 1)
        {
            CheckIfIWon();
        }
    }

    private void SetFinishGame(string id, bool me)
    {
        if (!me || !iFinished)
        {
            Debug.Log("SET FINISH");
            ActivePlayersInRoom--;

            int index = GetPlayerPosition(id);

            playersFinished.Add(playerObjects[index]);

            PlayerAvatarController controller = playerObjects[index].AvatarObject.GetComponent<PlayerAvatarController>();
            controller.Name.GetComponent<Text>().text = "";
            controller.Active = false;
            controller.finished = true;

            playerObjects[index].dice.SetActive(false);

            int position = playersFinished.Count;
            //if (GameManager.Instance.type == MyGameType.TwoPlayer)
            //{
            //    if (didIWin) position = 1;
            //    else position = 2;
            //}
            //Debug.LogError("PSOITION: " + position + "ME: " + me + "iFinished: " + iFinished);
            if (position == 1)
            {
                controller.Crown.SetActive(true);
            }


            if (me)
            {
                PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeoutLong;
                iFinished = true;
                if (ActivePlayersInRoom >= 0)
                {
                    if (GameManager.Instance.type == MyGameType.Private)
                    {
                        PhotonNetwork.RaiseEvent((int)EnumPhoton.FinishedGame, PhotonNetwork.player.NickName, true, null);
                        Debug.Log("set finish call finish turn");
                        SendFinishTurn();
                    }
                }

                PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
                {
                    // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
                    Statistics = new List<StatisticUpdate> {
                    new StatisticUpdate { StatisticName = "MaxWin", Value = 1 },
                    }
                },
                result => { Debug.Log("User statistics updated"); },
                error => { Debug.LogError(error.GenerateErrorReport()); });

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add(MyPlayerData.GamesPlayedKey, (GameManager.Instance.myPlayerData.GetPlayedGamesCount() + 1).ToString());
                if (position == 1)
                {
                    WinSound.Play();
                    print("running");
                    if (GameManager.Instance.type == MyGameType.TwoPlayer)
                    {
                        Debug.LogError("Changed Amount here");
                        //int finalAmount = firstPlacePrize - (int)(firstPlacePrize * 0.10f);
                        float finalAmount = firstPlacePrize;
                        Debug.Log(finalAmount + " finishingvalue " + PlayerPrefs.GetInt("Finishing"));
                        if (PlayerPrefs.GetInt("Finishing") == 1)
                        {
                            GameManager.Instance.playfabManager.apiManager.AddCoins(finalAmount);
                        }
                    }

                    //data.Add(MyPlayerData.CoinsKey, (GameManager.Instance.myPlayerData.GetCoins() + firstPlacePrize).ToString());

                    else if (GameManager.Instance.type == MyGameType.TwoPlayer)
                    {
                        GameManager.Instance.twoWins++;
                        //   GameManager.Instance.playfabManager.apiManager.UpdatePlayerStats(2,1);
                        //  data.Add(MyPlayerData.TwoPlayerWinsKey, (GameManager.Instance.myPlayerData.GetTwoPlayerWins() + 1).ToString());
                    }
                    else if (GameManager.Instance.type == MyGameType.FourPlayer)
                    {
                        Debug.LogError("Amount Changed here");
                        //int finalAmount = firstPlacePrize - (int)(firstPlacePrize * 0.10f);
                        float finalAmount = firstPlacePrize;
                        Debug.Log(finalAmount);
                        if (PlayerPrefs.GetInt("Finishing") == 1)
                        {
                            GameManager.Instance.playfabManager.apiManager.AddCoins(finalAmount);
                        }
                    }
                    else if (GameManager.Instance.type == MyGameType.FourPlayer)
                    {
                        GameManager.Instance.fourWins++;
                        //GameManager.Instance.playfabManager.apiManager.UpdatePlayerStats(4,1);
                        // data.Add(MyPlayerData.FourPlayerWinsKey, (GameManager.Instance.myPlayerData.GetFourPlayerWins() + 1).ToString());
                    }
                    else if (GameManager.Instance.type == MyGameType.Private)
                    {
                        Debug.Log("Amount Changed here");
                        //int finalAmount = firstPlacePrize - (int)(firstPlacePrize * 0.10f);
                        float finalAmount = firstPlacePrize;
                        Debug.Log(finalAmount);
                        Debug.Log(finalAmount + " finishingvalue1 " + PlayerPrefs.GetInt("Finishing"));
                        if (PlayerPrefs.GetInt("Finishing") == 1)
                        {
                            GameManager.Instance.playfabManager.apiManager.AddCoins(finalAmount);
                        }
                    }
                    else if (GameManager.Instance.type == MyGameType.Private)
                    {
                        GameManager.Instance.fourWins++;
                        //GameManager.Instance.playfabManager.apiManager.UpdatePlayerStats(4,1);
                        // data.Add(MyPlayerData.FourPlayerWinsKey, (GameManager.Instance.myPlayerData.GetFourPlayerWins() + 1).ToString());
                    }
                    positionText.text = "Note: 10% service charges on win amount.";
                    stars.SetActive(true);
                    // GameManager.Instance.playfabManager.apiManager.UpdatePlayerStats();
                }

                else
                {
                    positionText.text = "You Lost";
                    stars.SetActive(false);
                    if (GameManager.Instance.type == MyGameType.TwoPlayer)
                    {
                        GameManager.Instance.twoWins++;
                        // GameManager.Instance.playfabManager.apiManager.UpdatePlayerStats(2,0);
                        // data.Add(MyPlayerData.TwoPlayerWinsKey, (GameManager.Instance.myPlayerData.GetTwoPlayerWins() + 1).ToString());
                    }
                    else if (GameManager.Instance.type == MyGameType.FourPlayer)
                    {
                        GameManager.Instance.fourWins++;
                        //  GameManager.Instance.playfabManager.apiManager.UpdatePlayerStats(4,0);
                        //  data.Add(MyPlayerData.FourPlayerWinsKey, (GameManager.Instance.myPlayerData.GetFourPlayerWins() + 1).ToString());
                    }
                }

                GameManager.Instance.myPlayerData.UpdateUserData(data);
            }
            else if (GameManager.Instance.currentPlayer.isBot)
            {
                SendFinishTurn();
            }

            controller.setPositionSprite(position);
            CheckPlayersIfShouldFinishGame();
        }
    }

    private void SetFinishGameManually(string id, bool me, int position)
    {
        Debug.LogWarning("Set Finish Game Manually");
        Debug.LogWarning("ME: " + me + "Psoition: " + position + "IFInished: " + iFinished);
        if (!me || !iFinished || true)
        {
            Debug.Log("SET FINISH Manually");
            ActivePlayersInRoom--;

            int index = GetPlayerPosition(id);

            playersFinished.Add(playerObjects[index]);

            playerObjects[index].myPosition = position;

            PlayerAvatarController controller = playerObjects[index].AvatarObject.GetComponent<PlayerAvatarController>();
            controller.Name.GetComponent<Text>().text = "";
            controller.Active = false;
            controller.finished = true;
            playerObjects[index].dice.SetActive(false);

            if (position == 1)
            {
                controller.Crown.SetActive(true);
            }

            if (me)
            {
                PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeoutLong;
                iFinished = true;

                PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
                {
                    // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
                    Statistics = new List<StatisticUpdate> {
                    new StatisticUpdate { StatisticName = "MaxWin", Value = 1 },
                    }
                },
                result => { Debug.Log("User statistics updated"); },
                error => { Debug.LogError(error.GenerateErrorReport()); });

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add(MyPlayerData.GamesPlayedKey, (GameManager.Instance.myPlayerData.GetPlayedGamesCount() + 1).ToString());

                if (GameManager.Instance.type == MyGameType.TwoPlayer)
                {
                    try
                    {
                        if (position == 1)
                        {
                            GameManager.Instance.playfabManager.apiManager.AddCoins(float.Parse(ReferenceManager.refMngr.firstPlacePrize));
                        }
                        else if (position == 2)
                        {
                            GameManager.Instance.playfabManager.apiManager.AddCoins(float.Parse(ReferenceManager.refMngr.secondPlacePrize));
                        }
                        else if (position == 3)
                        {
                            GameManager.Instance.playfabManager.apiManager.AddCoins(float.Parse(ReferenceManager.refMngr.thirdPlacePrize));
                        }
                    }
                    catch { }
                    //if (PlayerPrefs.GetInt("Finishing") == 1)
                    //{
                    //    GameManager.Instance.playfabManager.apiManager.AddCoins(finalAmount);
                    //}
                }

                if (position == 1)
                {
                    WinSound.Play();
                    print("running");
                    //if (GameManager.Instance.type == MyGameType.TwoPlayer)
                    //{
                    //    Debug.LogError("Changed Amount here");
                    //    //int finalAmount = firstPlacePrize - (int)(firstPlacePrize * 0.10f);
                    //    float finalAmount = firstPlacePrize;
                    //    Debug.Log(finalAmount + " finishingvalue " + PlayerPrefs.GetInt("Finishing"));
                    //    if (PlayerPrefs.GetInt("Finishing") == 1)
                    //    {
                    //        GameManager.Instance.playfabManager.apiManager.AddCoins(finalAmount);
                    //    }
                    //}
                    positionText.text = "Note: 10% service charges on win amount.";
                    stars.SetActive(true);
                }

                else
                {
                    positionText.text = "You Lost";
                    stars.SetActive(false);
                }

                positionText.text = "Your Position is: " + position.ToString();
                GameManager.Instance.myPlayerData.UpdateUserData(data);
            }
            //else if (GameManager.Instance.currentPlayer.isBot)
            //{
            //    SendFinishTurn();
            //}
            if (ActivePlayersInRoom == 0)
            {
                StopTimers();
                ShowGameFinishedWindowManually();
            }
            controller.setPositionSprite(position);
            //CheckPlayersIfShouldFinishGame();
        }
    }

    public int GetPlayerPosition(string id)
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id.Equals(id))
            {
                return i;
            }
        }
        return -1;
    }

    //bool canCallFinishAgain = true;

    UpdatePlayerTimer timer;
    //public void SendingFinishSlowly(UpdatePlayerTimer timr)
    //{
    //    //Debug.LogError("Slowly");
    //    if (canCallFinishAgain)
    //    {
    //        //Debug.LogError("Slowly Inside");
    //        canCallFinishAgain = false;
    //        timer = timr;
    //        Invoke(nameof(SendFinishTurnManually), 1.5f);
    //        Invoke(nameof(CanCallFinishAgainTrue), 5);
    //    }
    //}

    //void CanCallFinishAgainTrue()
    //{
    //    canCallFinishAgain = true;
    //}

    //public void SendFinishTurnManually()
    //{
    //    if (!TempGameManager.tempGM.iamalive)
    //    {
    //        //Debug.LogError("I am not alive");
    //        timer.Gamedice.RollDiceMAnually();
    //        Invoke(nameof(AutoMover), 1);
    //        //Invoke(nameof(TheEnd), 1.5f);
    //    }
    //}

    void AutoMover()
    {
        //Debug.LogError("Auto mover");
        //timer.PlayerAutoMove1();
    }
    public bool NextTurnSet = false;
    public void TheEnd()
    {
        NextTurnSet = true;
        //Debug.LogError("Sent Manually");
        //PhotonNetwork.RaiseEvent((int)EnumPhoton.NextPlayerTurn, currentPlayerIndex, true, null);

        //currentPlayerIndex = (myIndex + 1) % playerObjects.Count;

        //Debug.LogError("PLAYER BEFORE: " + currentPlayerIndex);

        setCurrentPlayerIndex(currentPlayerIndex);

        //Debug.LogError("PLAYER AFTER: " + currentPlayerIndex + " isbot: " + GameManager.Instance.currentPlayer.isBot);

        //Debug.LogError("My Index is : " + myIndex);
        SetTurn();
        //SetOpponentTurn();

        //GameManager.Instance.miniGame.setOpponentTurn();
        //Debug.LogError("Game Type: " + GameManager.Instance.type);
        //if (GameManager.Instance.type == MyGameType.TwoPlayer)
        //{
        //    Debug.LogError("1");
        //GameManager.Instance.miniGame.setOpponentTurn();
        //}
        //else
        //{
        //    Debug.LogError("2");
        //    //GameManager.Instance.miniGame.setMyTurn();
        //}
    }

    void CanCallFinishTurnAgain()
    {
        canCallFinish = true;
    }

    bool canCallFinish = true;

    public bool canSendOther = true;

    public void SendFinishTurn()
    {
        if (!canCallFinish) return;
        canCallFinish = false;
        Invoke(nameof(CanCallFinishTurnAgain), 1.2f);
        Debug.Log("Send Finish Turn");
        if (!FinishWindowActive && ActivePlayersInRoom > 1)
        {
            if (GameManager.Instance.currentPlayer.isBot)
            {
                BotDelay();
            }
            else
            {
                if (!GameManager.Instance.isLocalMultiplayer)
                {
                    if (GameManager.Instance.type == MyGameType.TwoPlayer)
                    {
                        string info = GetCurrentPlayerIndex(myIndex).ToString() + "," + playerObjects[currentPlayerIndex].timer.GetComponent<UpdatePlayerTimer>().turnCount.ToString() + "," + playerObjects[currentPlayerIndex].dice.GetComponent<GameDiceController>().myScore.text;
                        Debug.LogError("I sent");
                        PhotonNetwork.RaiseEvent((int)EnumPhoton.NextPlayerTurnWithName, info, true, null);
                        //PhotonNetwork.RaiseEvent((int)EnumPhoton.NextPlayerTurnWithName, GetCurrentPlayerIndex(myIndex), true, null);
                    }
                    else
                    {
                        PhotonNetwork.RaiseEvent((int)EnumPhoton.NextPlayerTurn, myIndex, true, null);
                    }
                }

                //currentPlayerIndex = (myIndex + 1) % playerObjects.Count;

                Debug.Log("PLAYER BEFORE: " + currentPlayerIndex);

                setCurrentPlayerIndex(myIndex);

                Debug.Log("PLAYER AFTER: " + currentPlayerIndex + " isbot: " + GameManager.Instance.currentPlayer.isBot);

                SetTurn();
                //SetOpponentTurn();

                GameManager.Instance.miniGame.setOpponentTurn();
            }
        }
    }

    public void SendFinishTurnOther()
    {
        if (!canCallFinish) return;
        canCallFinish = false;
        Invoke(nameof(CanCallFinishTurnAgain), 1.2f);
        Debug.Log("Send Finish Turn Other");
        if (!FinishWindowActive && ActivePlayersInRoom > 1)
        {
            if (GameManager.Instance.currentPlayer.isBot)
            {
                BotDelay();
            }
            else
            {
                if (!GameManager.Instance.isLocalMultiplayer)
                {
                    if (GameManager.Instance.type == MyGameType.TwoPlayer)
                    {
                        string info = GetCurrentPlayerIndex(currentPlayerIndex).ToString() + "," + playerObjects[currentPlayerIndex].timer.GetComponent<UpdatePlayerTimer>().turnCount.ToString() + "," + playerObjects[currentPlayerIndex].dice.GetComponent<GameDiceController>().myScore.text;
                        Debug.LogError("I sent");
                        PhotonNetwork.RaiseEvent((int)EnumPhoton.NextPlayerTurnWithName, info, true, null);
                        //PhotonNetwork.RaiseEvent((int)EnumPhoton.NextPlayerTurnWithName, GetCurrentPlayerIndex(currentPlayerIndex), true, null);
                    }
                }

                //currentPlayerIndex = (myIndex + 1) % playerObjects.Count;

                Debug.Log("PLAYER BEFORE: " + currentPlayerIndex);

                setCurrentPlayerIndex(currentPlayerIndex);

                SetTurn();
                //SetOpponentTurn();

                //GameManager.Instance.miniGame.setOpponentTurn();
            }
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        PhotonNetwork.OnEventCall += this.OnEvent;
    }

    void OnDestroy()
    {
        PhotonNetwork.OnEventCall -= this.OnEvent;
    }

    void SynchronizeScore(string dataa)
    {
        string[] dataPiece = dataa.Split(',');
        //currentPlayerIndex = int.Parse(dataPiece[0]);
        //GameManager.Instance.currentPlayer = playerObjects[currentPlayerIndex];
        //SetTurn();
        //gameDuration.text = dataPiece[1];
        SynchronizePlayerInfo(playerObjects[0], dataPiece, 2);
        SynchronizePlayerInfo(playerObjects[1], dataPiece, 18);
        if (playerObjects.Count > 2)
        {
            SynchronizePlayerInfo(playerObjects[2], dataPiece, 34);
            SynchronizePlayerInfo(playerObjects[3], dataPiece, 50);
        }
    }

    void SynchronizeData(string dataa)
    {
        string[] dataPiece = dataa.Split(',');
        currentPlayerIndex = int.Parse(dataPiece[0]);
        GameManager.Instance.currentPlayer = playerObjects[currentPlayerIndex];
        SetTurn();
        //gameDuration.text = dataPiece[1];
        SynchronizePlayerInfo(playerObjects[0], dataPiece, 2);
        SynchronizePlayerInfo(playerObjects[1], dataPiece, 18);
        if (playerObjects.Count > 2)
        {
            SynchronizePlayerInfo(playerObjects[2], dataPiece, 34);
            SynchronizePlayerInfo(playerObjects[3], dataPiece, 50);
        }
        //hasSynchronized = true;
        Invoke(nameof(Synchronize), 1);
        SetGameDuration(gameDuration.text, ':');
    }

    void Synchronize()
    {
        hasSynchronized = true;
    }

    void SynchronizePlayerInfo(PlayerObject obj, string[] data, int startIndex)
    {
        obj.timer.GetComponent<UpdatePlayerTimer>().SetOnlineTurnCountGraphic(int.Parse(data[startIndex]));
        obj.timer.SetActive(bool.Parse(data[startIndex + 2]));
        obj.timer.GetComponent<UpdatePlayerTimer>().timer.fillAmount = float.Parse(data[startIndex + 1]);
        if (int.Parse(data[startIndex]) >= 3)
        {
            obj.dice.GetComponent<GameDiceController>().score = -1;
            obj.dice.GetComponent<GameDiceController>().myScore.text = "-1";
            SetPlayerDisconnected(obj.id);
        }
        else
        {
            obj.dice.GetComponent<GameDiceController>().score = int.Parse(data[startIndex + 3]);
            obj.dice.GetComponent<GameDiceController>().myScore.text = data[startIndex + 3];
        }
        return;
        for (int i = 0; i < 4; i++)
        {
            obj.pawns[i].GetComponent<LudoPawnController>().mainInJoint = bool.Parse(data[0 + startIndex + 3 + (i * 3) + 1]);
            obj.pawns[i].GetComponent<LudoPawnController>().isOnBoard = bool.Parse(data[1 + startIndex + 3 + (i * 3) + 1]);
            int curPOS = obj.pawns[i].GetComponent<LudoPawnController>().currentPosition = int.Parse(data[2 + startIndex + 3 + (i * 3) + 1]);
            RectTransform[] path = obj.pawns[i].GetComponent<LudoPawnController>().path;
            if (curPOS >= 0)
                DavMaster.CopyRectTransform(path[curPOS], obj.pawns[i].GetComponent<RectTransform>());
        }
    }

    private void OnEvent(byte eventcode, object content, int senderid)
    {
        Debug.Log("received event: " + eventcode);
        if (GameManager.Instance.roomOwner)
        {
            if (!IsInvoking(nameof(SendSynchronizationOfScore))) Invoke(nameof(SendSynchronizationOfScore), 3);
        }
        if (eventcode == (int)EnumPhoton.NextPlayerTurn)
        {
            if (!FinishWindowActive)
            {
                if (!NextTurnSet)
                {
                    setCurrentPlayerIndex((int)content);

                    //if (playerObjects[currentPlayerIndex].id.Contains("_BOT"))
                    // {
                    //     BotTurn();
                    // }
                    // else
                    // {
                    SetTurn();
                    // }
                }
                else NextTurnSet = false;
            }
        }
        else if (eventcode == (int)EnumPhoton.NextPlayerTurnWithName)
        {
            if (!FinishWindowActive)
            {
                if (!NextTurnSet)
                {
                    canSendOther = false;
                    for (int i = 0; i < playerObjects.Count; i++)
                    {
                        playerObjects[i].timer.GetComponent<UpdatePlayerTimer>().CancelSlowInvoke();
                    }
                    //Debug.LogError("NExt player turn with name: " + playerObjects[(int)content]);
                    string[] tempInfo = content.ToString().Split(',');
                    Debug.LogError("I received");
                    playerObjects[currentPlayerIndex].timer.GetComponent<UpdatePlayerTimer>().SetOnlineTurnCountGraphic(int.Parse(tempInfo[1]));
                    //playerObjects[currentPlayerIndex].dice.GetComponent<GameDiceController>().score = int.Parse(tempInfo[2]);
                    //playerObjects[currentPlayerIndex].dice.GetComponent<GameDiceController>().myScore.text = tempInfo[2];
                    //playerObjects[currentPlayerIndex].
                    SetCurrentPlayerIndexDav(int.Parse(tempInfo[0]));

                    //SetCurrentPlayerIndexDav((int)content);

                    //if (playerObjects[currentPlayerIndex].id.Contains("_BOT"))
                    // {
                    //     BotTurn();
                    // }
                    // else
                    // {
                    SetTurn();
                    // }
                }
                else NextTurnSet = false;
            }
        }
        else if (eventcode == (int)EnumPhoton.SynchronizeTurn)
        {
            if (CanSynchronize)
            {
                Debug.LogError("I am synchronizing");
                Debug.LogError(content.ToString());
                CanSynchronize = false;
                SynchronizeData(content.ToString());
            }
            else
            {
                Debug.LogError("I am unable to synchronize");
                //Invoke(nameof(CansynchronizeLater), 4);
            }
        }
        else if (eventcode == (int)EnumPhoton.SynchronizeScore)
        {
            if (!GameManager.Instance.roomOwner)
            {
                SynchronizeScore(content.ToString());
            }
        }
        else if (eventcode == (int)EnumPhoton.SetDuration)
        {
            //gameDuration.text = content.ToString();
            SetGameDuration(gameDuration.text, ':');
        }
        else if (eventcode == (int)EnumPhoton.NeedDuration)
        {
            if (IsInvoking(nameof(SendDurationAgain))) CancelInvoke(nameof(SendDurationAgain));
            SendDurationAgain();
        }
        else if (eventcode == (int)EnumPhoton.NeedSynchronize)
        {
            Debug.LogError("I am sending Synchronize.");
            SendSynchronization();
        }
        else if (eventcode == (int)EnumPhoton.OnlineGameFinished)
        {
            canPlayGame = false;
            if (hasSynchronized)
            {
                CheckIfIWon();
            }
        }
        else if (eventcode == (int)EnumPhoton.SendChatMessage)
        {
            string[] message = ((string)content).Split(';');
            Debug.Log("Received message " + message[0] + " from " + message[1]);
            for (int i = 0; i < playerObjects.Count; i++)
            {
                if (playerObjects[i].id.Equals(message[1]))
                {
                    playerObjects[i].ChatBubbleText.SetActive(true);
                    playerObjects[i].ChatbubbleImage.SetActive(false);
                    playerObjects[i].ChatBubbleText.GetComponent<Text>().text = message[0];
                    playerObjects[i].ChatBubble.GetComponent<Animator>().Play("MessageBubbleAnimation");
                }
            }
        }
        else if (eventcode == (int)EnumPhoton.SendChatEmojiMessage)
        {
            string[] message = ((string)content).Split(';');
            Debug.Log("Received message " + message[0] + " from " + message[1]);
            for (int i = 0; i < playerObjects.Count; i++)
            {
                if (playerObjects[i].id.Equals(message[1]))
                {
                    playerObjects[i].ChatBubbleText.SetActive(false);
                    playerObjects[i].ChatbubbleImage.SetActive(true);
                    int index = int.Parse(message[0]);

                    if (index > emojiSprites.Length - 1)
                    {
                        index = emojiSprites.Length;
                    }
                    playerObjects[i].ChatbubbleImage.GetComponent<Image>().sprite = emojiSprites[index];
                    playerObjects[i].ChatBubble.GetComponent<Animator>().Play("MessageBubbleAnimation");
                }
            }
        }
        else if (eventcode == (int)EnumPhoton.AddFriend)
        {
            if (PlayerPrefs.GetInt(StaticStrings.FriendsRequestesKey, 0) == 0)
            {
                string[] data = ((string)content).Split(';');
                if (PhotonNetwork.playerName.Equals(data[2]))
                    invitiationDialog.GetComponent<PhotonChatListener2>().showInvitationDialog(data[0], data[1], null);
            }
            else
            {
                Debug.Log("Invitations OFF");
            }

        }
        else if (eventcode == (int)EnumPhoton.FinishedGame)
        {
            string message = (string)content;
            Debug.LogError("I Called");

            SetFinishGame(message, false);

        }
        else if (eventcode == (int)EnumPhoton.DisconnectPlayer)
        {
            string message = (string)content;
            SetPlayerDisconnectedReceived(message);
        }
    }

    void CansynchronizeLater()
    {
        CanSynchronize = true;
    }
    private void SetMyTurn()
    {
        GameManager.Instance.isMyTurn = true;

        if (GameManager.Instance.miniGame != null)
            GameManager.Instance.miniGame.setMyTurn();

        StartTimer();
    }

    private void BotTurn()
    {
        oppoTurnSource.Play();
        //GameManager.Instance.currentPlayer = playerObjects[currentPlayerIndex];
        GameManager.Instance.isMyTurn = false;
        Debug.Log("Bot Turn");
        StartTimer();
        if (!GameManager.Instance.isLocalMultiplayer)
        {
            GameManager.Instance.miniGame.BotTurn(true);
        }
        else
        {
            if (GameManager.Instance.isPlayingWithComputer)
            {
                GameManager.Instance.miniGame.BotTurn(true);
            }
            else
                playerObjects[currentPlayerIndex].dice.GetComponent<GameDiceController>().EnableOpponentLocal();
        }
        //Invoke("BotDelay", 2.0f);
    }

    private void SetTurn()
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            playerObjects[i].dice.GetComponent<GameDiceController>().EnableDiceShadow();
        }

        playerObjects[currentPlayerIndex].dice.GetComponent<GameDiceController>().DisableDiceShadow();

        GameManager.Instance.currentPlayer = playerObjects[currentPlayerIndex];

        Debug.Log("Current player set");
        Debug.Log("id   " + playerObjects[currentPlayerIndex].id + "   myid  " + myId + "   currentPlayerIndex   " + currentPlayerIndex);

        if (playerObjects[currentPlayerIndex].id == myId)
        {
            //Debug.LogError("my turn");
            SetMyTurn();
        }
        else if (playerObjects[currentPlayerIndex].isBot)
        {
            //Debug.LogError("bot turn");
            BotTurn();
        }
        else
        {
            //Debug.LogError("opponent turn");
            SetOpponentTurn();
        }
    }

    private void BotDelay()
    {
        if (!FinishWindowActive)
        {
            Debug.Log("Bot Delayy    ");
            setCurrentPlayerIndex(currentPlayerIndex);
            SetTurn();
        }

    }

    private void setCurrentPlayerIndex(int current)
    {

        while (true)
        {
            current = current + 1;
            currentPlayerIndex = (current) % playerObjects.Count;
            GameManager.Instance.currentPlayer = playerObjects[currentPlayerIndex];
            //GameManager.Instance.currentPlayerTurnIndex = currentPlayerIndex;
            if (playerObjects[currentPlayerIndex].AvatarObject.GetComponent<PlayerAvatarController>().Active) break;
        }

    }

    public int GetCurrentPlayerIndex(int current)
    {
        int returnInt = 0;
        while (true)
        {
            current = current + 1;
            returnInt = (current) % playerObjects.Count;
            //GameManager.Instance.currentPlayerTurnIndex = currentPlayerIndex;
            if (playerObjects[returnInt].AvatarObject.GetComponent<PlayerAvatarController>().Active)
            {
                return returnInt;
                break;
            }
        }
    }

    public void SetCurrentPlayerIndexDav(int index)
    {
        currentPlayerIndex = index;
        GameManager.Instance.currentPlayer = playerObjects[currentPlayerIndex];
        SetTurn();
    }

    private void SetOpponentTurn()
    {
        oppoTurnSource.Play();
        GameManager.Instance.isMyTurn = false;
        /*if (playerObjects[currentPlayerIndex].id.Contains("_BOT"))
        {
            BotTurn();
        }*/

        StartTimer();
    }

    private void StartTimer()
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (i == currentPlayerIndex)
            {
                playerObjects[currentPlayerIndex].timer.SetActive(true);
            }
            else
            {
                playerObjects[i].timer.SetActive(false);
            }
        }
    }

    public void StopTimers()
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            playerObjects[i].timer.SetActive(false);
        }
    }

    public void PauseTimers()
    {
        if (!GameManager.Instance.isLocalMultiplayer)
            playerObjects[currentPlayerIndex].timer.GetComponent<UpdatePlayerTimer>().Pause();
    }

    public void restartTimer()
    {
        if (!GameManager.Instance.isLocalMultiplayer)
            playerObjects[currentPlayerIndex].timer.GetComponent<UpdatePlayerTimer>().restartTimer();
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("Player disconnected: " + otherPlayer.NickName);

        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id.Equals(otherPlayer.NickName))
            {
                setPlayerDisconnected(i);
                if (!GameManager.Instance.isLocalMultiplayer)
                {
                    playerObjects[i].dice.GetComponent<GameDiceController>().score = -1;
                    playerObjects[i].dice.GetComponent<GameDiceController>().myScore.text = "-1";
                }
                break;
            }
        }
        CheckToFinishGame();
    }

    public void SetPlayerDisconnected(string id)
    {
        PhotonNetwork.RaiseEvent((int)EnumPhoton.DisconnectPlayer, id, true, null);
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id.Equals(id))
            {
                setPlayerDisconnected(i);
                if (!GameManager.Instance.isLocalMultiplayer)
                {
                    playerObjects[i].dice.GetComponent<GameDiceController>().score = -1;
                    playerObjects[i].dice.GetComponent<GameDiceController>().myScore.text = "-1";
                }
                break;
            }
        }
        CheckToFinishGame();
    }

    public void SetPlayerDisconnectedReceived(string id)
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id.Equals(id))
            {
                setPlayerDisconnected(i);
                if (!GameManager.Instance.isLocalMultiplayer)
                {
                    playerObjects[i].dice.GetComponent<GameDiceController>().score = -1;
                    playerObjects[i].dice.GetComponent<GameDiceController>().myScore.text = "-1";
                    playerObjects[i].timer.GetComponent<UpdatePlayerTimer>().SetOnlineTurnCountGraphic(3);
                }
                break;
            }
        }
        CheckToFinishGame();
    }


    public void CheckToFinishGame()
    {
        Debug.LogError("CheckToFinishGame");
        if (!GameManager.Instance.isLocalMultiplayer && GameManager.Instance.isPlayingWithComputer)
        {
            GameManager.exitedPlayers++;
            if (ReferenceManager.refMngr.onlineNoOfPlayer - GameManager.exitedPlayers == 1)
            {
                Debug.LogError("One Player Left...");
                CancelInvoke(nameof(UpdateGameDuration));
                StopTimers();
                CheckIfIWon();
            }
        }
        else
        {
            CheckPlayersIfShouldFinishGame();
        }
    }

    // public void CheckPlayersIfShouldFinishGame()
    // {
    //     if (!FinishWindowActive)
    //     {
    //         if ((ActivePlayersInRoom == 1 && !iFinished) || ActivePlayersInRoom == 1)
    //         {

    //             StopAndFinishGame();
    //         }

    //         if (iFinished && ActivePlayersInRoom == 1 && CheckIfOtherPlayerIsBot())
    //         {
    //             AddBotToListOfWinners();
    //             StopAndFinishGame();
    //         }
    //     }
    // }

    public void CheckPlayersIfShouldFinishGame()
    {
        Debug.LogError("CheckPlayersIfShouldFinishGame");
        if (!FinishWindowActive)
        {
            if ((ActivePlayersInRoom == 1 && !iFinished))
            {
                StopAndFinishGame();
                return;
            }

            if (ActivePlayersInRoom == 0)
            {
                StopAndFinishGame();
                return;
            }

            if (iFinished && ActivePlayersInRoom == 1 && CheckIfOtherPlayerIsBot())
            {
                AddBotToListOfWinners();
                StopAndFinishGame();
                return;
            }

            if (ActivePlayersInRoom > 1 && iFinished)
            {
                TIPButtonObject.SetActive(true);
            }

        }
    }
    public void AddBotToListOfWinners()
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id.Contains("_BOT") && playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().Active)
            {
                playersFinished.Add(playerObjects[i]);
            }
        }
    }

    public bool CheckIfOtherPlayerIsBot()
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id.Contains("_BOT") && playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().Active)
            {
                playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().finished = true;
                return true;
            }
        }
        return false;
    }

    public void setPlayerDisconnected(int i)
    {
        if (!FinishWindowActive)
        {
            if (!playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().finished)
                ActivePlayersInRoom--;

            Debug.Log("Active players: " + ActivePlayersInRoom);
            if (currentPlayerIndex == i && ActivePlayersInRoom > 1)
            {
                setCurrentPlayerIndex(currentPlayerIndex);
                SetTurn();
            }

            Debug.Log("za petla");
            playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().PlayerLeftRoom();

            // LUDO
            // LUDO
            playerObjects[i].dice.SetActive(false);
            if (!playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().finished)
            {
                for (int j = 0; j < playerObjects[i].pawns.Length; j++)
                {
                    // playerObjects[i].pawns[j].SetActive(false);
                    playerObjects[i].pawns[j].GetComponent<LudoPawnController>().GoToInitPosition(false);
                }
            }
            // END LUDO
        }
    }

    public void LeaveGame(bool finishWindow)
    {
        Debug.LogError("Leave Game");
        if (GameManager.Instance.isLocalMultiplayer && !GameManager.Instance.isPlayingWithComputer)
        {

            LoadmenuScene();
            // SavingWindow.SetActive(true);
        }
        else
        {
            if (!iFinished || finishWindow)
            {
                Debug.LogError("MAine Menu");
                loadingPanel.SetActive(true);
                PhotonNetwork.LeaveRoom();
                LoadmenuScene();
            }
            else
            {
                ShowGameFinishWindow();
            }
        }
        loadingPanel.SetActive(true);
    }

    public void ShowHideChatWindow()
    {
        if (!ChatWindow.activeSelf)
        {
            ChatWindow.SetActive(true);
            //ChatButton.GetComponent<Text>().text = "X";
        }
        else
        {
            ChatWindow.SetActive(false);
            // ChatButton.GetComponent<Text>().text = "CHAT";
        }
    }

    public void LoadmenuScene()
    {
        GameManager.Instance.playfabManager.roomOwner = false;
        GameManager.Instance.roomOwner = false;
        GameManager.Instance.resetAllData();
        loadingPanel.SetActive(false);
        SceneManager.LoadScene("MenuScene");

    }
    public void ToggleSound()
    {
        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            PlayerPrefs.SetInt("Muted", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Muted", 0);
        }
        SetSoundState();
    }
    private void SetSoundState()
    {
        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            AudioListener.volume = 1;
            audioOnIcon.SetActive(true);
            audioOffIcon.SetActive(false);
        }
        else
        {
            AudioListener.volume = 0;
            audioOnIcon.SetActive(false);
            audioOffIcon.SetActive(true);
        }
    }


    public void SaveGame()
    {
        /*
        bool value = playerObjects[currentPlayerIndex].dice.GetComponent<GameDiceController>().isDiceRolled;
        int steps = playerObjects[currentPlayerIndex].dice.GetComponent<GameDiceController>().steps;
        GameData _Gd = new GameData(GameManager.Instance.requiredPlayers, currentPlayerIndex, value, steps);
        foreach (MultiDimensionalGameObject _multi in PlayersPawns)
        {
            _Gd.AddPoistion(_multi.objectsArray);
        }
        string data = JsonUtility.ToJson(_Gd);
        Dictionary<string, string> st = new Dictionary<string, string>();
        string previousdata = PlayerPrefs.GetString(StaticStrings.SaveGameString, "null");
        if (!previousdata.Equals("null"))
        {
            st = PlayFab.SimpleJson.DeserializeObject<Dictionary<string, string>>(previousdata);
        }
        st[_Gd.Day + _Gd.Date + " " + _Gd.Time + _Gd.numberofPlayers] = data;
        string jsondata = PlayFab.SimpleJson.SerializeObject(st);
        PlayerPrefs.SetString(StaticStrings.SaveGameString, jsondata);
        Debug.Log("jsonData  " + jsondata);
        LoadmenuScene();
        */
    }
    public void OnBackbtnClick()
    {
        GameManager.Instance.isLocalMultiplayer = false;
        GameManager.Instance.gameSceneStarted = false;
        GameManager.Instance.opponentsIDs = new List<string>() { null, null, null };
        GameManager.Instance.opponentsAvatars = new List<Sprite>() { null, null, null };
        GameManager.Instance.opponentsNames = new List<string>() { null, null, null };
        SceneManager.LoadScene(1);
    }
    public void LoadGame(string key)
    {
        /*
        string jsonData = PlayerPrefs.GetString(StaticStrings.SaveGameString);
        Dictionary<string, string> std = new Dictionary<string, string>();
        std = PlayFab.SimpleJson.DeserializeObject<Dictionary<string, string>>(jsonData);
        Debug.Log("converData   " + std[key]);
        GameData _Gd_new;
        _Gd_new = JsonUtility.FromJson<GameData>(std[key]);
        Debug.Log("Time   " + _Gd_new.Time + "   players  " + _Gd_new.diceNumber + "   Date   " + _Gd_new.Date);
        */
    }
    IEnumerator pic()
    {
        string url = GameManager.apiBase1 + "client_details/my_referral_code=" + GameManager.Instance.userID;
        Debug.Log(url);
        WWW www = new WWW(url);

        yield return www;
        JsonData jsonvale = JsonMapper.ToObject(www.text);
        string playerImageUrl = jsonvale["result_push"][0]["profile_pic"].ToString();
        if (jsonvale["result_push"][0]["profile_pic"].ToString() != "0")
        {
            pImage.gameObject.SetActive(true);
            playerImageUrl = jsonvale["result_push"][0]["profile_pic"].ToString();
            UnityWebRequest unityWebRequest3 = UnityWebRequest.Get(playerImageUrl);
            yield return unityWebRequest3.SendWebRequest();
            byte[] bytes3 = unityWebRequest3.downloadHandler.data;
            Texture2D tex3 = new Texture2D(2, 2);
            tex3.LoadImage(bytes3);
            Sprite playerimage = Sprite.Create(tex3, new Rect(0.0f, 0.0f, tex3.width, tex3.height), new Vector2(0.5f, 0.5f), 100.0f);
            pImage.texture = playerimage.texture;

        }
        else
        {
            pImage.gameObject.SetActive(false);
        }
    }

    #region AUDioSOurce
    [Header("audio source")]
    public AudioSource StarAudioSource;
    public void StarSound()
    {
        if (StarAudioSource != null)
            StarAudioSource.Play();
    }

    #endregion
}