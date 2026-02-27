using UnityEngine;

public class GroundTileManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] tiles;

    [SerializeField] private float tileLength = 50f;

    private int nextTileIndex = 0;

    private void FixedUpdate()
    {
    if (player == null || tiles.Length == 0) return;

    if (player.position.z > tiles[nextTileIndex].position.z + tileLength)
    {
        MoveTileToFront(nextTileIndex);
        nextTileIndex = (nextTileIndex + 1) % tiles.Length;
    }
    }

    private void MoveTileToFront(int index)
    {
        float farthestZ = GetFarthestTileZ();
        Vector3 pos = tiles[index].position;
        pos.z = farthestZ + tileLength;
        tiles[index].position = pos;
    }

    private float GetFarthestTileZ()
    {
        float maxZ = tiles[0].position.z;
        for (int i = 1; i < tiles.Length; i++)
        {
            if (tiles[i].position.z > maxZ)
                maxZ = tiles[i].position.z;
        }
        return maxZ;
    }
}