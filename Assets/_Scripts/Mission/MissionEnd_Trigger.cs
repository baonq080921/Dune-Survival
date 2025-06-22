using UnityEngine;

public class MissionEnd_Trigger : MonoBehaviour
{
    private GameObject player;
    private float distance;
    [SerializeField]


    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        caculateDistance();
    }

    private void caculateDistance()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        Debug.Log(distance);
    }


    public float DistanceToFinishLine() => distance;


    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, player.transform.position);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != player)
            return;
        if (MissionManager.instance.MissionCompleted())
        {
            GameManager.instance.GameWinner();     
        }
            
    }
}
