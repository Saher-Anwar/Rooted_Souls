using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = true; //OG: Was originally set to false.
        visualCue.SetActive(true);

    }

    private void Update()
    {
        if (playerInRange)
        {
            visualCue.SetActive(true);

            //Debug.Log(inkJSON.text);
            
            //
            if (Input.GetKeyDown("i"))
            {
                //Debug.Log(inkJSON.text);
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
            
        } else
        {
            visualCue.SetActive(false);
           
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("Trigger");
        
        
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
        
    }


    private void OnTriggerExit2D(Collider2D collider)
    {

        //Debug.Log("No Trigger");
       
        
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
        


        //playerInRange = false;
    }
    */

}
