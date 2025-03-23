using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class HexGrid : MonoBehaviour
{
    public GameObject hexPrefab;
    public int width = 10;
    public int height = 10;
    public float hexSize = 1f;
    private readonly Dictionary<Vector2Int, Hex> hexMap = new();

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        float xOffset = hexSize * 2f * 0.75f;
        float zOffset = Mathf.Sqrt(3) * hexSize;

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                float xPos = col * xOffset + (gameObject.transform.position.x - (height * xOffset) / 2);
                float zPos = row * zOffset + (gameObject.transform.position.z - (width * xOffset) / 2);

                if (col % 2 == 1)
                    zPos += zOffset * 0.5f;

                GameObject hex = Instantiate(hexPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity);
                hex.name = $"Hex_{col}_{row}";
                hex.transform.parent = this.transform;
                hex.GetComponent<Hex>().InitializeHex(new Vector2Int(col, row), HexType.Mountain, false, this);
                hexMap[new Vector2Int(col, row)] = hex.GetComponent<Hex>();
            }
        }
        hexMap[new Vector2Int(width / 2, height/2)].SetActive(true);
    }

    private List<Vector2Int> GetHexNeighbors(Vector2Int coords)
    {
        Vector2Int[] evenOffsets = new Vector2Int[]
        {
        new (+1,  0), new (-1,  0),
        new ( 0, +1), new ( 0, -1),
        new (-1, -1), new (+1, -1)
        };

        Vector2Int[] oddOffsets = new Vector2Int[]
        {
        new (+1,  0), new (-1,  0),
        new ( 0, +1), new ( 0, -1),
        new (-1, +1), new (+1, +1)
        };

        Vector2Int[] offsets = coords.x % 2 == 0 ? evenOffsets : oddOffsets;
        List<Vector2Int> neighbors = new();

        foreach (var offset in offsets)
        {
            neighbors.Add(coords + offset);
        }

        return neighbors;
    }

    // 
    public void CheckIfCloseToActive(Hex hex)
    {
        List<Vector2Int> neighbors = GetHexNeighbors(hex.hexCoordinates);

        foreach (var neighborPos in neighbors)
        {
            if (hexMap.ContainsKey(neighborPos))
            {
                if (!hexMap[neighborPos].isActive)
                    hexMap[neighborPos].isClose = true;
            }
        }
    }

    // move all the hex like a wave
    private IEnumerator ShockWaveCoroutine(Vector2Int startCoords)
    {
        if (!hexMap.ContainsKey(startCoords))
            yield break;

        HashSet<Vector2Int> visited = new();
        Queue<Vector2Int> queue = new();
        queue.Enqueue(startCoords);
        visited.Add(startCoords);

        while (queue.Count > 0)
        {
            int layerCount = queue.Count;
            List<Hex> currentLayer = new ();

            for (int i = 0; i < layerCount; i++)
            {
                Vector2Int current = queue.Dequeue();
                if (hexMap.TryGetValue(current, out Hex hex))
                {
                    currentLayer.Add(hex);
                    foreach (Vector2Int neighbor in GetHexNeighbors(current))
                    {
                        if (!visited.Contains(neighbor) && hexMap.TryGetValue(neighbor, out Hex neighborHex) && neighborHex.isActive)
                        {
                            queue.Enqueue(neighbor);
                            visited.Add(neighbor);
                        }
                    }
                }
            }
            foreach (Hex hex in currentLayer)
                hex.UpDown(0.2f, 0.1f);
            yield return new WaitForSeconds(0.05f);
        }
    }


    public void ShockWave(Vector2Int startCoords)
    {
        StartCoroutine(ShockWaveCoroutine(startCoords));
    }
}
