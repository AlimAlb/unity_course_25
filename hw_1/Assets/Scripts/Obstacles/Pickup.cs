using UnityEngine;

public class Pickup : MonoBehaviour{
    [SerializeField] private PickupData data;

    private void OnTriggerEnter(Collider other){
        var player = other.GetComponent<PlayerController>();
        if (player == null) return;

        if (data != null){
            if (data.type == PickupType.Score)
                GameManager.I?.AddScore(data.amount);

            if (data.type == PickupType.Heal)
                player.Heal(data.amount);
        }
        Destroy(gameObject); 
    }
}