using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class MassegeItems
{
	public GameObject massegeCanvas;
	public Text header;
	public Text massege;
	public Button closeButton;

	public GameObject popupCanvas;
	public Text popupText;
}
[System.Serializable]
public class TheItemsInThisGame
{
	public bool
		hasCoin, hasDiemonds, hasHighScore, hasLevels, hasLife,hasLevelProgress,showLevelWinStars;

}
[System.Serializable]
public class MyButtons
{
	public Button
		playButton, levelButton, levelCloseButton, settingCloseButton, pauseButton, resumeButton;
	public Button[] reTryButtons, homeButtons, musicButtons, soundButtons, vibrateButtons, settingButtons;

}
[System.Serializable]
public class GameEvents
{
	public UnityEvent OnPlayEvent;
	public UnityEvent OnPauseEvent;
	public UnityEvent OnResumeEvent;
	public UnityEvent OnGameoverEvent;
	public UnityEvent OnGameWinEvent;
	public UnityEvent OnReTryEvent;
	public UnityEvent OnCloseLevelPanelEvent;
}
[System.Serializable]
public class GameItems
{
	public GameObject gameWinStars_parent;
	public GameObject[] gameWinStars;
	public Text[] coinTexts;
	public Text[] diemondTexts;
	public Text[] levelNo;
	public Text preveLevelNo;
}

public class GameMaster : MonoBehaviour
{
	public enum GamePlayMode
    {
		None,HighScore,Levels,Difficulty
    }
	public enum GameMode
	{
		Easy, Medium, Hard, EndLess
	}	
	public GamePlayMode gamePlayMode;
	public static GameMode gameMode;
	public MyButtons gameButtons;
	[SerializeField] GameEvents gameEvents;
	[SerializeField] TheItemsInThisGame theItemsInThisGame;
	public GameItems gameItems;
	[SerializeField]MassegeItems massegeItems;

	[Header ("Level Progress UI")]
	//sceneOffset: because you may add other scenes like (Main menu, ...)
	//[SerializeField] int sceneOffset;
	[SerializeField] Text nextLevelText;
	[SerializeField] Text currentLevelText;
	[SerializeField] Image progressFillImage;

	[Space]
	[SerializeField] Text levelCompletedText;

	[Space]
	//white fading panel at the start
	[SerializeField] Image fadePanel;

	[SerializeField] List<GameObject> UIObjects;	
	//[SerializeField] GameObject back;	
	[SerializeField] GameObject transition;	
	public GameObject gameover_warnPanel;	
	//[SerializeField] PlayerHealth playerHealth;
	//[SerializeField]GameController_Grappling gameController;
	[SerializeField] GameObject massegeEffect;
	[SerializeField]Text sorry_you_failed;	
	[SerializeField] Animator coinAnim;
	[SerializeField] Animator diemondAnim;

	#region Singleton class: UIManager

	public static GameMaster Instance;
	[SerializeField] GameObject confetti;
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		OffAllUIObjects(false);		
		//StartCoroutine(ActiveBack(true, 0));
		Game.achivedLevelTarget = 0;
		CheckForGameItems();
        if (confetti) { confetti.SetActive(false); }
	}
	void CheckForGameItems()
    {
		
		//if(gamePlayMode != GamePlayMode.Levels)
		//{
		//	currentLevelText.gameObject.SetActive(false);
		//	nextLevelText.gameObject.SetActive(false); ;
		//}
		//if(!theItemsInThisGame.hasCoin)
		//{			
		//	GameObject coin = UIObject(Game.HUD).transform.Find("top/coin").gameObject;
		//	coin.SetActive(false);
		//}
		//if (!theItemsInThisGame.hasDiemonds)
		//{			
		//	GameObject diemond = UIObject(Game.HUD).transform.Find("top/diemond").gameObject;
		//	diemond.SetActive(false);
		//}		
	}
	#endregion
	public IEnumerator ActiveBack(bool value,float wait)
    {
		yield return new WaitForSeconds(wait);
		//back.SetActive(value);
    }

	void Start ()
	{
		massegeItems.closeButton.onClick.AddListener(CloseMassegeCanvas);
		//reset progress value
		//progressFillImage.fillAmount = 0f;
		GetButtonsAndApplyFuntions();
		CloseMassegeCanvas();
		//UpdateLevelStatus();
		if (Game.retryCount == 0) { ShowUI(Game.Menu); }		
	    //StartCoroutine(MusicManager.PlayMusic("menu",2));	
	
	}
	public void StartGame()
    {
		ShowUI(Game.Level);
		UIObject(Game.Levels_Panel).SetActive(false);
		MusicManager.PlaySfx("button");
		Game.playNextLevel = false;
		if (gameItems.levelNo.Length > 0)
		{
			for (int i = 0; i < gameItems.levelNo.Length; i++)
			{
				gameItems.levelNo[i].text = Game.CurrentLevel.ToString();

			}
		}
		int pLevel = Game.CurrentLevel - 1;
		gameItems.preveLevelNo.text = pLevel.ToString();
		ActAddingCoin();
		ActAddingDiemond();
	}
    private void Update()
    {
		if (Game.gameStatus == Game.GameStatus.isPlaying)
		{
			Game.levelPlayTime += Time.deltaTime;
		}
	}
    #region ASSINING BUTTONS
    void GetButtonsAndApplyFuntions()
    {
        if (gameButtons.playButton) { gameButtons.playButton.onClick.AddListener(OnPlay); }
		if (gameButtons.pauseButton) { gameButtons.pauseButton.onClick.AddListener(OnPause); }
		if (gameButtons.levelButton) { gameButtons.levelButton.onClick.AddListener(OpenLevelPanel); }
		if (gameButtons.levelCloseButton) { gameButtons.levelCloseButton.onClick.AddListener(CloseLevelPanel); }
        if (gameButtons.settingButtons.Length>0)
        {
            for (int i = 0; i < gameButtons.settingButtons.Length; i++)
            {
				gameButtons.settingButtons[i].onClick.AddListener(OpenSettingPanel);
			}
        }		
		if (gameButtons.reTryButtons.Length > 0)
		{
			for (int i = 0; i < gameButtons.reTryButtons.Length; i++)
			{
				gameButtons.reTryButtons[i].onClick.AddListener(RetryLevel);
			}
		}
		if (gameButtons.homeButtons.Length > 0)
		{
			for (int i = 0; i < gameButtons.homeButtons.Length; i++)
			{
				gameButtons.homeButtons[i].onClick.AddListener(GoHome);
			}
		}		
		if (gameButtons.settingCloseButton) { gameButtons.settingCloseButton.onClick.AddListener(CloseSettingPanel); }
		
	}
	 void OnPlay()
    {
		gameEvents.OnPlayEvent.Invoke();

	}
	public void ShowLevelPanel()
	{
		ShowUI(Game.Level);
	}
	public void CloseLevelPanel()
	{
		gameEvents.OnCloseLevelPanelEvent.Invoke();
	}

	void GoHome()
	{
		Level_Loader.instance.LoadLevel("MainMenu");
	}
	void OpenLevelPanel()
	{
		ShowUI(Game.Level);	
	}
	void OpenSettingPanel()
    {
		UIObject(Game.SettingPanel).SetActive(true);
		Game.gameStatus = Game.GameStatus.isPaused;
    }
	void CloseSettingPanel()
	{
		UIObject(Game.SettingPanel).SetActive(false);
		//Game.gameStatus = Game.GameStatus.isPlaying;
	}
	public void RetryLevel()
	{
		Game.retryCount++;
		Game.playNextLevel = false;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	
	void OnResume()
	{
		Game.gameStatus = Game.GameStatus.isPlaying;
		ShowUI(Game.HUD);
		MusicManager.PlayMusic("music");
		MenuManager.Instance.GameState = true;
		gameEvents.OnResumeEvent.Invoke();
		//StartCoroutine(ActiveBack(false, 1));
	}
	void OnPause()
	{
		Game.gameStatus = Game.GameStatus.isPaused;
		ShowUI(Game.Pause);
		MusicManager.PauseMusic(0.2f);
		MenuManager.Instance.GameState = false;
		//StartCoroutine(ActiveBack(true, 0.7f));
		StartCoroutine(Pause());
		gameEvents.OnPauseEvent.Invoke();
	}
	IEnumerator Pause()
	{
		yield return new WaitForSeconds(1.2F);
		Button resumeButton = UIObject(Game.Pause).transform.Find("panel/RESUME").GetComponent<Button>();
		resumeButton.onClick.AddListener(OnResume);
		StartCoroutine(RequestIntAd(2f));
	}

	#endregion ASSINING BUTTONS
	public void ShowPopup(string massege)
    {
		StartCoroutine(ShowPopup_(massege));
    }
	IEnumerator ShowPopup_(string massege)
    {
		massegeItems.popupText.text = string.Empty;
		massegeItems.popupCanvas.SetActive(true);
		massegeItems.popupText.text = massege;
		yield return new WaitForSeconds(5f);
		massegeItems.popupText.text = string.Empty;
		massegeItems.popupCanvas.SetActive(false);
	}
	void CloseMassegeCanvas()
    {
		massegeItems.massegeCanvas.SetActive(false);
	}
	public void ShowMassege(string header,string massege,bool value)
    {		
		massegeItems.massegeCanvas.SetActive(true);
		massegeItems.header.text = header;
		massegeItems.massege.text = massege;
        if (value == false) { massegeEffect.SetActive(false); } else { massegeEffect.SetActive(true); }
	}
	void OffAllUIObjects(bool value)
	{
		for (int i = 0; i < UIObjects.Count; i++)
		{
			UIObjects[i].SetActive(value);
		}
	}
	void PlayButtonClip()
    {
		MusicManager.PlaySfx("button");		
    }

	public GameObject UIObject(string name)
	{
		int objectIndex = UIObjects.FindIndex(gameObject => string.Equals(name, gameObject.name));
		return UIObjects[objectIndex];
	}
	IEnumerator WaitAndShow(GameObject object_,bool value, float wait)
	{
		yield return new WaitForSeconds(wait);
		object_.SetActive(value);
	}
	public void WaitAndShowUI(float wait, string uiName)
	{
		waitTime = wait;
		StartCoroutine(ContineuShowUI(uiName));
	}
	float waitTime;
	public void ShowUI(string uiName)
	{
		StartCoroutine(ContineuShowUI(uiName));
	}
	IEnumerator PlayTransition()
	{
		//SoundManager.PlaySfx("transition");
		transition.SetActive(false);
		yield return new WaitForSeconds(0.1f);
		transition.SetActive(true);
		yield return new WaitForSeconds(2f);
		transition.SetActive(false);

	}
	IEnumerator ContineuShowUI(string uiName)
	{
		yield return new WaitForSeconds(waitTime);
		//SoundManager.PlaySfx("button");
		StartCoroutine(PlayTransition());
		yield return new WaitForSeconds(1f);
		OffAllUIObjects(false);
		UIObject(uiName).SetActive(true);
		waitTime = 0;
		Button[] allButtons = FindObjectsOfType<Button>();
        if (allButtons.Length > 0)
        {
            for (int i = 0; i < allButtons.Length; i++)
            {
				allButtons[i].onClick.AddListener(PlayButtonClip);
			}
        }
		//AdmobAdmanager.Instance.ShowInterstitial();
	}

	//--------------------------------------
	public void ShowLevelCompletedUI ()
	{
		//fade in the text (from 0 to 1) with 0.6 seconds
		//levelCompletedText.DOFade (1f, .6f).From (0f);
	}

	public void Fade ()
	{
		//fade out the panel (from 1 to 0) with 1.3 seconds
		//fadePanel.DOFade (0f, 1.3f).From (1f);
	}
	
	public void UpdateLevelStatus()
	{

		Debug.Log("UpdateLevelStatus"+ Game.achivedLevelTarget + Game.currentLevelTarget);
		float val = 1f - ((float)Game.achivedLevelTarget / Game.currentLevelTarget);
		//transition fill (0.4 seconds)
		progressFillImage.DOFillAmount(val, .4f);

		if (gamePlayMode == GamePlayMode.Levels&&theItemsInThisGame.hasLevelProgress)
        {
			currentLevelText.text = Game.CurrentLevel.ToString();
			int nextLevel = Game.CurrentLevel + 1;
			nextLevelText.text = nextLevel.ToString();
        }
        else
        {
			currentLevelText.gameObject.SetActive(false);	
		    nextLevelText.gameObject.SetActive(false); ;
        }
		

		//items.levelFill.fillAmount = Mathf.Clamp(kill / levelItemCount*1, 0, 1f);

	}
	
	public void OnGameover()
    {
		MusicManager.PlaySfx_Other("gameOver_start");
		Game.retryCount = 0;
		Game.gameStatus = Game.GameStatus.isGameover;
		ShowUI(Game.Gameover);
		StartCoroutine(Gameover());
		MusicManager.PauseMusic(0.2f);
		gameEvents.OnGameoverEvent.Invoke();
	}
	IEnumerator Gameover()
    {		
		yield return new WaitForSeconds(0.2f);
		//StartCoroutine(ActiveBack(true, 0));		
		
		Text header = UIObject(Game.Gameover).transform.Find("header").GetComponent<Text>();
		#region COIN DIEMOND
		if (theItemsInThisGame.hasCoin)
        {
			Text coin_num = UIObject(Game.Gameover).transform.Find("coin_num").GetComponent<Text>();
			coin_num.text = Game.TotalCoins.ToString();			
        }
        else
        {
			Text coin_num = UIObject(Game.Gameover).transform.Find("coin_num").GetComponent<Text>();
			coin_num.gameObject.SetActive(false);
			GameObject coin = UIObject(Game.Gameover).transform.Find("coin").gameObject;
			coin.SetActive(false);
		}
		if (theItemsInThisGame.hasDiemonds)
        {
			Text diemond_num = UIObject(Game.Gameover).transform.Find("diemond_num").GetComponent<Text>();
			diemond_num.text = Game.TotalDiemonds.ToString();
		}
        else
        {
			Text diemond_num = UIObject(Game.Gameover).transform.Find("diemond_num").GetComponent<Text>();
			diemond_num.gameObject.SetActive(false);
			GameObject diemond = UIObject(Game.Gameover).transform.Find("diemond").gameObject;
			diemond.SetActive(false);
		}
		#endregion COIN DIEMOND

		if (gamePlayMode == GamePlayMode.HighScore)
		{
			Text High_Score_num = UIObject(Game.Gameover).transform.Find("High_Score_num").GetComponent<Text>();
			High_Score_num.text = Game.HighScore.ToString();
			Text current_Score_num = UIObject(Game.Gameover).transform.Find("current_Score_num").GetComponent<Text>();
			current_Score_num.text = Game.currentScore.ToString();
			if (Game.currentScore > Game.HighScore)
			{
				header.text = "New Score";
				Game.HighScore = Game.currentScore;
				MusicManager.PlaySfx("new_score");
			}
			else
			{
				header.text = "Gameover";
				MusicManager.PlaySfx("gameover");
			}
		}
        else if (gamePlayMode == GamePlayMode.Difficulty)
		{
			Text High_Score_text = UIObject(Game.Gameover).transform.Find("High_Score_text").GetComponent<Text>();
			High_Score_text.text = "Level Target";
			Text current_Score = UIObject(Game.Gameover).transform.Find("current_Score").GetComponent<Text>();
			current_Score.text = "Current Score";
			Text High_Score_num = UIObject(Game.Gameover).transform.Find("High_Score_num").GetComponent<Text>();
			High_Score_num.text = Game.currentLevelTarget.ToString();
			Text current_Score_num = UIObject(Game.Gameover).transform.Find("current_Score_num").GetComponent<Text>();
			current_Score_num.text = Game.achivedLevelTarget.ToString();
		}
		else if (gamePlayMode == GamePlayMode.Levels)
		{
			Text High_Score_num = UIObject(Game.Gameover).transform.Find("High_Score_num").GetComponent<Text>();
			High_Score_num.gameObject.SetActive(false);
			Text current_Score_num = UIObject(Game.Gameover).transform.Find("current_Score_num").GetComponent<Text>();
			current_Score_num.gameObject.SetActive(false);
			sorry_you_failed.gameObject.SetActive(true);
		}
		StartCoroutine( MusicManager.WaitAndPlaySfx_Other("gameOver",1.5f));
		StartCoroutine(RequestIntAd(2f));
	}
	IEnumerator RequestIntAd(float after)
    {
		yield return new WaitForSeconds(after);
		AdmobAdmanager.Instance.ShowInterstitial();
	}
	public void ShowGameOver_Warn()
    {
		gameover_warnPanel.SetActive(true);
		Game.gameStatus = Game.GameStatus.isPaused;
		MusicManager.PauseMusic(0.2f);
	}
	public void GoForGameOver()
    {
		gameover_warnPanel.SetActive(false);
		OnGameover();
	}
	public void MakeResumeTheGame()
	{
		//playerHealth.gameObject.SetActive(true);
		//playerHealth.ResetPlayer();
		gameover_warnPanel.SetActive(false);		
		MusicManager.UnpauseMusic();
	}
	public void OnGameWon()
	{
		Game.retryCount = 0;
		if (confetti) { confetti.SetActive(true); }
		MusicManager.PauseMusic(0.2f);
		//StartCoroutine(ActiveBack(true, 1.5f));
		WaitAndShowUI(2.0f, Game.GameWin);
		StartCoroutine(LevelWon());			
	}
	IEnumerator LevelWon()
	{
		yield return new WaitForSeconds(.2f);
        for (int i = 0; i < gameItems.gameWinStars.Length; i++)
        {
			gameItems.gameWinStars[i].SetActive(false);

		}
		
		#region COIN DIEMOND
		if (theItemsInThisGame.hasCoin)
		{
			Text coin_num = UIObject(Game.GameWin).transform.Find("coin_num").GetComponent<Text>();
			coin_num.text = Game.TotalCoins.ToString();
		}
		else
		{
			Text coin_num = UIObject(Game.GameWin).transform.Find("coin_num").GetComponent<Text>();
			coin_num.gameObject.SetActive(false);
			GameObject coin = UIObject(Game.GameWin).transform.Find("coin").gameObject;
			coin.SetActive(false);
		}
		if (theItemsInThisGame.hasDiemonds)
		{
			Text diemond_num = UIObject(Game.GameWin).transform.Find("diemond_num").GetComponent<Text>();
			diemond_num.text = Game.TotalDiemonds.ToString();
		}
		else
		{
			Text diemond_num = UIObject(Game.GameWin).transform.Find("diemond_num").GetComponent<Text>();
			diemond_num.gameObject.SetActive(false);
			GameObject diemond = UIObject(Game.GameWin).transform.Find("diemond").gameObject;
			diemond.SetActive(false);
		}
        #endregion COIN DIEMOND
		Text levelno = UIObject(Game.GameWin).transform.Find("level no").GetComponent<Text>();
		levelno.text = Game.CurrentLevel.ToString();
		if (theItemsInThisGame.showLevelWinStars)
        {
			gameItems.gameWinStars_parent.SetActive(true);
			if (Game.levelPlayTime > Game.levelMaxTime)
            {
				gameItems.gameWinStars[0].SetActive(true);
            }
			else if (Game.levelPlayTime < Game.levelMaxTime)
			{
				gameItems.gameWinStars[0].SetActive(true);
				StartCoroutine(WaitAndShow(gameItems.gameWinStars[1],true,1));
			}
			else if (Game.levelPlayTime < Game.levelMinTime)
			{
				gameItems.gameWinStars[0].SetActive(true);
				StartCoroutine(WaitAndShow(gameItems.gameWinStars[1], true, 1));
				StartCoroutine(WaitAndShow(gameItems.gameWinStars[2], true, 1));
			}
		}
        else
        {
			gameItems.gameWinStars_parent.SetActive(false);
        }
		gameEvents.OnGameWinEvent.Invoke();
		//if (confetti) { confetti.SetActive(false); }
		StartCoroutine(MusicManager.WaitAndPlaySfx_Other("win", 3f));
	}
	
	void OnEnable()
	{		
		SceneManager_New.onSceneLoaded += OnSceneLoaded;
	}

	// called second
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (Game.retryCount > 0)
		{
			Game.currentLevelTarget = 10 + LevelTarget(Game.CurrentLevel);
			PlayerManager.Instance.SetGameDifficulty(gameMode);
			ShowUI(Game.HUD);			
			if (gameItems.levelNo.Length > 0)
			{
				for (int i = 0; i < gameItems.levelNo.Length; i++)
				{
					gameItems.levelNo[i].text = Game.CurrentLevel.ToString();

				}
			}
			int pLevel = Game.CurrentLevel - 1;
			gameItems.preveLevelNo.text = pLevel.ToString();
			PlayerManager.Instance.GetCurrentEnvItems();
			gameEvents.OnReTryEvent.Invoke();
		}
		if (Game.playNextLevel)
        {
			Game.CurrentLevel++;
			Game.currentLevelTarget = 10 + LevelTarget(Game.CurrentLevel);
			PlayerManager.Instance.SetGameDifficulty(gameMode);
			ShowUI(Game.HUD);
            if (gameItems.levelNo.Length > 0)
            {
                for (int i = 0; i < gameItems.levelNo.Length; i++)
                {
					gameItems.levelNo[i].text = Game.CurrentLevel.ToString();

				}
            }
			int pLevel = Game.CurrentLevel - 1;
			gameItems.preveLevelNo.text = pLevel.ToString();
			PlayerManager.Instance.GetCurrentEnvItems();
		}
		//Debug.Log("OnSceneLoaded: " + scene.name);
		//Debug.Log(mode);
		
	}
	IEnumerator Retry()
	{

		yield return new WaitForSeconds(0.01f);
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		
	}
	// called when the game is terminated
	void OnDisable()
	{		
		SceneManager_New.onSceneLoaded -= OnSceneLoaded;
	}
    #region SHOP
    public void ShowShop()
    {
		ShowUI(Game.shop);
    }
	public void CloseShop()
	{
		ShowUI(Game.Menu);
	}
	public void PurchaseAkMag()
    {
        if (Game.TotalCoins >= Game.akBulletPrice)
        {
			Game.TotalCoins -= Game.akBulletPrice;
			Game.AKBullet += 27;
			ShowMassege
				("hurray"
				, "You Purchased AK47 23 Bullets",false);
        }
        else
        {
			ShowMassege
				("Oh No!"
				, "You Don't Have Enough Credits", false);
		}
    }
	public void PurchaseRifleMag()
	{
		if (Game.TotalCoins >= Game.rifleBulletPrice)
		{
			Game.TotalCoins -= Game.rifleBulletPrice;
			Game.RifleBullet += 23;
			ShowMassege
				("hurray"
				, "You Purchased Rifle 23 Bullets", false);
		}
		else
		{
			ShowMassege
				("Oh No!"
				, "You Don't Have Enough Credits", false);
		}
	}
	public void PurchasePistolMag()
	{
		if (Game.TotalCoins >= Game.pistolBulletPrice)
		{
			Game.TotalCoins -= Game.pistolBulletPrice;
			Game.PistolBullet += 18;
			ShowMassege
				("hurray"
				, "You Purchased Pistol 18 Bullets", false);
		}
		else
		{
			ShowMassege
				("Oh No!"
				, "You Don't Have Enough Credits", false);
		}
	}
	public void PurchaseLife()
	{
		if (Game.TotalCoins >= Game.lifePrice)
		{
			Game.TotalCoins -= Game.lifePrice;
			Game.Life += 1;
			ShowMassege
				("hurray"
				, "You Purchased 1 Life", false);
		}
		else
		{
			ShowMassege
				("Oh No!"
				, "You Don't Have Enough Credits", false);
	
		}
	}
    #endregion
    public void RewardTheUser()
    {
		Game.TotalCoins += 100;
		ShowMassege
				("Congratulation"
				, "You Won 100 Credits", true);
	}
	public void RewardTheUser_Half()
	{
		Game.TotalCoins += 50;
		ShowMassege
				("Congratulation"
				, "You Won 50 Credits", true);
	}

	public void WarnAdClosed()
	{
		ShowMassege
				("Sorry"
				, "You Closed The AD So You Will Not Get Any Credits", false);
	}
  //  private void OnDisable()
  //  {
		//Game.retryCount = 0;
  //  }
	public void ActivateGameDifficulty(int index)
    {
        switch (index)
        {
			case 1:
				gameMode = GameMode.Easy;
				PlayerManager.Instance.SetGameDifficulty(GameMode.Easy);
				UIObject(Game.Levels_Panel).SetActive(true);
				break;
			case 2:
				gameMode = GameMode.Medium;
				PlayerManager.Instance.SetGameDifficulty(GameMode.Medium);
				UIObject(Game.Levels_Panel).SetActive(true);
				break;
			case 3:
				gameMode = GameMode.Hard;
				PlayerManager.Instance.SetGameDifficulty(GameMode.Hard);
				UIObject(Game.Levels_Panel).SetActive(true);
				break;
			case 4:
				gameMode = GameMode.EndLess;
				PlayerManager.Instance.SetGameDifficulty(GameMode.EndLess);
				Game.currentLevelTarget = 500;
				ShowUI(Game.HUD);
				break;
		}
    }
	int levelTargeMultiplier;
	int LevelTarget(int lIndex)
    {
        switch (lIndex)
        {
			case 1:
				levelTargeMultiplier = 5;
				break;
			case 2:
				levelTargeMultiplier = 10;
				break;
			case 3:
				levelTargeMultiplier = 15;
				break;
			case 4:
				levelTargeMultiplier = 20;
				break;
			case 5:
				levelTargeMultiplier = 25;
				break;
			case 6:
				levelTargeMultiplier = 30;
				break;
			case 7:
				levelTargeMultiplier = 45;
				break;
			case 8:
				levelTargeMultiplier = 70;
				break;
			case 9:
				levelTargeMultiplier = 95;
				break;
			case 10:
				levelTargeMultiplier = 140;
				break;
		}
		return levelTargeMultiplier;

	}
	public void ActivateLevel(int lIndex)
    {
		Game.currentLevelTarget = 10 + LevelTarget(lIndex);
		Game.CurrentLevel=lIndex;
		ShowUI(Game.HUD);
		MusicManager.PlaySfx("button");
		PlayerManager.Instance.GetCurrentEnvItems();
	}
	public void BackFroLevel()
    {
		UIObject(Game.Levels_Panel).SetActive(false);
		MusicManager.PlaySfx("button");
	}
	public void ActAddingCoin()
    {
		coinAnim.SetTrigger("blink");
        if (gameItems.coinTexts.Length > 0)
        {
            for (int i = 0; i < gameItems.coinTexts.Length; i++)
            {
				gameItems.coinTexts[i].text = Game.TotalCoins.ToString();

			}
        }
    }
	public void ActAddingDiemond()
	{
		diemondAnim.SetTrigger("blink");
		if (gameItems.diemondTexts.Length > 0)
		{
			for (int i = 0; i < gameItems.diemondTexts.Length; i++)
			{
				gameItems.diemondTexts[i].text = Game.TotalDiemonds.ToString();

			}
		}
	}
	public void PlayNextLevel()
    {
		Game.retryCount = 0;
		Game.playNextLevel = true;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	public void OpenEnvironentPanel()
    {
		UIObject(Game.Environent).SetActive(true);
    }
	public void CloseEnvironentPanel()
	{
		UIObject(Game.Environent).SetActive(false);
	}
	public void GoToMenu()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
