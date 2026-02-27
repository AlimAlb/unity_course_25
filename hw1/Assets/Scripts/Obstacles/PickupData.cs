using UnityEngine;

public enum PickupType { Score, Heal }

[CreateAssetMenu(menuName = "Runner/Pickup Data")]
public class PickupData : ScriptableObject
{
    public PickupType type = PickupType.Score;
    public int amount = 10;
}