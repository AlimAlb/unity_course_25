using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private ObstacleData data;

    private bool didHitPlayer = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (didHitPlayer) return;

        var player = collision.collider.GetComponent<PlayerController>();
        if (player == null) return;

        didHitPlayer = true;

        int dmg = (data != null) ? data.damage : 1;
        player.TakeDamage(dmg);
    }
}