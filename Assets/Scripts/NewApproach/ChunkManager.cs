using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WFC
{

    public class ChunkManager : MonoBehaviour
    {
        public Chunk chunkPrefab;
        public GameObject player;
        public int viewDistance = 3;
        private Dictionary<Vector3, Chunk> existingChunks = new Dictionary<Vector3, Chunk>();

        private Vector3 lastPlayerPosition;
        private int chunkSize;

        private void Start()
        {
            chunkSize = chunkPrefab.sizeOfGrid * (int)chunkPrefab.cellSize;
            lastPlayerPosition = player.transform.position;
            InitializeChunksAroundPlayer();
        }

    //     public bool HasNeighbor(Vector3 chunkPosition, Direction direction)
    // {
    //     Vector3 offset = Vector3.zero;
    //     switch (direction)
    //     {
    //         case Direction.Top:
    //             offset = new Vector3(0, 0, chunkSize);
    //             break;
    //         case Direction.Bottom:
    //             offset = new Vector3(0, 0, -chunkSize);
    //             break;
    //         case Direction.Left:
    //             offset = new Vector3(-chunkSize, 0, 0);
    //             break;
    //         case Direction.Right:
    //             offset = new Vector3(chunkSize, 0, 0);
    //             break;
    //     }
    //     Vector3 neighborPosition = chunkPosition + offset;
    //     return existingChunks.ContainsKey(neighborPosition);
    // }
        
        private void Update()
        {
            if (Vector3.Distance(lastPlayerPosition, player.transform.position) > chunkSize)
            {
                UpdateChunks();
                lastPlayerPosition = player.transform.position;
            }
        }

        private void InitializeChunksAroundPlayer()
        {
            Vector3 playerPosition = player.transform.position;
            Vector2 playerChunkPosition = new Vector2(
                Mathf.FloorToInt(playerPosition.x / chunkSize),
                Mathf.FloorToInt(playerPosition.z / chunkSize)
            );

            for (int x = -viewDistance; x <= viewDistance; x++)
            {
                for (int z = -viewDistance; z <= viewDistance; z++)
                {
                    Vector3 chunkPosition = new Vector3((playerChunkPosition.x + x) * chunkSize, 0, (playerChunkPosition.y + z) * chunkSize);
                    CreateChunk(chunkPosition);
                }
            }
        }

        private void RemoveChunksOutsideOfView()
        {
            List<Vector3> chunksToRemove = new List<Vector3>();

            foreach (var chunkEntry in existingChunks)
            {
                if (!IsChunkInPlayerView(chunkEntry.Key))
                {
                    chunksToRemove.Add(chunkEntry.Key);
                }
            }

            foreach (var position in chunksToRemove)
            {
                RemoveChunk(position);
            }
        }

        private void UpdateChunks()
        {
            Vector3 playerPosition = player.transform.position;
            Vector2 playerChunkPosition = new Vector2(
                Mathf.FloorToInt(playerPosition.x / chunkSize),
                Mathf.FloorToInt(playerPosition.z / chunkSize)
            );

            for (int x = (int)playerChunkPosition.x - viewDistance; x <= playerChunkPosition.x + viewDistance; x++)
            {
                for (int z = (int)playerChunkPosition.y - viewDistance; z <= playerChunkPosition.y + viewDistance; z++)
                {
                    Vector3 chunkPosition = new Vector3(x * chunkSize, 0, z * chunkSize);

                    if (!IsChunkInPlayerView(chunkPosition))
                    {
                        RemoveChunk(chunkPosition);
                    }
                    else if (!DoesChunkExist(chunkPosition))
                    {
                        CreateChunk(chunkPosition);
                    }
                }
            }
            RemoveChunksOutsideOfView();
        }

        private bool IsChunkInPlayerView(Vector3 chunkPosition)
        {
            Vector3 playerPosition = player.transform.position;
            float distanceX = Mathf.Abs(chunkPosition.x - playerPosition.x);
            float distanceZ = Mathf.Abs(chunkPosition.z - playerPosition.z);

            return distanceX <= viewDistance * chunkSize && distanceZ <= viewDistance * chunkSize;
        }

        private bool DoesChunkExist(Vector3 position)
        {
           
            return existingChunks.ContainsKey(position);
        }

        private void CreateChunk(Vector3 position)
        {
            if (!existingChunks.ContainsKey(position))
            {
                Chunk newChunk = Instantiate(chunkPrefab, position, Quaternion.identity);
                newChunk.InstantiateCells();
                newChunk.transform.parent = this.transform;
                existingChunks[position] = newChunk;
            }
        }

        private void RemoveChunk(Vector3 position)
        {
            if (existingChunks.TryGetValue(position, out Chunk chunk))
            {
                Destroy(chunk.gameObject);
                existingChunks.Remove(position);
            }
        }


    }

public enum Direction
{
    Top,
    Bottom,
    Left,
    Right
    
}

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.blue;

    //     for (int x = 0; x < gridSize; x++)
    //     {
    //         for (int z = 0; z < gridSize; z++)
    //         {
    //             Vector3 chunkPosition = new Vector3(x * chunkPrefab.width * chunkPrefab.cellSize, 0, z * chunkPrefab.height * chunkPrefab.cellSize);
    //             Vector3 chunkSize = new Vector3(chunkPrefab.width * chunkPrefab.cellSize, 0, chunkPrefab.height * chunkPrefab.cellSize);

    //             Gizmos.DrawWireCube(chunkPosition + chunkSize / 2, chunkSize);
    //         }
    //     }
    // }
}





