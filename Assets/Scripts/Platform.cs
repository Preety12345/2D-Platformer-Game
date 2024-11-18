using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private Transform visual;
    [SerializeField] private float moveSpeed;
    private int currentWayPoint = 0;

    private void Update()
    {
        visual.position = Vector2.MoveTowards(visual.position, wayPoints[currentWayPoint].position, moveSpeed* Time.deltaTime);
        if(Vector2.Distance(visual.position, wayPoints[currentWayPoint].position) < 0.1f) currentWayPoint = (currentWayPoint + 1) % wayPoints.Length;
    }
}
