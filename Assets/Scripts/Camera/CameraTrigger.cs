using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("Loading next level");
            GameManager.instance.LoadNextLevel();

            Debug.Log("Disabling this trigger..");
            gameObject.SetActive(false);
        }
    }
}
