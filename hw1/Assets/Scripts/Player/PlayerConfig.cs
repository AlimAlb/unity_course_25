using UnityEngine;

[CreateAssetMenu(menuName = "Runner/Player Config", fileName = "PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [Header("Movement")]
    [Min(0.1f)] public float forwardSpeed = 8f;
    [Min(0.1f)] public float laneChangeSpeed = 12f;

    [Header("Lanes (X positions)")]
    public float[] lanesX = new float[] { -2f, 0f, 2f };

    [Header("Jump")]
    [Min(0.1f)] public float jumpImpulse = 7f;
    [Range(0, 5)] public int maxJumps = 2;
    [Min(0.01f)] public float groundCheckRadius = 0.25f;
    public LayerMask groundMask;

    [Header("Health")]
    [Min(1)] public int maxHealth = 3;
}