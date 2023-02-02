using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    List<CinemachineVirtualCamera> levelCameras = new List<CinemachineVirtualCamera>();
    int currLevel = 0;

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

    public void LoadNextLevel()
    {
        if (currLevel + 1 >= levelCameras.Count)
        {
            Debug.Log("Not enough cameras to load next level");
            return;
        }

        int currPriority = levelCameras[currLevel].Priority;
        currLevel++;

        levelCameras[currLevel].Priority = (currPriority + 1);
    }
}
