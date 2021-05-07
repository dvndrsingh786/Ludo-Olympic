using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.Chat;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
#if UNITY_ANDROID || UNITY_IOS
using UnityEngine.Advertisements;
#endif
using AssemblyCSharp;
using Random = System.Random;
using LitJson;
using TMPro;
public class InitMenuScript : MonoBehaviour
{
    public GameObject FacebookLinkReward;
    public GameObject rewardDialogText;
    public GameObject FacebookLinkButton;
    public Text playerName;
    public GameObject videoRewardText;
    public GameObject playerAvatar;
    public RawImage playerAvatarDav;
    public GameObject fbFriendsMenu;
    public GameObject matchPlayer;
    public GameObject backButtonMatchPlayers;
    public GameObject MatchPlayersCanvas;
    public GameObject menuCanvas;
    public GameObject tablesCanvas;
    public GameObject gameTitle;
    public GameObject changeDialog;
    public GameObject inputNewName;
    public GameObject tooShortText;
    public Text coinsText;
    public Text newName;
    public Text offlineCoinText;
    
   // public Text UpiText;
    public GameObject coinsTextShop;
    public GameObject coinsTab;
    public GameObject TheMillButton;
    public GameObject dialog;
    // Use this for initialization
    public GameObject GameConfigurationScreen;
    public GameObject FourPlayerMenuButton;

    public GameObject storePanel;

    public GameObject ScalingAnimator;
    public GameObject offerPanel;
    public GameObject sharePopup;

    public GameObject audioOnIcon;
    public GameObject audioOffIcon;

    [Header("UI Attribute")]

    public InputField paytm;
    public InputField bankName;
    public InputField ifscCode;
    public InputField bankAccount;
    public InputField upiId;
   

    [Header("New Atrribute")]

    public Text referalcode;

    [Header("Refferal Attribute")]

    string refferalCodeURL;
    public InputField mobileNumber;
    public Text error;
    public RawImage pImage;
    public RawImage pImage2;

    [Header("New Objects")]
    public GameObject redeemPanel;

    string status;

    private string results;
    public string Results
    {
        get
        {
            return results;
        }
    }

    public List<Betting> twoPlayerBetting = new List<Betting>();
    public List<Betting> fourPlayerBetting = new List<Betting>();

    public AudioSource[] soundEffects;

    public GameObject prizeDistributionPopUp;
    public Text rupeeText;
    public Button onlineGamePlayButton;

    void Start()
    {
        GameManager.Instance.playfabManager.apiManager.joinedOnlineOnTime = false;
        refferalCodeURL = GameManager.apiBase1 + "share-code";
        if (PlayerPrefs.HasKey("Logintoken"))
        {
            referalcode.text = GameManager.friendrefferalCode;
            coinsText.text = GameManager.Instance.coinsCount.ToString();
            offlineCoinText.text = GameManager.offlineAmount;

            // Reedem get Coins...!!
           if(GameManager.paytmNumber!=null)
                paytm.text = GameManager.paytmNumber.ToString();
            if (GameManager.bankName != null)
                bankName.text = GameManager.bankName;
            if (GameManager.bankIfscCode != null)
                ifscCode.text = GameManager.bankIfscCode;
            if (GameManager.accountNumber != null)
                bankAccount.text = GameManager.accountNumber;
            if (GameManager.upiID != null)
                upiId.text = GameManager.upiID;

            if (GameManager.profileImge)
            {
                playerAvatarDav.texture = GameManager.profileImge.texture;
            }

            Debug.Log(GameManager.playerName);
        }
        //offerPanel.SetActive(true);
        StaticStrings.isFourPlayerModeEnabled = true;
        StartCoroutine(pic());
        //if (PlayerPrefs.GetInt(StaticStrings.SoundsKey, 0) == 0)
        //{
        //    AudioListener.volume = 1;
        //}
        //else
        //{
        //    AudioListener.volume = 0;
        //}
        //if (PlayerPrefs.GetInt("Muted") == 1)
        //{
        //    AudioListener.volume = 0;
        //}
        //if (PlayerPrefs.GetInt("Muted") == 0)
        //{
        //    AudioListener.volume = 1;
        //}
        if (PlayerPrefs.GetInt("IsSoundEffect", 1) == 1)
        {
            SetEffectToMute(false);
        }
        else
        {
            SetEffectToMute(true);
        }
        SetSoundState();

        FacebookLinkReward.GetComponent<Text>().text = "+ " + StaticStrings.CoinsForLinkToFacebook;
        playerName.GetComponent<Text>().text = GameManager.playerName;
        coinsText.GetComponent<Text>().text = GameManager.Instance.coinsCount.ToString();
      //  playerAvatar.GetComponent<RawImage>().texture =  GameManager.Instance.playfabManager.staticGameVariables.avatars[UnityEngine.Random.Range(0,22)].texture;
        //GameManager.Instance.playfabManager.apiManager.Betting();
        Debug.LogWarning("GET Betting disabled here");
        if (!StaticStrings.isFourPlayerModeEnabled)
        {
            FourPlayerMenuButton.SetActive(false);
        }

        GameManager.Instance.FacebookLinkButton = FacebookLinkButton;

        GameManager.Instance.dialog = dialog;
        videoRewardText.GetComponent<Text>().text = "+" + StaticStrings.rewardForVideoAd;
        GameManager.Instance.tablesCanvas = tablesCanvas;
        GameManager.Instance.facebookFriendsMenu = fbFriendsMenu.GetComponent<FacebookFriendsMenu>(); ;
        GameManager.Instance.matchPlayerObject = matchPlayer;
        GameManager.Instance.backButtonMatchPlayers = backButtonMatchPlayers;
        playerName.GetComponent<Text>().text = GameManager.Instance.nameMy;
        GameManager.Instance.MatchPlayersCanvas = MatchPlayersCanvas;

        if (PlayerPrefs.GetString("LoggedType").Equals("Facebook"))
        {
            FacebookLinkButton.SetActive(false);
        }

        if (GameManager.Instance.avatarMy != null) {
           // playerAvatar.GetComponent<RawImage>().texture = GameManager.Instance.avatarMy.texture;
        }
        
        GameManager.Instance.myAvatarGameObject = playerAvatar;
        GameManager.Instance.myNameGameObject = playerName.gameObject;

        GameManager.Instance.coinsTextMenu = coinsText.gameObject;
        GameManager.Instance.coinsTextShop = coinsTextShop;
        GameManager.Instance.initMenuScript = this;

        if (StaticStrings.hideCoinsTabInShop)
        {
            coinsTab.SetActive(false);
        }

#if UNITY_WEBGL
        coinsTab.SetActive (false);
#endif

        rewardDialogText.GetComponent<Text>().text = "1 Video = " + StaticStrings.rewardForVideoAd + " Coins";
        //coinsText.GetComponent<Text>().text = GameManager.Instance.myPlayerData.GetCoins() + "";
        GameManager.Instance.playfabManager.splashCanvas.SetActive(false);
        Debug.Log("Load ad menu");
        // AdsManager.Instance.adsScript.ShowAd(AdLocation.GameStart);
        //  GameManager.Instance.playfabManager.apiManager.OnRequestPlans();
        NewGameManager newGameManager = FindObjectOfType<NewGameManager>();
        //Debug.LogError("Hiden");
        newGameManager.newLoginScreen.SetActive(false);
        newGameManager.mobileVerificationScreen.SetActive(false);
        newGameManager.EnterYourPinScreen.SetActive(false);
        UIFlowHandler.uihandler.loadingPanel.SetActive(false);
        //GameManager.Instance.playfabManager.apiManager.isFirstTimeLogin = true;
        if (GameManager.Instance.playfabManager.apiManager.isFirstTimeLogin)
        {
            GameManager.Instance.playfabManager.apiManager.newUpdateProfilePage.SetActive(true);
        }
    }

    public void OpenPrizeDistributionPopup(bool isFourr, string entryFeee, string playerCountt, string firstt, string secondd, string thirdd)
    {
        PrizeDistributionInfo info = prizeDistributionPopUp.GetComponent<PrizeDistributionInfo>();
        info.thirdPrize.SetActive(isFourr);
        info.fourthPrize.SetActive(isFourr);

        info.entryFee.text = rupeeText.text + entryFeee;
        info.playerCount.text = "Player(" + playerCountt + ")";
        info.firstPrizeText.text = rupeeText.text + firstt;
        info.secondPrizeText.text = rupeeText.text + secondd;
        if(isFourr) info.thirdPrizeText.text = rupeeText.text + thirdd;
        prizeDistributionPopUp.SetActive(true);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void LinkToFacebook()
    {
        GameManager.Instance.facebookManager.FBLinkAccount();
    }

    public void LoginBtn()
    {
        GameManager.Instance.playfabManager.apiManager.LoginPanel.SetActive(true);
        GameManager.Instance.playfabManager.apiManager.loginBtn.enabled = true;
    }
    public void Register()
    {
        GameManager.Instance.playfabManager.apiManager.SignupPanel.SetActive(true);
        GameManager.Instance.playfabManager.apiManager.signupBtn.enabled = true;
    }
    public void OpenStore()
    {
        if (!GameManager.Instance.userID.Contains("Guest"))
        {
           storePanel.SetActive(true);
        }
        else
        {
            GameManager.Instance.playfabManager.apiManager.LoginPanel.SetActive(true);
        }
    }
    
    public void OpenPlayerStats()
    {
        GameManager.Instance.playfabManager.apiManager.PlayerStats();
    }
    
    public void OpenPaymentHistory()
    {
        GameManager.Instance.playfabManager.histroyScript.OnPaymentHistory();
    }

    public void OnWalletBtn()
    {
        GameManager.Instance.playfabManager.apiManager.walletPanel.SetActive(true);
    }
    public void LogOut()
    {
        Debug.LogError("Init menu Logout");
        GameManager.Instance.playfabManager.apiManager.LogOut();
    }

    public void OnKyc()
    {
        GameManager.Instance.playfabManager.apiManager.kycPanel.SetActive(true);
    }
    public void OnNotificationBtn()
    {
        GameManager.Instance.playfabManager.apiManager.OnBtn();
    }

    public void playerProfile()
    {
        GameManager.Instance.playfabManager.apiManager.OnMyProfile();
    }
    public void ShowGameConfiguration(int index)
    {
        // switch (index)
        // {
        //     case 0:
        //         GameManager.Instance.type = MyGameType.TwoPlayer;
        //         break;
        //     case 1:
        //         GameManager.Instance.type = MyGameType.FourPlayer;
        //         break;
        //     case 2:
        //         GameManager.Instance.type = MyGameType.Private;
        //         break;
        // }
        GameConfigurationScreen.SetActive(true);
        //AdsManager.Instance.adsScript.ShowAd(AdLocation.GamePropertiesWindow);
    }

    public void SetPrivateFourPlayer(bool value)
    {
        StaticStrings.isFourPlayerModeEnabled = value;
    }

    [SerializeField] Transform private2PlayerTransform, private4PlayerTransform;
    public void SetColorsOfNoOfPlayers(int NoOfPlayers)
    {
        if (NoOfPlayers == 2)
        {
            private2PlayerTransform.GetComponent<Image>().color = Color.yellow;
            private2PlayerTransform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.yellow;
            private4PlayerTransform.GetComponent<Image>().color = Color.white;
            private4PlayerTransform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        else
        {
            private4PlayerTransform.GetComponent<Image>().color = Color.yellow;
            private4PlayerTransform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.yellow;
            private2PlayerTransform.GetComponent<Image>().color = Color.white;
            private2PlayerTransform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        }
    }

    [SerializeField] Toggle local2PRadio, local4PRadio;
    public void SetNoOfPlayers(int index)
    {
        indexxx = index;
        StartCoroutine(TestRoutine());
        return;
        switch (index)
        {
            case 0:
                GameManager.Instance.type = MyGameType.TwoPlayer;
                GameManager.Instance.currentBettingIndex = 0;
                try { GameManager.Instance.currentBetting = twoPlayerBetting[GameManager.Instance.currentBettingIndex]; }
                catch { Debug.LogError("Catch has been called"); }
                try { GameManager.Instance.currentBetAmount = float.Parse(twoPlayerBetting[GameManager.Instance.currentBettingIndex].bettingValue.ToString()); }
                catch { Debug.LogError("Catch has been called"); }
                try { GameManager.Instance.currentWinningAmount = float.Parse(twoPlayerBetting[GameManager.Instance.currentBettingIndex].winningAmount.ToString()); }
                catch { Debug.LogError("Catch has been called"); }
                GameManager.Instance.payoutCoins = GameManager.Instance.currentWinningAmount;
                //local2PRadio.isOn = true;
                //local4PRadio.isOn = false;
                break;
             case 1:
                GameManager.Instance.type = MyGameType.FourPlayer;
                GameManager.Instance.currentBettingIndex = 0;
               GameManager.Instance.currentBetting = fourPlayerBetting[GameManager.Instance.currentBettingIndex];
                GameManager.Instance.currentBetAmount =float.Parse(fourPlayerBetting[GameManager.Instance.currentBettingIndex].bettingValue.ToString());
                GameManager.Instance.currentWinningAmount =float.Parse(fourPlayerBetting[GameManager.Instance.currentBettingIndex].winningAmount.ToString());
                GameManager.Instance.payoutCoins = GameManager.Instance.currentWinningAmount;
                //local2PRadio.isOn = false;
                //local4PRadio.isOn = true;
                Debug.Log("index "+GameManager.Instance.currentBetAmount);
                Debug.Log("index "+GameManager.Instance.currentWinningAmount);
                break;
        }
      //  ShowBettingText();
    }

    public int indexxx;

    IEnumerator TestRoutine()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        switch (indexxx)
        {
            case 0:
                GameManager.Instance.type = MyGameType.TwoPlayer;
                GameManager.Instance.currentBettingIndex = 0;
                print(GameManager.Instance.currentBettingIndex + "index value");
                try {  GameManager.Instance.currentBetting = GameManager.Instance.initMenuScript.twoPlayerBetting[GameManager.Instance.currentBettingIndex]; }
                catch { Debug.LogError("Catch has been called"); }
                try { GameManager.Instance.currentBetAmount = float.Parse(GameManager.Instance.initMenuScript.twoPlayerBetting[GameManager.Instance.currentBettingIndex].bettingValue.ToString()); }
                catch { Debug.LogError("Catch has been called"); }
                try { GameManager.Instance.currentWinningAmount = float.Parse(GameManager.Instance.initMenuScript.twoPlayerBetting[GameManager.Instance.currentBettingIndex].winningAmount.ToString()); }
                catch { Debug.LogError("Catch has been called"); }
                GameManager.Instance.payoutCoins = GameManager.Instance.currentWinningAmount;
                FindObjectOfType<BetDataScript>().OnPlan(true);
                break;
            case 1:
                GameManager.Instance.type = MyGameType.FourPlayer;
                GameManager.Instance.currentBettingIndex = 0;
                GameManager.Instance.currentBetting = GameManager.Instance.initMenuScript.fourPlayerBetting[GameManager.Instance.currentBettingIndex];
                GameManager.Instance.currentBetAmount = float.Parse(GameManager.Instance.initMenuScript.fourPlayerBetting[GameManager.Instance.currentBettingIndex].bettingValue.ToString());

                GameManager.Instance.currentWinningAmount = float.Parse(GameManager.Instance.initMenuScript.fourPlayerBetting[GameManager.Instance.currentBettingIndex].winningAmount.ToString());
                GameManager.Instance.payoutCoins = GameManager.Instance.currentWinningAmount;
                Debug.Log("index " + GameManager.Instance.currentBetAmount);
                Debug.Log("index " + GameManager.Instance.currentWinningAmount);
                FindObjectOfType<BetDataScript>().OnPlan(false);
                break;
        }
    }


    public void SetColor(int index)
    {
        switch (index)
        {
            case 0:
                GameManager.Instance.isColorSelected = (int)MyPawnColor.Blue;
                break;
            case 1:
                GameManager.Instance.isColorSelected = (int)MyPawnColor.Red;
                break;
            case 2:
                GameManager.Instance.isColorSelected = (int)MyPawnColor.Green;
                break;
            case 3:
                GameManager.Instance.isColorSelected = (int)MyPawnColor.Yellow;
                break;
        }
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
    public void SetSoundState()
    {
        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            //AudioListener.volume = 1;
            Camera.main.GetComponent<AudioSource>().volume = 1;
            //audioOnIcon.SetActive(true);
            //audioOffIcon.SetActive(false);
        }
        else
        {
            Camera.main.GetComponent<AudioSource>().volume = 0;
            //AudioListener.volume = 0;
            //audioOnIcon.SetActive(false);
            //audioOffIcon.SetActive(true);
        }
    }

    UIFlowHandler flowHandler;
    public void AddPanelToOpenedPanelsList(GameObject objectToAdd)
    {
        if (!flowHandler) flowHandler = FindObjectOfType<UIFlowHandler>();
        if (!objectToAdd.GetComponent<PanelScriptMandatory>())
        {
            flowHandler.openedPanels.Add(objectToAdd);
        }
    }

    public void SetEffectToMute(bool state)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            soundEffects[i].mute = state;
        }
    }

    public void TakeScreenshot()
    {
        ScreenCapture.CaptureScreenshot("TestScreenshot.png");
    }

    // Update is called once per frame
    void Update()
    {
        playerName.text = GameManager.playerName;
       // coinsText.text = GameManager.Instance.coinsCount.ToString();
    }

    public void showAdStore()
    {
        // AdsManager.Instance.adsScript.ShowAd(AdLocation.StoreWindow);
    }

    public void backToMenuFromTableSelect()
    {
        GameManager.Instance.offlineMode = false;
        tablesCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        gameTitle.SetActive(true);
    }

    public void showSelectTableScene(bool challengeFriend)
    {
        if (!challengeFriend)
            GameManager.Instance.inviteFriendActivated = false;

        // AdsManager.Instance.adsScript.ShowAd(AdLocation.GameStart);
        if (GameManager.Instance.offlineMode)
        {
            TheMillButton.SetActive(false);
        }
        else
        {
            TheMillButton.SetActive(true);
        }
        menuCanvas.SetActive(false);
        tablesCanvas.SetActive(true);
        gameTitle.SetActive(false);
    }

    public void playOffline()
    {
        //GameManager.Instance.tableNumber = 0;
        GameManager.Instance.offlineMode = true;
        GameManager.Instance.roomOwner = true;
        // showSelectTableScene(false);
        GameManager.Instance.playfabManager.PlayofflineMode();
        // SceneManager.LoadScene("GameScene");
    }

    public void PlayLocalPlayer()
    {
        GameManager.Instance.playfabManager.PlayOfflineWithReal();
    }

    public void switchUser()
    {
        GameManager.Instance.playfabManager.destroy();
        GameManager.Instance.facebookManager.destroy();
        GameManager.Instance.connectionLost.destroy();

      //  GameManager.Instance.avatarMy = null;
        PhotonNetwork.Disconnect();

        PlayerPrefs.DeleteAll();
        GameManager.Instance.resetAllData();
        LocalNotification.ClearNotifications();
        //GameManager.Instance.myPlayerData.GetCoins() = 0;
        SceneManager.LoadScene("LoginSplash");
    }

    public void showChangeDialog()
    {
        changeDialog.SetActive(true);
    }

    public void changeUserName()
    {
        Debug.Log("Change Nickname");

    }
    public void OnExitbtn()
    {
        Application.Quit();
    }
    public void startQuickGame()
    {
        GameManager.Instance.facebookManager.startRandomGame();
    }

    public void startQuickGameTableNumer(int tableNumer, int fee)
    {
     //   GameManager.Instance.payoutCoins = fee;
        GameManager.Instance.tableNumber = tableNumer;
        GameManager.Instance.facebookManager.startRandomGame();
    }

    public void showFacebookFriends()
    {
        // AdsManager.Instance.adsScript.ShowAd(AdLocation.FacebookFriends);
      //  GameManager.Instance.playfabManager.GetPlayfabFriends();
    }

    public void setTableNumber()
    {
        GameManager.Instance.tableNumber = Int32.Parse(GameObject.Find("TextTableNumber").GetComponent<Text>().text);
    }

    public void ShareMyCode()
    {
        ///GameManager.friendrefferalCode;
        ///string shareSubject = "Game Link";
        string abc = " invited you to play Ludo Olympics - First ever real money ludo game. Enter referral code '"; //'U7AZ47'
        string def = " & get real 2 % direct income every time when they play by using your referral code.";
        //string shareMessage = "Download Game from this"+ GameManager.friendrefferalCode+"code";
        string shareMessage = GameManager.playerName + abc + GameManager.friendrefferalCode + def;
        NativeShare share = new NativeShare();
        share.Share(shareMessage, null, null, "Share Via");
    }

    public void ShareLink()
    {
        if (!Application.isEditor)
        {
            string shareSubject = "Game Link";
            string abc = " invited you to play Ludo Olympics - First ever real money ludo game.Enter referral code '"; //'U7AZ47'
            string def = "' & get real 2 % direct income every time when they play by using your referral code.";
            //string shareMessage = "Download Game from this"+ GameManager.friendrefferalCode+"code";
            string shareMessage = GameManager.Instance.nameMy + abc + GameManager.friendrefferalCode + def;
            //Create intent for action send
            AndroidJavaClass intentClass =
                new AndroidJavaClass ("android.content.Intent");
            AndroidJavaObject intentObject =
                new AndroidJavaObject ("android.content.Intent");
            intentObject.Call<AndroidJavaObject>
                ("setAction", intentClass.GetStatic<string> ("ACTION_SEND"));
 
            //put text and subject extra
            intentObject.Call<AndroidJavaObject> ("setType", "text/plain");
            intentObject.Call<AndroidJavaObject>
                ("putExtra", intentClass.GetStatic<string> ("EXTRA_SUBJECT"), shareSubject);
            intentObject.Call<AndroidJavaObject>
                ("putExtra", intentClass.GetStatic<string> ("EXTRA_TEXT"), shareMessage);
 
            //call createChooser method of activity class
            AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity =
                unity.GetStatic<AndroidJavaObject> ("currentActivity");
            AndroidJavaObject chooser =
                intentClass.CallStatic<AndroidJavaObject>
                    ("createChooser", intentObject, "Share your text");
            currentActivity.Call ("startActivity", chooser);
            
        }
       
    }

    IEnumerator pic()
    {
        string url = GameManager.apiBase1 + "client_details/my_referral_code=" + GameManager.Instance.userID;
        Debug.Log(url);
        WWW www = new WWW(url);

        yield return www;
        JsonData jsonvale = JsonMapper.ToObject(www.text);
        Debug.Log("TestData");
        print(jsonvale["result_push"][0]["profile_pic"].ToString()+"Dbase");
        if (jsonvale["result_push"][0]["profile_pic"].ToString() != "")
        {
            Debug.Log("TestData1");
            pImage.gameObject.SetActive(true);
            pImage2.gameObject.SetActive(true);
          string playerImageUrl = jsonvale["result_push"][0]["profile_pic"].ToString();
            UnityWebRequest unityWebRequest3 = UnityWebRequest.Get(playerImageUrl);
            yield return unityWebRequest3.SendWebRequest();
            byte[] bytes3 = unityWebRequest3.downloadHandler.data;
            Texture2D tex3 = new Texture2D(2, 2);
            tex3.LoadImage(bytes3); Debug.Log("TestData2");
            Sprite playerimage = Sprite.Create(tex3, new Rect(0.0f, 0.0f, tex3.width, tex3.height), new Vector2(0.5f, 0.5f), 100.0f);
            pImage.texture = playerimage.texture;
            pImage2.texture = playerimage.texture;
            playerAvatarDav.texture = playerimage.texture;
            Debug.Log("TestData3");
        }
        else {
            pImage.gameObject.SetActive(false);
            pImage2.gameObject.SetActive(false);
            try
            {
                playerAvatarDav.texture = GameManager.profileImge.texture;
            }
            catch { }
        }
        GameManager.playerName = jsonvale["result_push"][0]["fullname"].ToString();
        newName.text = GameManager.playerName;
        Debug.Log(GameManager.playerName);
    }

   
        public void ShowRewardedAd()
        {
#if UNITY_ANDROID || UNITY_IOS
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
#endif
        }

#if UNITY_ANDROID || UNITY_IOS
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                GameManager.Instance.playfabManager.addCoinsRequest(StaticStrings.rewardForVideoAd);
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
#endif

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            ReCalculateTablesTime();
        }
    }

    void ReCalculateTablesTime()
    {
        BetDataScript script = FindObjectOfType<BetDataScript>();
        for (int i = 0; i < script.betdataPublic.childCount; i++)
        {
            script.betdataPublic.GetChild(i).GetComponent<BetScript>().RecalculateTime();
        }
    }

    public void OFfVibration(int number)
    {
        PlayerPrefs.SetInt(StaticStrings.SoundsKey, number);
    }

    public void OnAboutUs()
    {
        Application.OpenURL("https://ludocashwin.com/about-us/");
    }

    public void OnPrivacyPolicy()
    {
        Application.OpenURL("https://ludocashwin.com/privacy-policy/");
    }

    public void OnTermsandConditions()
    {
        Application.OpenURL("https://ludocashwin.com/terms-and-conditions/");
    }
    public void SetFinish(int num)
    {
        PlayerPrefs.SetInt("Finishing", num);
    }

    #region New Functions

    public void OpenPrivacyPolicy()
    {
        GameManager.Instance.playfabManager.apiManager.newPrivacyPolicy.SetActive(true);
    }

    public void OpenSettings()
    {
        GameManager.Instance.playfabManager.apiManager.newOptionsPanel.SetActive(true);
    }

    public void OpenProfilePage()
    {
        GameManager.Instance.playfabManager.apiManager.OpenProfilePage();
    }

    public void OpenRules()
    {
        GameManager.Instance.playfabManager.apiManager.newRules.SetActive(true);
    }

    public void OpenNotifications()
    {
        GameManager.Instance.playfabManager.apiManager.newNotificationsPanel.SetActive(true);
    }

    public void OpenAddMoney()
    {
        GameManager.Instance.playfabManager.apiManager.newAddMoney.SetActive(true);
    }

    public GameObject newReferAFriendPanel;

    #endregion
}
