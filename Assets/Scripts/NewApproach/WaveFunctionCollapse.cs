using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace WFC
{

    public class WaveFunctionCollapse : MonoBehaviour
    {
        public int sizeOfGrid = 5;

        public float delay = 0.5f;
        public CellManager cellManager;
        public GameObject parentObject;
        GridManager gridManager;
        public List<Cell>[,] grid;

        private void Awake() {
            
                        gridManager = new GridManager(cellManager)
            {
                gridSize = sizeOfGrid

            };
        }
        public void Run()
        {
            grid = gridManager.StartWFC();

            // while (!gridManager.IsAllCollapsed())
            // {
            //     gridManager.UpdateEntropyGrid();
            //     Vector2Int? collapsedCell = gridManager.CollapseLowestEntropyCell();

            //     if (collapsedCell.HasValue)
            //     {
            //         gridManager.UpdateNeighbours(collapsedCell.Value.x, collapsedCell.Value.y);
            //     }
            // }
            // gridManager.StartWFC();


        }
       
        // public void SetupWFC()
        // {
        //     // cellManager.GetInitializedCells(); Niepotrzebne ;) 
        //     gridManager.CreateGrid();
        //     Vector2Int? collapsedCellCoords = gridManager.CollapseRandomCell();

        //     if (collapsedCellCoords.HasValue)
        //     {
        //         gridManager.UpdateNeighbours(collapsedCellCoords.Value.x, collapsedCellCoords.Value.y);
        //     }
        //     gridManager.InitializeEntropyGrid();
        // }

        public void InstantiatePrefabsFromGrid() 
        {
            StartCoroutine(gridManager.InstantiatePrefabsFromGridCoroutine(this.transform, this.delay));
        }

        public void StartDualInstantiation()
        {
            // Rozpoczyna od górnego lewego rogu do dolnego prawego
            StartCoroutine(gridManager.InstantiatePrefabsFromGridCoroutine(this.transform, this.delay, 0, 0, gridManager.gridSize, gridManager.gridSize));
            // Rozpoczyna od dolnego prawego rogu do górnego lewego
            StartCoroutine(gridManager.InstantiatePrefabsFromGridCoroutine(this.transform, this.delay, gridManager.gridSize - 1, gridManager.gridSize - 1, -1, -1));
        }

        public void DeletePrefabs()
        {
            gridManager.DestroyAllChildPrefabs(parentObject);
        }
        public void RenderGridinUnity()
        {
            gridManager.InstantiatePrefabsFromGrid(this.transform);
        }
        public void CollapseLowestEntropyCell()
        {
            gridManager.CollapseLowestEntropyCell();
        }
        public void ShowConcentOfEntropyGrid()
        {

            gridManager.ShowContentOfEntropyGrid();
        }

        public void InitializeEntropyGrid()
        {
            gridManager.InitializeEntropyGrid();
        }


        public void ShowContentOfGrid()
        {
            gridManager.ShowContentOfGrid();
        }

        public void CollapseRandomCell()
        {
            gridManager.CollapseRandomCell();
        }


        public void Restart()
        {
            cellManager.ResetListOfCells();
        }

    }

}

