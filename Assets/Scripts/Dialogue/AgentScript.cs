/* https://www.youtube.com/watch?v=SDfEytEjb5o*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentScript : MonoBehaviour
{
    //[SerializeField] Transform target;
    private NavMeshAgent agent;

    //public Transform target;

    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        //target = GameObject.FindGameObjectWithTag("Player").transform;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //RaycastHit2D hitData = Physics2D.Raycast(new Vector2(worldPosition.x, worldPosition.y), Vector2.zero, 0);
        //if (hitData && Input.GetMouseButtonDown(0))
        

        agent.SetDestination(target.transform.position);
        

    }
}
