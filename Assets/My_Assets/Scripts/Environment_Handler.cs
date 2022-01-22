using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Environment_Handler : MonoBehaviour
{
    [SerializeField] GameObject[] environments;
    int index;
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    // Start is called before the first frame update
    private void Start()
    {
        if (nextButton)
        {
            nextButton.onClick.AddListener(ActivateNextEnv);
        }
        if (prevButton)
        {
            prevButton.onClick.AddListener(ActivatePrevEnv);
        }
    }
    void OnEnable()
    {       
        DisableAllEnv();
        index = 0;
        environments[0].SetActive(true);
    }
    int NextIndex()
    {
        index++;
        if(index >= environments.Length)
        {
            index = 0;
        }
        return index;
    }
    int PrevIndex()
    {
        index--;
        if (index < 0)
        {
            index = environments.Length - 1;
        }
        return index;
    }
    void ActivateNextEnv()
    {
        DisableAllEnv();
        environments[NextIndex()].SetActive(true);
    }
    void ActivatePrevEnv()
    {
        DisableAllEnv();
        environments[PrevIndex()].SetActive(true);
    }
    void DisableAllEnv()
    {
        for (int i = 0; i < environments.Length; i++)
        {
            environments[i].SetActive(false);
        }
    }
    public void ActivateEnvironent(int index)
    {
        Game.EnvCount = 0;
        switch (index)
        {
            case 1:
                Game.EnvCount = 1;
                PlayerManager.Instance.environtType = PlayerManager.EnvirontType.Circle;
                GameMaster.Instance.CloseEnvironentPanel();
                break;
            case 2:
                Game.EnvCount = 2;
                PlayerManager.Instance.environtType = PlayerManager.EnvirontType.Triangle;
                GameMaster.Instance.CloseEnvironentPanel();
                break;
            case 3:
                Game.EnvCount = 3;
                PlayerManager.Instance.environtType = PlayerManager.EnvirontType.Hecta;
                GameMaster.Instance.CloseEnvironentPanel();
                break;
            case 4:
                Game.EnvCount = 4;
                PlayerManager.Instance.environtType = PlayerManager.EnvirontType.Square;
                GameMaster.Instance.CloseEnvironentPanel();
                break;
        }
    }
}
