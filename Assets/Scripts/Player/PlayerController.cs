using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float maxClickDistance = 500f;
    [SerializeField] private float navMeshSampleRadius = 2f;

    private Vector3 startPosition;

    private NavMeshAgent agent;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        PlayerInput playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        startPosition = transform.position;
    }

    /// <summary>
    /// Raycasts from the camera through the mouse cursor
    /// </summary>
    /// <param name="context"></param>
    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        if (cam == null || !agent.enabled)
        {
            return;
        }

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mouseScreenPosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, maxClickDistance, groundMask))
        {
            return;
        }

        if (!NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, navMeshSampleRadius, NavMesh.AllAreas))
        {
            return;
        }

        agent.SetDestination(navHit.position);
    }

    /// <summary>
    /// Teleports the player back to startposition and resets path
    /// </summary>
    public void ResetPosition()
    {
        agent.ResetPath();
        agent.Warp(startPosition);
    }
}