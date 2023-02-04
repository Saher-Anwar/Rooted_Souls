using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Enemy
{
    public class Blob : MonoBehaviour
    {
        public UnityEvent OnBlobDefeated;

        [Header("Blob settings")]
        [SerializeField]
        private int HP = 10;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnDamage(int damage)
        {
            HP -= damage;
            if (HP <= 0)
            {
                Debug.Log("Blob defeated.");
                OnBlobDefeated?.Invoke();
                Destroy(gameObject);
            }
        }

        public void DieImediately()
        {
            OnDamage(HP);
        }

    }
}