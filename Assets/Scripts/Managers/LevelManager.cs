using Cinemachine;
using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField]
    private GameManager gameManager;
    
    [Header("Level Settings")]
    [SerializeField]
    private int levelNumber = -1;
    
    [Header("Camera Triggers")]
    [SerializeField]
    private GameObject cameraTrigger;

    [Header("Boss Settings")]
    [SerializeField]
    private Transform bossSpawnPoint;
    [SerializeField]
    private GameObject bossPrefab;
    
    private GameObject bossInstance;

    private bool LevelCompletenessCheck()
    {
        // check game manager
        if (gameManager == null)
        {
            Debug.LogError("Game manager is null");
            return false;
        }
        
        // check level
        if (levelNumber < 0)
        {
            Debug.LogError("Level number is invalid");
            return false;
        }
        
        // check boss
        if (bossPrefab == null)
        {
            Debug.LogError("Boss prefab is null");
            return false;

        }
        if (bossSpawnPoint == null)
        {
            Debug.LogError("Boss spawn point is null");
            return false;
        }

        // check camera trigger
        if (cameraTrigger == null)
        {
            Debug.LogError("Camera trigger is null");
            return false;
        }

        return true;
    }

    void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (!LevelCompletenessCheck())
        {
            // Quit editor
            Debug.LogError("Level compromised! Quiting...");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    private void OnEnable()
    {
        gameManager.OnLoadingNextLevel.AddListener(OnLoadingNextLevel);
    }

    private void OnDisable()
    {
        gameManager.OnLoadingNextLevel.RemoveListener(OnLoadingNextLevel);
    }

    private void OnLoadingNextLevel()
    {
        Debug.Log("LevelManager knows Game Manager is loading next level!");
        if (gameManager.GetCurrLevel() == levelNumber)
        {
            InitBoss();
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InitBoss()
    {
        bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation, transform);
        bossInstance.name = "Boss " + levelNumber;
    }
    
    public void TriggerBoss()
    {
        if (bossInstance == null)
        {
            Debug.LogError("Boss instance is null");
            return;
        }
        Boss boss = bossInstance.GetComponent<Boss>();
        
        boss.SetBossTriggered(true);
        boss.OnBossDefeated.AddListener(() =>
        {
            cameraTrigger.SetActive(true);
            Debug.Log("LevelManager knows Boss defeated! Enabling camera trigger!");
        });
    }
    
    public CinemachineVirtualCamera getLevelCamera()
    {
        return GetComponentInChildren<CinemachineVirtualCamera>();
    }
}
