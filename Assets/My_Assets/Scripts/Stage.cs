using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public enum ItemType
    {
        None,Diemond,Coin,Star
    }

     public float nextStageDistance;
     public float angle;
    public GameObject effects;
    Animator animator;
    [SerializeField] bool startingStage;
    [SerializeField] ItemType itemType;
    [SerializeField] GameObject item;
    [SerializeField] bool stageParent;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (!stageParent)
        {
            effects.SetActive(false);
            animator = GetComponent<Animator>();
            if (item) { item.SetActive(true); }
        }       
        
    }
 
    public void ActiveEffects()
    {
        if (!stageParent)
        {
            effects.SetActive(true);
            Invoke("DisableSelf", 2);
            if (item) { item.SetActive(false); }
            Game.achivedLevelTarget++;
            switch (itemType)
            {
                case ItemType.Coin:
                    Game.TotalCoins += 10;
                    MusicManager.PlaySfx_Other("coin");
                    GameMaster.Instance.ActAddingCoin();
                    break;
                case ItemType.Diemond:
                    Game.TotalDiemonds++;
                    MusicManager.PlaySfx_Other("diemond");
                    GameMaster.Instance.ActAddingDiemond();
                    break;
                case ItemType.Star:
                    Game.achivedStar++;
                    MusicManager.PlaySfx_Other("star");
                    break;
            }
            animator.SetBool("act", true);
        }
       
       
    }
    void DisableSelf()
    {
        gameObject.SetActive(false);
        PlayerManager.Instance.stageInScene--;
    }
}
