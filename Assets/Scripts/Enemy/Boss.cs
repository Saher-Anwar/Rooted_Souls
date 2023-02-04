using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Enemy
{
    public class Boss : MonoBehaviour
    {
        public UnityEvent OnBossDefeated;

        [Header("Boss settings")]
        [SerializeField]
        private int blobsToSpawnWhenBossDies = 5;
        [SerializeField]
        private int HP = 100;
        [SerializeField]
        private bool isBossDefeatable = true;
        [SerializeField]
        private bool isAlive = true;

        [Space]
        [Header("Blobs settings")]
        [SerializeField]
        private bool spawnBlobs = true;
        [SerializeField]
        private float sapwnRange = 5.0f;
        [SerializeField]
        private GameObject[] blobPrefabs;
        [SerializeField]
        private int maxBlobsToSpawn = 5;

        private List<Blob> blobs = new List<Blob>();

        [Space]
        [Header("Blobs spawn settings")]
        [SerializeField]
        private float minTimeBetweenBlobs = 2.0f;

        private float bossAliveTime = 0.0f;
        private float nextSpawnTime = 0.0f;

        private void CalcNextSpawnTime()
        {
            //minTimeBetweenBlobs += bossAliveTime / 100;
            nextSpawnTime = bossAliveTime + minTimeBetweenBlobs;
        }

        private bool BossCompletenessCheck()
        {
            // check blobs
            if (blobPrefabs.Length <= 0)
            {
                Debug.LogError("Blob prefabs is empty");
                return false;
            }
            if (maxBlobsToSpawn <= 0)
            {
                Debug.LogError("Max blobs to spawn is less than or equal to 0");
                return false;
            }
            return true;
        }

        private void Awake()
        {
            if (!BossCompletenessCheck())
            {
                // Quit editor
                Debug.LogError("Boss compromised! Quiting...");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            }
        }

        // Use this for initialization
        void Start()
        {
            nextSpawnTime = minTimeBetweenBlobs;
        }

        // Update is called once per frame
        void Update()
        {
            bossAliveTime += Time.deltaTime;
            if (bossAliveTime >= nextSpawnTime)
            {
                if (blobs.Count < maxBlobsToSpawn)
                {
                    SpawnBlob(true);
                }
                CalcNextSpawnTime();
            }
        }

        private void SpawnBlob(bool bindToBoss)
        {
            if (!spawnBlobs)
            {
                return;
            }
            Vector3 spawnPos = transform.position;
            spawnPos.x += Random.Range(-sapwnRange, sapwnRange);

            // generate random blob
            int blobIndex = Random.Range(0, blobPrefabs.Length);
            var blobGO = Instantiate(blobPrefabs[blobIndex], spawnPos, Quaternion.identity);

            Blob blob = blobGO.GetComponent<Blob>();

            if (bindToBoss)
            {
                // bind blob to boss
                blob.OnBlobDefeated.AddListener(() => RemoveBlob(blob));
                // add blob to blobs
                blobs.Add(blob);
            }

        }

        public void OnDamage(int damage)
        {
            HP -= damage;

            if (HP <= 0)
            {
                if (!isBossDefeatable)
                {
                    Debug.Log("Boss is not defeatable");
                    HP = 1;
                    return;
                }
                
                isAlive = false;
                // kill all blobs
                foreach (var blob in blobs)
                {
                    blob.DieImediately();
                }

                Debug.Log("Boss defeated");
                // spawn final blobs
                for (int i = 0; i < blobsToSpawnWhenBossDies; ++i)
                {
                    SpawnBlob(false);
                }
                OnBossDefeated?.Invoke();
                Destroy(gameObject);
            }
        }

        public void SetBossDefeatable(bool isDefeatable)
        {
            isBossDefeatable = isDefeatable;
        }

        public void SetBossHP(int hp)
        {
            HP = hp;
        }

        public void RemoveBlob(Blob blob)
        {
            if (!isAlive)
            {
                return;
            }
            blobs.Remove(blob);
            CalcNextSpawnTime();
        }
    }
}