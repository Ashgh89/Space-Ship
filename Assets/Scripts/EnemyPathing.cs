using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{

     WaveConfig waveConfig;
     List<Transform> waypoints; // Why Transform? Cause we are looking for position
    
    int waypointIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfig.GetWayPoints();
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        // this is our waveConfig variabel up in the scripts
        this.waveConfig = waveConfig;
    }
    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
            
            
        }
    }
}
