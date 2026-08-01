using UnityEngine;

public class EnemyStationary : EnemyBase
{
    [SerializeField] private float sweepAngle = 60f;
    [SerializeField] private float sweepSpeed = 30f;
    [SerializeField] private float arriveDistance = 0.5f;

    private Vector3 standPosition;
    private float standYaw;
    private float sweepTimer;

    protected override void Start()
    {
        base.Start();

        standPosition = transform.position;
        standYaw = transform.eulerAngles.y;
    }

    /// <summary>
    /// Walks back to the starting post if the enemy wandered off chasing,
    /// otherwise rotates back and forth around its starting angle.
    /// </summary>
    protected override void DoDefaultBehavior()
    {
        if (Vector3.Distance(transform.position, standPosition) > arriveDistance)
        {
            agent.updateRotation = true;
            agent.SetDestination(standPosition);
            return;
        }

        agent.updateRotation = false;
        sweepTimer += Time.deltaTime * sweepSpeed;

        float yawOffset = Mathf.PingPong(sweepTimer, sweepAngle * 2f) - sweepAngle;
        transform.rotation = Quaternion.Euler(0f, standYaw + yawOffset, 0f);
    }
}