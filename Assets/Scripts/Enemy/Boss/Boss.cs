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

        [SerializeField]
        private bool isTriggered = false;
        public void SetBossTriggered(bool value)
        {
            isTriggered = value;
        }

        public bool GetBossTriggered()
        {
            return isTriggered;
        }

        [Space]
        [Header("Blobs settings")]
        [SerializeField]
        private bool spawnBlobs = true;
        [SerializeField]
        private List<EnemyAI> differentTypesOfEnemies = new List<EnemyAI>();
        [SerializeField]
        private int maxBlobsToSpawn = 5;
        
        private List<IBlob> blobs = new();

        [Space]
        [Header("Blobs spawn settings")]
        [SerializeField]
        private Vector2 blobSpawnOffset = new Vector2(0.0f, 2.0f);
        [SerializeField]
        private float minTimeBetweenBlobs = 2.0f;
        [SerializeField]
        private float minThrowDistance = 5.0f;
        [SerializeField]
        private float maxThrowDistance = 10.0f;
        [SerializeField]
        private float minThrowAngle = 30.0f;
        [SerializeField]
        private float maxThrowAngle = 60.0f;

        [Space]
        [Header("Player")]
        private GameObject player;

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
            if (differentTypesOfEnemies.Count <= 0)
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
        private void OnDisable()
        {
            OnBossDefeated.RemoveAllListeners();
        }

        // Use this for initialization
        void Start()
        {
            // Get player
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player not found");
            }
            maxThrowDistance = (player.transform.position.x - transform.position.x) / 2;
            minThrowDistance = maxThrowDistance / 2;
            nextSpawnTime = minTimeBetweenBlobs;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isTriggered)
            {
                return;
            }

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
            Vector2 targetPos = new(transform.position.x, transform.position.y);
            targetPos.x += Random.Range(minThrowDistance, maxThrowDistance);

            Vector2 startPos = new Vector2(transform.position.x, transform.position.y) + blobSpawnOffset;

            // generate random blob
            int blobIndex = Random.Range(0, differentTypesOfEnemies.Count);
            EnemyAI randomEnemy = differentTypesOfEnemies[blobIndex];
            
            var blobGO = Instantiate(randomEnemy, transform.position, Quaternion.identity);


            float angle = Random.Range(minThrowAngle, maxThrowAngle);

            Vector2 speed = CalculateVelocity2D(startPos, targetPos, angle);
            blobGO.GetComponent<Rigidbody2D>().velocity = speed;

            IBlob blob = blobGO.GetComponent<IBlob>();
            if (bindToBoss)
            {
                // bind blob to boss

                blobGO.GetComponent<BlobNotify>().OnBlobDefeated.AddListener(() => RemoveBlob(blob));
                // add blob to blobs
                blobs.Add(blob);
                blobGO.name = "Blob " + blobs.Count;
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
                    blob.TakeDamage(100000000);
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

        public bool SetBossDefeatable()
        {
            return isBossDefeatable;
        }

        public void SetBossDefeatable(bool isDefeatable)
        {
            isBossDefeatable = isDefeatable;
        }

        public void SetBossHP(int hp)
        {
            HP = hp;
        }

        public void RemoveBlob(IBlob blob)
        {
            if (!isAlive)
            {
                return;
            }
            blobs.Remove(blob);
            CalcNextSpawnTime();
        }

        // calculate initial velocity to reach target
        private Vector2 CalculateVelocity2D(Vector2 start, Vector2 target, float angle)
        {
            // define gravity
            float gravity = Physics2D.gravity.magnitude;

            // define distance
            Vector2 distance = target - start;
            float distanceX = distance.x;
            float distanceY = distance.y;

            // define angle
            float angleRad = angle * Mathf.Deg2Rad;

            // calculate initial velocity
            float velocity = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY) / (Mathf.Sin(2 * angleRad) / gravity);

            // calculate velocity components
            float velocityX = Mathf.Sqrt(velocity) * Mathf.Cos(angleRad);
            float velocityY = Mathf.Sqrt(velocity) * Mathf.Sin(angleRad);

            // create and return velocity vector
            return new Vector2(velocityX, velocityY);
        }
    }
}