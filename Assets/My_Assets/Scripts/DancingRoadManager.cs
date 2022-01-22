using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancingRoadManager : MonoBehaviour
{
    [SerializeField]Transform ball;
    Vector3 startMousePos, startBallPose;
    bool moveTheBall, DetectNewPath;
    [Range(0f, 1f)] public float maxSpeed;
    [Range(0f, 1f)] public float camSpeed;
    [Range(0f, 50f)] public float pathSpeed;
    float storedPathSpeed;
    float velocity, camVelocity_x, camVelocity_y;
    private Camera mainCam;
    [SerializeField] Transform path;
    Rigidbody rb;
    Collider _collider;
    Renderer BallRenderer;
    [SerializeField] ParticleSystem[] particleSystems;
    [SerializeField] GameObject colliderParticle;
    [SerializeField] ParticleSystem airEffect;
    public Transform ballMesh;

    [SerializeField] GameObject[] stages;
    int stageIndex;
    [HideInInspector] Vector3 currentPose=new Vector3 (0,0,0);
    [SerializeField] GameObject[] platforms;
    int platformIndex=0;
    public Transform currentStages;
    public static DancingRoadManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        storedPathSpeed = pathSpeed;
        ball = transform;
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        BallRenderer = ball. GetComponent<Renderer>();
        airEffect.gameObject.SetActive(false);
        //CreateNewPlatform();
        StartCoroutine(TryToCreatePlateform());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && MenuManagerRoad.Instance.GameState)
        {
            moveTheBall = true;

            Plane newPlane = new Plane(Vector3.up ,0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (newPlane.Raycast(ray, out var distance))
            {
                startMousePos = ray.GetPoint(distance);
                startBallPose = ball.position;
            }

        }else if (Input.GetMouseButtonUp(0))
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
                Vector3 MouseNewPose =mouseNewPose-startMousePos;
                Vector3 DesireBallPos = MouseNewPose + startBallPose;

                DesireBallPos.x = Mathf.Clamp(DesireBallPos.x, -33.5f, 33.5f);
                ball.position = new Vector3(Mathf.SmoothDamp(ball.position.x, DesireBallPos.x, ref velocity, maxSpeed)
                    , ball.position.y, ball.position.z);
            }
           
        }
        if (MenuManagerRoad.Instance.GameState)
        {
            var pathNewPose = path.position;
            path.position = new Vector3(pathNewPose.x, pathNewPose.y, Mathf.MoveTowards(pathNewPose.z, -1000f, pathSpeed * Time.deltaTime));
        }
        if (MenuManagerRoad.Instance.GameState)
        {
            if (ballMesh)
            {
                ballMesh.Rotate(pathSpeed, 0, 0);
            }
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
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("obstacle"))
        //{
        //    gameObject.SetActive(false);
        //    MenuManager.Instance.GameState = false;
        //    MenuManager.Instance.Restart();
        //}
        if (MenuManagerRoad.Instance.GameState)
        {
            switch (other.tag)
            {
                case "red":
                    GetNewParticle(other);
                    break;
                case "green":
                    GetNewParticle(other);
                    break;
                case "yellow":
                    GetNewParticle(other);
                    break;
                case "blue":
                    GetNewParticle(other);
                    break;
                case "platform":
                    ResetPlatformPose();
                    break;
                case "stage":
                    Jump(other.transform);
                    break;

            }
        }
    }
    public void Jump(Transform other)
    {
        //rb.isKinematic = _collider.isTrigger = false;
        //float yForce = other.GetComponent<Stage>().nextStageDistance / 1.4f;
        float jumpForce = CalculateJumpSpeed(other.GetComponent<Stage>().nextStageDistance, 4f);
        rb.velocity = new Vector3(0f,jumpForce, 0f);
        other.GetComponent<Stage>().ActiveEffects();
        //pathSpeed = pathSpeed * 2;
    }
    private float CalculateJumpSpeed(float jumpHeight, float gravity)
    {
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }
    int FlatforOtherIndex()
    {
        if (platformIndex == 0)
        {
            return 1;
        }
        else
            return 0;
        
    }
    private void ResetPlatformPose()
    {       
        platforms[platformIndex].transform.position = new Vector3(
            platforms[platformIndex].transform.position.x,
            platforms[platformIndex].transform.position.y, 
            platforms[platformIndex].transform.position.z + 180);       
            platformIndex = FlatforOtherIndex();        
    }
    void GetNewParticle(Collider other)
    {
        other.gameObject.SetActive(false);
        BallRenderer.material = other.GetComponent<Renderer>().material;
        var newParticle = Instantiate(colliderParticle, transform.position, Quaternion.identity);
        newParticle.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
    }
    private void OnTriggerExit(Collider other)
    {
    //    if (other.CompareTag("stage"))
    //    {
    //        rb.isKinematic = _collider.isTrigger = false;
    //        float yForce = other.GetComponent<Stage>().nextStageDistance / 10;
    //        rb.velocity = new Vector3(0f, yForce, 0f);
    //        pathSpeed = pathSpeed * 2;
    //        //airEffect.gameObject.SetActive(true);
    //        //var airEffectMain = airEffect.main;
    //        //airEffectMain.simulationSpeed = 10f;
    //    }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("stage"))
        //{
        //    rb.isKinematic = _collider.isTrigger = true;          
        //    pathSpeed = storedPathSpeed;

        //    //var airEffectMain = airEffect.main;
        //    //airEffectMain.simulationSpeed = 4f;
        //    //airEffect.gameObject.SetActive(false);
        //}
    }
    IEnumerator TryToCreatePlateform()
    {
        yield return new WaitForSeconds(1f);
        //CreateNewPlatform();
        StartCoroutine(TryToCreatePlateform());
    }
    public void CreateNewPlatform()
    {
        stageIndex = Random.Range(0, stages.Length);
        GameObject newStage = Instantiate(stages[stageIndex]);
        float randomX = Random.Range(-3.5f, 3.5f);
        float randomZ = Random.Range(currentStages.position.z+5f, currentStages.position.z + 25f);       
        currentPose = new Vector3(randomX, 13.5f, randomZ);
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
}
