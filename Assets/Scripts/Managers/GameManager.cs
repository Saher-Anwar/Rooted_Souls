using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    List<LevelManager> levelManagers = new List<LevelManager>();

    List<CinemachineVirtualCamera> levelCameras = new List<CinemachineVirtualCamera>();
    
    [SerializeField]
    private int currLevel = 0;

    public UnityEvent OnLoadingNextLevel;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (LevelManager levelManager in levelManagers)
        {
            levelCameras.Add(levelManager.getLevelCamera());
        }
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        //if (currLevel + 1 >= levelCameras.Count)
        //{
        //    Debug.Log("Not enough cameras to load next level");
        //    return;
        //}

        //int currPriority = levelCameras[currLevel].Priority;
        //currLevel++;

        //levelCameras[currLevel].Priority = (currPriority + 1);

        currLevel++;
        OnLoadingNextLevel?.Invoke();
    }

    public int GetCurrLevel()
    {
        return currLevel;
    }
}
