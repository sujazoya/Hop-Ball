using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class MyUIItems
{
    public Text[]   countText;
    public Animator countAniator;
    public Image   levelImage;
    public GameObject[] starsIn;
    public GameObject[] starsOut;
}

public class MenuManager : MonoBehaviour
{
    public MyUIItems items;
    public static MenuManager Instance;
    public bool GameState;
    public GameObject menuElement;
    Transform currentStages;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        menuElement.SetActive(true);
        for (int i = 0; i < items.countText.Length; i++)
        {
            items.countText[i].text = Game.achivedLevelTarget.ToString();           
        }
    }
    public void UpdateUI()
    {
        for (int i = 0; i < items.countText.Length; i++)
        {
            items.countText[i].text = Game.achivedLevelTarget.ToString();
            items.countAniator.SetTrigger("blink");
        }
       
    }
    // Start is called before the first frame update
    void Start()
    {
        items.levelImage.fillAmount=0f;
        currentStages = PlayerManager.Instance.currentStages;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartTheGame()
    {
        //Game.currentLevelTarget = 50;
        Game.achivedLevelTarget = 0;
        GameState = true;
        menuElement.SetActive(false);
        PlayerManager.Instance.Jump(currentStages);       
        items.levelImage.fillAmount = 0f;
        MusicManager.PlayMusic("music1");
        MusicManager.PlaySfx("button");
        GameMaster.Instance.ActAddingCoin();       
        GameMaster.Instance.ActAddingDiemond();
    }
    public void Restart()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        MusicManager.PlaySfx("button");
    }
    public void SheduleLevelSlider()
    {
       
        float val =  (float)Game.achivedLevelTarget / Game.currentLevelTarget;
        //Debug.Log("UpdateLevelStatus" + Game.achivedLevelTarget + Game.currentLevelTarget+"  "+ val);
        //transition fill (0.4 seconds)
        items.levelImage.DOFillAmount(val, .4f);
        items.starsIn[0].SetActive(false);
        items.starsIn[1].SetActive(false);
        items.starsIn[2].SetActive(false);
        if (GameMaster.gameMode == GameMaster.GameMode.EndLess)
        {
            items.starsIn[0].SetActive(true);
            items.starsIn[1].SetActive(true);
            items.starsIn[2].SetActive(true);
            if (items.levelImage.fillAmount > 0.30f && !items.starsOut[0].activeSelf)
            {
                items.starsIn[0].SetActive(false);
                items.starsOut[0].SetActive(true);
            }
            if (items.levelImage.fillAmount > 0.60f && !items.starsOut[1].activeSelf)
            {
                items.starsIn[1].SetActive(false);
                items.starsOut[1].SetActive(true);
            }
            if (items.levelImage.fillAmount > 0.90f && !items.starsOut[2].activeSelf)
            {
                items.starsIn[2].SetActive(false);
                items.starsOut[2].SetActive(true);
            }
        }
    }
}
