using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Environment
{
    public string name;
    public GameObject envParent;
    public GameObject[] stages;
    public GameObject[] otherItems;
    public Transform path;
}

public class PlayerManager : MonoBehaviour
{
    public enum EnvirontType
    {
        Circle,Triangle,Square,Hecta,Scifi
    }
    public EnvirontType environtType=EnvirontType.Triangle;
    public Environment[] environments;
    public Transform ball;
    Vector3 startMousePos, startBallPose;
    [SerializeField] bool moveTheBall;
    [Range(0f, 1f)] public float maxSpeed;
    [Range(0f, 1f)] public float camSpeed;
    [Range(0f, 50f)] public float pathSpeed;
    [Range(0f, 15f)] public float multyplaier;
    float storedPathSpeed;
    float velocity, camVelocity_x, camVelocity_y;
    private Camera mainCam;
    [SerializeField] Transform path;
    Rigidbody rb;
    //Collider _collider;
    Renderer BallRenderer;
    [SerializeField] ParticleSystem[] particleSystems;
    [SerializeField] GameObject colliderParticle;
    [SerializeField] ParticleSystem airEffect;
    public Transform ballMesh;

    [SerializeField] GameObject[] stages;
    int stageIndex;
    [HideInInspector] Vector3 currentPose=new Vector3 (0,0,0);
    //[SerializeField] GameObject[] platforms;
    int platformIndex=0;
    public Transform currentStages;
    public static PlayerManager Instance;
    [SerializeField] GameObject boostEffect;
    [SerializeField] GameObject ballSelecter;
    [SerializeField] Texture2D[] balls;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (boostEffect) { boostEffect.SetActive(false); }
        if (ballSelecter) { ballSelecter.SetActive(false); }
    }
    // Start is called before the first frame update
    void Start()
    {

        //SetGameDifficulty(GameMaster.GameMode.Easy);
        storedPathSpeed = pathSpeed;
        ball = transform;
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();
        //_collider = GetComponent<Collider>();
        BallRenderer = ballMesh. GetComponent<Renderer>();
        airEffect.gameObject.SetActive(false);
        //CreateNewPlatform();
        GetCurentEnvironent();
    }
    void UpdateBallTexture()
    {
        BallRenderer = ballMesh.GetComponent<Renderer>();

        //Make sure to enable the Keywords
        BallRenderer.material.EnableKeyword("_NORMALMAP");
        BallRenderer.material.EnableKeyword("_METALLICGLOSSMAP");

        //Set the Texture you assign in the Inspector as the main texture (Or Albedo)
        BallRenderer.material.SetTexture("_MainTex", balls[Game.BallIndex]);
        //Set the Normal map using the Texture you assign in the Inspector
        //BallRenderer.material.SetTexture("_BumpMap", m_Normal);
        ////Set the Metallic Texture as a Texture you assign in the Inspector
        //BallRenderer.material.SetTexture("_MetallicGlossMap", m_Metal);
    }
    public void ShowBallSelecter()
    {
        GameMaster.Instance.UIObject(Game.Menu).SetActive(false);
        if (ballSelecter) { ballSelecter.SetActive(true); }
    }
    public void CloseBallSelecter()
    {
        if (ballSelecter) { ballSelecter.SetActive(false); }
        GameMaster.Instance.UIObject(Game.Menu).SetActive(true);
    }
    void GetCurentEnvironent()
    {
        switch (Game.EnvCount)
        {
            case 1:
                environtType = EnvirontType.Circle;
                break;
            case 2:
                environtType = EnvirontType.Triangle;
                break;
            case 3:
                environtType = EnvirontType.Hecta;
                break;
            case 4:
                environtType = EnvirontType.Square;
                break;
            case 5:
                environtType = EnvirontType.Scifi;
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!Application.isMobilePlatform)
        {
            if (Input.GetMouseButtonDown(0) && MenuManager.Instance.GameState)
            {
                moveTheBall = true;

                Plane newPlane = new Plane(Vector3.up, 0f);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (newPlane.Raycast(ray, out var distance))
                {
                    startMousePos = ray.GetPoint(distance);
                    startBallPose = ball.position;
                }

            }
            else if (Input.GetMouseButtonUp(0))
            {
                moveTheBall = false;
            }
            if (moveTheBall)
            {
                Plane newPlane = new Plane(Vector3.up, 0f);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (newPlane.Raycast(ray, out var distance))
                {
                    Vector3 mouseNewPose = ray.GetPoint(distance);
                    Vector3 MouseNewPose = mouseNewPose - startMousePos;
                    Vector3 DesireBallPos = MouseNewPose + startBallPose;

                    DesireBallPos.x = Mathf.Clamp(DesireBallPos.x, -3.5f, 3.5f);
                    ball.position = new Vector3(Mathf.SmoothDamp(ball.position.x, DesireBallPos.x, ref velocity, maxSpeed)
                        , ball.position.y, ball.position.z);
                }

            }
        }
        else
        {
            OperateBall();
        }
        if (MenuManager.Instance.GameState)
        {
            var pathNewPose = path.position;
            path.position = new Vector3(pathNewPose.x, pathNewPose.y, Mathf.MoveTowards(pathNewPose.z, -1000f, pathSpeed * Time.deltaTime));
        }
        if (MenuManager.Instance.GameState)
        {
            if (ballMesh)
            {
                ballMesh.Rotate(pathSpeed, 0, 0);
            }
        }
    }  
    public void SetGameDifficulty(GameMaster.GameMode mode)
    {
        switch (mode)
        {
            case GameMaster.GameMode.Easy:
                pathSpeed = 7f    ;
                maxSpeed = 0.23f  ;
                multyplaier = 3.18f;
                break;
            case GameMaster.GameMode.Medium:
                pathSpeed = 7f     * 2f;
                maxSpeed = 0.23f / 1.9f;
                multyplaier = 3.18f/2;
                break;
            case GameMaster.GameMode.Hard:
                pathSpeed = 7f       * 3f;
                maxSpeed = 0.23f / 3f;
                multyplaier = 3.18f/3;
                break;
            case GameMaster.GameMode.EndLess:
                pathSpeed = 7f;
                maxSpeed = 0.23f;
                multyplaier = 3.18f;
                break;
        }
    }
    private void LateUpdate()
    {
        var cameraNewPose = mainCam.transform.position;
        if (rb.isKinematic)
        {
            mainCam.transform.position = new Vector3(Mathf.SmoothDamp(cameraNewPose.x, ball.transform.position.x,ref camVelocity_x, camSpeed),
                Mathf.SmoothDamp(cameraNewPose.y, ball.transform.position.y + 3f,ref camVelocity_y, camSpeed), cameraNewPose.z);
        }
        if (ball.localPosition.y<=0 && !gameOvered)
        {
            gameOvered = true;
            MenuManager.Instance.GameState = false;
            GameMaster.Instance.OnGameover();
        }
    }
    bool gameOvered;
    #region FOR MOBILE
    void OperateBall()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            Vector3 pos = touch.position;

            if (touch.phase == TouchPhase.Began && MenuManager.Instance.GameState)
            {
                moveTheBall = true;

                Plane newPlane = new Plane(Vector3.up, 0f);
                Ray ray = Camera.main.ScreenPointToRay(pos);
                if (newPlane.Raycast(ray, out var distance))
                {
                    startMousePos = ray.GetPoint(distance);
                    startBallPose = ball.position;
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                moveTheBall = false;
            }
            if (moveTheBall)
            {
                Plane newPlane = new Plane(Vector3.up, 0f);
                Ray ray = Camera.main.ScreenPointToRay(pos);
                if (newPlane.Raycast(ray, out var distance))
                {
                    Vector3 mouseNewPose = ray.GetPoint(distance);
                    Vector3 MouseNewPose = mouseNewPose - startMousePos;
                    Vector3 DesireBallPos = MouseNewPose + startBallPose;

                    DesireBallPos.x = Mathf.Clamp(DesireBallPos.x, -3.5f, 3.5f);
                    ball.position = new Vector3(Mathf.SmoothDamp(ball.position.x, DesireBallPos.x, ref velocity, maxSpeed)
                        , ball.position.y, ball.position.z);
                }

            }
        }
    }
    #endregion
    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("obstacle"))
        //{
        //    gameObject.SetActive(false);
        //    MenuManager.Instance.GameState = false;
        //    MenuManager.Instance.Restart();
        //}
        if (MenuManager.Instance.GameState)
        {
            switch (other.tag)
            {
                case "red":
                    if(Game.gameStatus != Game.GameStatus.isGameWin&&!gameOvered)
                    {
                        gameOvered = true;
                        MenuManager.Instance.GameState = false;
                        GameMaster.Instance.OnGameover();
                    }                   
                    break;
                //case "green":                
                //    GetNewParticle(other);
                //    break;
                //case "yellow":               
                //    GetNewParticle(other);
                //    break;
                //case "blue":               
                //    GetNewParticle(other);
                //    break;
                case "platform":
                    //ResetPlatformPose();
                    break;
                case "stage":
                    Jump(other.transform);
                    break;

            }
        }
    }   
    public void Jump(Transform other)
    {       
        float jumpForce = CalculateJumpSpeed(other.GetComponent<Stage>().nextStageDistance, pathSpeed);
        rb.velocity = new Vector3(0, jumpForce + maxSpeed, 0f);
        other.GetComponent<Stage>().ActiveEffects();
        MenuManager.Instance.UpdateUI();
        MenuManager.Instance.SheduleLevelSlider();
        MusicManager.PlaySfx("Ball_Drop");
        //pathSpeed = pathSpeed * 2;
    }
    private float CalculateJumpSpeed(float jumpHeight, float gravity)
    {
        float pm = gravity / multyplaier;
        float ps = pathSpeed / jumpHeight * pm;
        return Mathf.Sqrt( jumpHeight * gravity / ps);
    }
    int FlatforOtherIndex()
    {
        if (platformIndex == 0)
        {
            return 1;
        }
        else if (platformIndex == 1)
        {
            return 0;
        }
        else
        {
            return 0;
        }
        
    }
    //private void ResetPlatformPose()
    //{       
    //    platforms[platformIndex].transform.position = new Vector3(
    //        platforms[platformIndex].transform.position.x,
    //        platforms[platformIndex].transform.position.y, 
    //        platforms[platformIndex].transform.position.z + 180);       
    //    platformIndex = FlatforOtherIndex();        
    //}
    void GetNewParticle(Collider other)
    {
        other.gameObject.SetActive(false);
        BallRenderer.material = other.GetComponent<Renderer>().material;
        var newParticle = Instantiate(colliderParticle, transform.position, Quaternion.identity);
        newParticle.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material; ;
    }
    //private void OnTriggerExit(Collider other)
    //{
    ////    if (other.CompareTag("stage"))
    ////    {
    ////        rb.isKinematic = _collider.isTrigger = false;
    ////        float yForce = other.GetComponent<Stage>().nextStageDistance / 10;
    ////        rb.velocity = new Vector3(0f, yForce, 0f);
    ////        pathSpeed = pathSpeed * 2;
    ////        //airEffect.gameObject.SetActive(true);
    ////        //var airEffectMain = airEffect.main;
    ////        //airEffectMain.simulationSpeed = 10f;
    ////    }
    //}
    //private void OnCollisionEnter(Collision collision)
    //{
    //    //if (collision.gameObject.CompareTag("stage"))
    //    //{
    //    //    rb.isKinematic = _collider.isTrigger = true;          
    //    //    pathSpeed = storedPathSpeed;

    //    //    //var airEffectMain = airEffect.main;
    //    //    //airEffectMain.simulationSpeed = 4f;
    //    //    //airEffect.gameObject.SetActive(false);
    //    //}
    //}
    bool winDeclared;
    [HideInInspector] public int stageInScene;  
    private int stageCreated;
    int stageCountToCreate=10;
    public void GetCurrentEnvItems()
    {
        switch (environtType)
        {
            case EnvirontType.Circle:
                environments[0].envParent.SetActive(true);
                environments[1].envParent.SetActive(false);
                environments[2].envParent.SetActive(false);
                environments[3].envParent.SetActive(false);
                environments[4].envParent.SetActive(false);
                stages = null;
                stages = environments[0].stages;
                path = null;
                path = environments[0].path;
                for (int i = 0; i < environments[0].otherItems.Length; i++)
                {
                    environments[0].otherItems[i].SetActive(true);
                }               
                break;
            case EnvirontType.Triangle:
                environments[0].envParent.SetActive(false);
                environments[1].envParent.SetActive(true);
                environments[2].envParent.SetActive(false);
                environments[3].envParent.SetActive(false);
                environments[4].envParent.SetActive(false);
                stages = null;
                stages = environments[1].stages;
                path = null;
                path = environments[1].path;
                for (int i = 0; i < environments[1].otherItems.Length; i++)
                {
                    environments[1].otherItems[i].SetActive(true);
                }
                break;
            case EnvirontType.Hecta:
                environments[0].envParent.SetActive(false);
                environments[1].envParent.SetActive(false);
                environments[2].envParent.SetActive(true);
                environments[3].envParent.SetActive(false);
                environments[4].envParent.SetActive(false);
                stages = environments[2].stages;
                path = environments[2].path;
                for (int i = 0; i < environments[2].otherItems.Length; i++)
                {
                    environments[2].otherItems[i].SetActive(true);
                }
                break;
            case EnvirontType.Square:
                environments[0].envParent.SetActive(false);
                environments[1].envParent.SetActive(false);
                environments[2].envParent.SetActive(false);
                environments[3].envParent.SetActive(true);
                environments[4].envParent.SetActive(false);
                stages = environments[3].stages;
                path = environments[3].path;
                for (int i = 0; i < environments[3].otherItems.Length; i++)
                {
                    environments[3].otherItems[i].SetActive(true);
                }
                break;
            case EnvirontType.Scifi:
                environments[0].envParent.SetActive(false);
                environments[1].envParent.SetActive(false);
                environments[2].envParent.SetActive(false);
                environments[3].envParent.SetActive(false);
                environments[4].envParent.SetActive(true);
                stages = environments[4].stages;
                path = environments[4].path;
                for (int i = 0; i < environments[3].otherItems.Length; i++)
                {
                    environments[4].otherItems[i].SetActive(true);
                }
                break;


        }
        StartCoroutine(TryToCreatePlateform());
        UpdateBallTexture();
    }
   public IEnumerator TryToCreatePlateform()
    {       
        yield return new WaitForSeconds(0.2f);
        if (stageInScene < stageCountToCreate&& stageCreated < Game.currentLevelTarget) 
        {
            CreateNewPlatform();
            stageInScene++;
            stageCreated++;
        }
        if(GameMaster.gameMode == GameMaster.GameMode.EndLess)
        {
            switch (stageCreated)
            {
                case 35:
                    SetGameDifficulty(GameMaster.GameMode.Medium);
                    StartCoroutine(ActBoost());
                    break;
                case 75:
                    SetGameDifficulty(GameMaster.GameMode.Hard);
                    StartCoroutine(ActBoost());
                    break;               
            }
        }
        if (MenuManager.Instance.GameState && Game.achivedLevelTarget >= Game.currentLevelTarget&&!winDeclared)
        {
            Game.gameStatus = Game.GameStatus.isGameWin;
            GameMaster.Instance.OnGameWon();
            winDeclared = true;
            MenuManager.Instance.UnlockLevel(Game.CurrentLevel);
        }
         StartCoroutine(TryToCreatePlateform());
       
    }

    IEnumerator ActBoost()
    {
        if (boostEffect) { boostEffect.SetActive(true); }
        yield return new WaitForSeconds(2.2f);
        if (boostEffect) { boostEffect.SetActive(false); }
    }
    public void CreateNewPlatform()
    {
        stageIndex = Random.Range(0, stages.Length);
        GameObject newStage = Instantiate(stages[stageIndex]);
        float randomX = Random.Range(-3.5f, 3.5f);
        float randomZ = Random.Range(currentStages.position.z+5f, currentStages.position.z + 25f);
        if (environtType == EnvirontType.Square)
        {
            currentPose = new Vector3(0, 13.5f, randomZ);
        }
        else
        {
            currentStages.GetComponent<Stage>().angle = randomX;
            currentPose = new Vector3(randomX, 13.5f, randomZ);
        }
        newStage.transform.position = currentPose;
        newStage.transform.rotation = Quaternion.identity;
        newStage.transform.parent = path;
        float distnce = Vector3.Distance(currentStages.transform.position, newStage.transform.position);
        currentStages.GetComponent<Stage>().nextStageDistance = distnce;
        currentStages = newStage.transform;
        //stageIndex++;
        //if(stageIndex>= stages.Length)
        //{
        //    stageIndex = 0;
        //}
    }
   [SerializeField]float nextZ = 365;
    public float NextZ()
    {
        nextZ += 183;
        return nextZ;
    }
}
