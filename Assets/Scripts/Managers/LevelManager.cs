using Cinemachine;
using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField]
    private int levelNumber = 0;
    [Header("Camera Triggers")]
    [SerializeField]
    private GameObject cameraTrigger;

    [Header("Boss Settings")]
    [SerializeField]
    private Transform bossSpawnPoint;
    [SerializeField]
    private GameObject bossPrefab;
    
    private bool LevelCompletenessCheck()
    {
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


    // Start is called before the first frame update
    void Start()
    {
        var boss = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);

        boss.GetComponent<Boss>().OnBossDefeated.AddListener(() =>
        {
            cameraTrigger.SetActive(true);
            Debug.Log("LevelManager knows Boss defeated! Enabling camera trigger!");
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public CinemachineVirtualCamera getLevelCamera()
    {
        return GetComponentInChildren<CinemachineVirtualCamera>();
    }
}
