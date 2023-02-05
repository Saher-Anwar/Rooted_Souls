using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPoint : MonoBehaviour
{
    [SerializeField]
    private LevelManager levelManager;
    [SerializeField]
    private float triggerEnableDelay = 1f;
    [SerializeField]
    private enum TriggerType { ENABLE, DISABLE }
    [SerializeField]
    private TriggerType triggerType = TriggerType.ENABLE;

    // if touched triggerEnablePoint, enable trigger after 1s
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("TriggerPoint: Player touched trigger point");
            StartCoroutine(EnableTrigger());
        }
    }

    private IEnumerator EnableTrigger()
    {
        yield return new WaitForSeconds(triggerEnableDelay);
        if (triggerType == TriggerType.ENABLE)
        {
            Debug.Log("TriggerPoint: Enable trigger");
            levelManager?.EnableTrigger();
        }
        else if (triggerType == TriggerType.DISABLE)
        {
            Debug.Log("TriggerPoint: Disable trigger");
            levelManager?.DisableTrigger();
        }
    }
}
