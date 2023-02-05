using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    public Sprite[] backgrounds;
    private int imageIndex = 0;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private float typeWriterSpeed = 30f;
    private GameObject npc;
    [SerializeField] private Image BG;
    [SerializeField] private TextAsset initialInkJSON;


    [Header("Choices UI")]

    private Story currentStory;
    public bool dialogueIsPlaying;
    public int lockTyping;
    [SerializeField, OptionalField]
    LevelManager currLevel;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the Scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    public void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        readDialogue(initialInkJSON);
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (currentStory.currentChoices.Count == 0 && Input.GetKeyDown("q"))
        {
            ContinueStory();
        }

    }


    public void EnterDialogueMode(TextAsset inkJSON, GameObject npc)
    {
        this.npc = npc;
        readDialogue(inkJSON);

    }

    private void readDialogue(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);

        dialogueIsPlaying = true;

        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        Destroy(npc);

        currLevel?.TriggerBoss();
        loadNextUI();
    }


    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
           
            if (lockTyping == 0)
            {
                
                string textToType = currentStory.Continue();
                StartCoroutine(TypeText(textToType, dialogueText));
            }

            displayChoices();
        }
        else
        {
            ExitDialogueMode();

        }
    }

    /**
     * Method to type text by each characher (str), it gets locked until it 
     * finishes to type in the dialog box(dialogueText) then unlocks for next
     * textToType
     * 
     */
    private IEnumerator TypeText(string textToType, TextMeshProUGUI dialogueText)
    {

        dialogueText.text = string.Empty;
        lockTyping = 1;

        //yield return new WaitForSeconds(1);

        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {

            t += Time.deltaTime * typeWriterSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            dialogueText.text = textToType.Substring(0, charIndex);

            yield return null;

        }

   
        
        lockTyping = 0;

        dialogueText.text = textToType;
    }


    private void displayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        int index = 0;
       
        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        // Event System needs to be cleared first then wait at least for one frame 

        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
  
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
    }

    public void loadNextUI()
    {
        if(imageIndex+1 < backgrounds.Length)
        BG.sprite = backgrounds[imageIndex];
        imageIndex++;
    }
}
