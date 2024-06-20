using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace WFC
{

    public class Chunk : MonoBehaviour
    {
        public float cellSize = 1.0f;
        public int sizeOfGrid = 3;
        public bool oneChunk = false;
        // public List<string> topEdgeConnectors;
        // public List<string> bottomEdgeConnectors;
        // public List<string> leftEdgeConnectors;
        // public List<string> rightEdgeConnectors;

        // public List<Cell> topEdgeCells;
        // public List<Cell> bottomEdgeCells;
        // public List<Cell> leftEdgeCells;
        // public List<Cell> rightEdgeCells;

        public CellManager cellManager;
        GridManager gridManager;

        public List<Cell>[,] grid;
        public Vector2Int? lastCollapsed;

        void Awake()
        {
            gridManager = new GridManager(cellManager)
            {
                gridSize = sizeOfGrid

            };

        }
        public void InstantiateGridOnScene(List<Cell>[,] grid, float cellSize, Transform parent)
        {
            if (grid == null)
            {
                Debug.LogError("Grid is null and cannot be instantiated.");
                return;
            }
            // int prefabCount = 0;

            int height = grid.GetLength(0);
            int width = grid.GetLength(0);

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {

                    Vector3 cellPosition = parent.position + new Vector3(x * cellSize + cellSize / 2, 0, z * cellSize + cellSize / 2);

                    if (grid[x, z] != null && grid[x, z].Count > 0)
                    {

                        Cell cellPrefabToInstantiate = grid[x, z][0];
                        Cell newCell = Instantiate(cellPrefabToInstantiate, cellPosition, Quaternion.identity, parent);
                        newCell.name = $"{x},{z} {cellPrefabToInstantiate.name}";
                        // prefabCount++;

                    }
                }
            }
            // Debug.Log($"Instantiated {prefabCount} prefabs.");
        }


        public Transform parentTransform;

        public void InstantiateCells()
        {
            grid = gridManager.StartWFC();
            InstantiateGridOnScene(grid, cellSize, parentTransform);
        }
        void Start()
        {
            if (oneChunk)
            {

                InstantiateCells();
            }

        }

        // public void GatherEdgeConnectors()
        // {
        //     topEdgeConnectors = new List<string>();
        //     bottomEdgeConnectors = new List<string>();
        //     leftEdgeConnectors = new List<string>();
        //     rightEdgeConnectors = new List<string>();


        //     foreach (Cell cell in topEdgeCells)
        //     {
        //         topEdgeConnectors.Add(cell.GetConnectorState("top"));
        //     }

        //     // Analogicznie dla pozostałych krawędzi...
        // }

        // public void InitializeEdgeCells()
        // {
        //     topEdgeCells = new List<Cell>();
        //     bottomEdgeCells = new List<Cell>();
        //     leftEdgeCells = new List<Cell>();
        //     rightEdgeCells = new List<Cell>();

        //     int height = grid.GetLength(0);
        //     int width = grid.GetLength(0);

        //     for (int x = 0; x < width; x++)
        //     {
        //         var topCells = grid[x, height - 1];
        //         if (topCells != null && topCells.Count > 0)
        //         {
        //             topEdgeCells.AddRange(topCells);
        //         }
        //     }


        //     for (int x = 0; x < width; x++)
        //     {
        //         var bottomCells = grid[x, 0];
        //         if (bottomCells != null && bottomCells.Count > 0)
        //         {
        //             bottomEdgeCells.AddRange(bottomCells);
        //         }
        //     }


        //     for (int y = 0; y < height; y++)
        //     {
        //         var leftCells = grid[0, y];
        //         if (leftCells != null && leftCells.Count > 0)
        //         {
        //             leftEdgeCells.AddRange(leftCells);
        //         }
        //     }


        //     for (int y = 0; y < height; y++)
        //     {
        //         var rightCells = grid[width - 1, y];
        //         if (rightCells != null && rightCells.Count > 0)
        //         {
        //             rightEdgeCells.AddRange(rightCells);
        //         }
        //     }
        // }



        //DEBUG METHODS


        // public void DisplayEdgeCells()
        // {
        //     Debug.Log("Top Edge Cells:");
        //     foreach (var cell in topEdgeCells)
        //     {
        //         Debug.Log(cell.name);
        //     }

        //     Debug.Log("Bottom Edge Cells:");
        //     foreach (var cell in bottomEdgeCells)
        //     {
        //         Debug.Log(cell.name);
        //     }

        //     Debug.Log("Left Edge Cells:");
        //     foreach (var cell in leftEdgeCells)
        //     {
        //         Debug.Log(cell.name);
        //     }

        //     Debug.Log("Right Edge Cells:");
        //     foreach (var cell in rightEdgeCells)
        //     {
        //         Debug.Log(cell.name);
        //     }
        // }



    }
}



