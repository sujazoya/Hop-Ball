using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stage_
{
    public GameObject stage;
    public MeshRenderer body;
    public Collider stageCollider;    
}

public class Square_Stage : MonoBehaviour
{
    [SerializeField] Stage_[] stages;
    [SerializeField] Material enableMat;
    int stageIndex;
    private Stage stage;
    // Start is called before the first frame update
    void OnEnable()
    {
        stage = GetComponent<Stage>();
        stageIndex = Random.Range(0, stages.Length);
        ActivateStage(stageIndex);       
    }
    void ActivateStage(int index)
    {

        stages[index].stage.tag = "stage";
        stages[index].stageCollider.enabled = true;
       
        var mats = stages[index].body.materials;
        mats[0].CopyPropertiesFromMaterial(enableMat);
        StartCoroutine(TranferSpeed(1f, index));
        //mats[0].mainTexture = texture;
        //for (var i = 0; i < mats.Length; i++)
        //    if (mats[i].name == "Material-A (Instance)")
        //        mats[i] = Resources.Load("Material-A") as Material;
        //mats[0] = enableMat;
    }
    IEnumerator TranferSpeed(float wait,int index)
    {
        yield return new WaitUntil(()=> stage.nextStageDistance>1);
        Stage myStage = stages[index].stage.GetComponent<Stage>();
        myStage.nextStageDistance = stage.nextStageDistance;
        myStage.angle = stage.angle;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1f);
    }
  
}
