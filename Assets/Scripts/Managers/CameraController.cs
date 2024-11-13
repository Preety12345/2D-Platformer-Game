using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController player;
    public float offset;
    public float offsetSmoothning;
    private Vector3 playerPosition;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>();

    }

    void FixedUpdate()
    {   
        playerPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        if( player.HorizontalInput> 0f) playerPosition = new Vector3(playerPosition.x + offset, playerPosition.y, playerPosition.z);
        else playerPosition = new Vector3(playerPosition.x - offset, playerPosition.y, playerPosition.z);
        transform.position= Vector3.Lerp(transform.position, playerPosition, offsetSmoothning * Time.deltaTime);
    }
}
