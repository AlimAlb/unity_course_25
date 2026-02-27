using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;   // Player
    [SerializeField] private Vector3 offset = new Vector3(0f, 6f, -10f);
    [SerializeField] private float smooth = 10f;

    private Vector3 vel;

    private void LateUpdate()
{
    if (target == null) return;
    transform.position = target.position + offset;
}
}