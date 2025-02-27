using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;
    [SyncVar] public int health = 100; // SyncVar synchronizes this across the network

    [Command]
    void CmdAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider hit in hitEnemies)
        {
            if (hit.TryGetComponent<EnemyController>(out var enemy))
            {
                RpcDestroyEnemy(enemy.gameObject); // Tell all clients to destroy the enemy
            }
        }
    }

    [ClientRpc] // Runs on all clients
    void RpcDestroyEnemy(GameObject enemy)
    {
        if (isServer) NetworkServer.Destroy(enemy); // Server actually destroys it
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float rotate = Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;

        transform.Translate(0, 0, move);
        transform.Rotate(0, rotate, 0);

        // Test damage (press Space to take damage)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdTakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.F)) CmdAttack(); // Press F to attack
    }

    [Command] // Commands run on the server
    void CmdTakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0; // Could add death logic here
        }
    }
}
