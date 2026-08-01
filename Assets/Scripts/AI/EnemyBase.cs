using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float viewAngle = 120f;
    [SerializeField] private float eyeHeight = 1f;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float catchDistance = 1.5f;

    protected NavMeshAgent agent;
    protected Transform playerTransform;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        playerTransform = GameManager.Instance.PlayerTransform;
    }

    private void Update()
    {
        if (!agent.enabled)
        {
            return;
        }

        if (CanSeePlayer())
        {
            ChasePlayer();
        }
        else
        {
            DoDefaultBehavior();
        }

        CheckForCatch();
    }

    /// <summary>
    /// Checks that the player is inside the detection radius cone
    /// </summary>
    /// <returns></returns>
    protected virtual bool CanSeePlayer()
    {
        Vector3 toPlayer = playerTransform.position - transform.position;
        toPlayer.y = 0f;

        if (toPlayer.magnitude > detectionRadius)
        {
            return false;
        }

        if (Vector3.Angle(transform.forward, toPlayer) > viewAngle / 2f)
        {
            return false;
        }

        return !IsSightBlocked();
    }

    /// <summary>
    /// Ray from this enemys eyes to the players eyes to see if a wall is in the way
    /// </summary>
    /// <returns></returns>
    protected bool IsSightBlocked()
    {
        Vector3 eyePosition = transform.position + Vector3.up * eyeHeight;
        Vector3 playerEyePosition = playerTransform.position + Vector3.up * eyeHeight;
        Vector3 eyeToPlayer = playerEyePosition - eyePosition;

        return Physics.Raycast(eyePosition, eyeToPlayer.normalized, eyeToPlayer.magnitude, obstacleLayer);
    }

    /// <summary>
    /// Walks toward the players position
    /// </summary>
    protected virtual void ChasePlayer()
    {
        agent.updateRotation = true;
        agent.SetDestination(playerTransform.position);
    }

    /// <summary>
    /// Distance check sends the player back to their start positin
    /// </summary>
    protected virtual void CheckForCatch()
    {
        Vector3 toPlayer = playerTransform.position - transform.position;
        toPlayer.y = 0f;

        if (toPlayer.magnitude > catchDistance)
        {
            return;
        }

        GameManager.Instance.ResetPlayer();
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, catchDistance);

        Gizmos.color = Color.yellow;
        float halfAngle = viewAngle / 2f;
        Vector3 leftDirection = Quaternion.Euler(0, -halfAngle, 0) * transform.forward * detectionRadius;
        Vector3 rightDirection = Quaternion.Euler(0, halfAngle, 0) * transform.forward * detectionRadius;

        Gizmos.DrawLine(transform.position, transform.position + leftDirection);
        Gizmos.DrawLine(transform.position, transform.position + rightDirection);
        Gizmos.DrawLine(transform.position + leftDirection, transform.position + rightDirection);
    }

    /// <summary>
    /// Patrol or stationary 
    /// </summary>
    protected virtual void DoDefaultBehavior()
    {

    }
}