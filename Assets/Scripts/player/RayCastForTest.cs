using Enemy;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.player
{
    public class RayCastForTest : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit2D = Physics2D.Raycast(screenPos, Vector2.zero);

                if (hit2D)
                {
                    print(hit2D.collider.name);
                    // if has boss component, call OnDamage(1)
                    if (hit2D.collider.GetComponent<Boss>())
                    {
                        hit2D.collider.GetComponent<Boss>().OnDamage(1);
                    }
                    // if has blob component, call OnDamage(1)
                    else
                    {
                        hit2D.collider.GetComponent<IBlob>().TakeDamage(1);
                    }
                }
                else
                {
                    print("no");
                }
            }
        }
    }
}