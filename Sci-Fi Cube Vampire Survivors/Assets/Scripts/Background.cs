using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform player;
    public Transform[] quads;

    public float tileSize = 15f;

    void Update()
    {
        // Get the bottom left tile position based on player's location
        Vector2 playerPos = player.position;
        int centerX = Mathf.RoundToInt(playerPos.x / tileSize);
        int centerY = Mathf.RoundToInt(playerPos.y / tileSize);

        // Arrange 4 tiles around the player
        int[,] offsets = new int[4, 2]
        {
            { 0,  0 }, // center tile
            { 1,  0 }, // right
            { 0,  1 }, // top
            { 1,  1 }  // top-right
        };

        for (int i = 0; i < quads.Length; i++)
        {
            int offsetX = offsets[i, 0];
            int offsetY = offsets[i, 1];

            float x = (centerX + offsetX) * tileSize;
            float y = (centerY + offsetY) * tileSize;

            quads[i].position = new Vector3(x-tileSize/2, y- tileSize / 2, quads[i].position.z);
        }
    }
}
