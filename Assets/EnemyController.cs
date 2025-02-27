using UnityEngine;
using Mirror;

public class EnemyController : NetworkBehaviour
{
    public float moveSpeed = 2f;
    private Transform target;

    void Update()
    {
        if (!isServer) return;

        if (target == null) // Keep looking until a target is found
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                target = player.transform;
            }
        }

        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }
}
