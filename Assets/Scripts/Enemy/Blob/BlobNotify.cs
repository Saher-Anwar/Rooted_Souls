using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Enemy
{
    public class BlobNotify : MonoBehaviour
    {
        public UnityEvent OnBlobDefeated;

        private void OnDisable()
        {
            OnBlobDefeated?.Invoke();
            OnBlobDefeated.RemoveAllListeners();
        }
    }
}