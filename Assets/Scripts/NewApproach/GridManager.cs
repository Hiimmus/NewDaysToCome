using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace WFC
{
    public class GridManager
    {
        public int gridSize = 3;
        public CellManager cellManager;

        public List<Cell>[,] grid;
        public float[,] entropyGrid;  //raczej nie public
        public Vector2Int? lastCollapsed;
        // public Vector3 spacing = new Vector3(1, 1, 1);
        public Vector2 spacing = new Vector2(8, 8);  // Odstęp między komórkami


        public GridManager(CellManager cellManager)
        {
            this.cellManager = cellManager;
        }
        public void CreateGrid()
        {
            grid = new List<Cell>[gridSize, gridSize];

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    grid[x, y] = new List<Cell>(cellManager.GetInitializedCells());
                }
            }
        }

        public List<Cell>[,] CreatAndReturnGrid()
        {
            grid = new List<Cell>[gridSize, gridSize];

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    grid[x, y] = new List<Cell>(cellManager.GetInitializedCells());
                }
            }
            return grid;
        }

        public List<Cell>[,] StartWFC()
        {
            CreateGrid();
            Vector2Int? collapsedCellCoords = CollapseRandomCell();
            if (collapsedCellCoords.HasValue)
            {
                UpdateNeighbours(collapsedCellCoords.Value.x, collapsedCellCoords.Value.y);
            }
            InitializeEntropyGrid();
            
            while (!IsAllCollapsed())
            {
                UpdateEntropyGrid();
                Vector2Int? collapsedCell = CollapseLowestEntropyCell();

                if (collapsedCell.HasValue)
                {
                    UpdateNeighbours(collapsedCell.Value.x, collapsedCell.Value.y);
                }
            }
            return grid;
        }
        public Vector2Int? CollapseRandomCell()
        {
            int randomX = Random.Range(0, gridSize);
            int randomY = Random.Range(0, gridSize);

            List<Cell> possibleCells = grid[randomX, randomY];

            if (possibleCells.Count > 0)
            {
                Cell selectedCell = possibleCells[Random.Range(0, possibleCells.Count)];

                grid[randomX, randomY].Clear();
                grid[randomX, randomY].Add(selectedCell);

                // Debug.Log($"Komórka w ({randomX}, {randomY}) została skolapsowana do prefabu: {selectedCell.gameObject.name}");
                lastCollapsed = new Vector2Int(randomX, randomY);
                UpdateNeighbours(randomX, randomY); // aktualizacja sąsiednich komórek.
                // Debug.Log($"WARTOŚCI komórek X{randomX} i Y{randomY}");
                return lastCollapsed;
                //  return new Vector2Int(randomX, randomY); // współrzędne skolapsowanej komórki

            }
            return null;
        }

        public void UpdateNeighbours(int x, int y)
        {
            Cell collapsedCell = grid[x, y][0];  // Zwraca skolapsowaną komórkę

            // Debug.Log($"Skolapsowana komórka w ({x}, {y}) to: {collapsedCell.gameObject.name}");

            // Sprawdzanie sąsiadów dla wszystkich 4 kierunków
            if (x > 0) UpdateCellBasedOnConnector(x - 1, y, collapsedCell.connectors.leftDir, "rightDir");
            /*  if (x > 0): sprawdza, czy skolapsowana komórka nie jest na skrajnie 
                lewej krawędzi siatki (czyli nie ma współrzędnej x równą 0). 
                Jeśli nie jest, aktualizujemy sąsiada po lewej stronie (x - 1).
            */

            if (x < gridSize - 1) UpdateCellBasedOnConnector(x + 1, y, collapsedCell.connectors.rightDir, "leftDir");
            /*
                if (x < gridSize - 1): sprawdza, czy skolapsowana 
                komórka nie jest na skrajnie prawej krawędzi siatki.
                Jeśli nie jest, aktualizujemy sąsiada po prawej stronie (x + 1).
            */
            if (y > 0) UpdateCellBasedOnConnector(x, y - 1, collapsedCell.connectors.backDir, "frontDir");
            /*
               if (y > 0): sprawdza, czy skolapsowana komórka nie jest na skrajnie
                dolnej krawędzi siatki (przyjmując, że dolna krawędź to y równa 0).
               Jeśli nie jest, aktualizujemy sąsiada poniżej (y - 1)
            */
            if (y < gridSize - 1) UpdateCellBasedOnConnector(x, y + 1, collapsedCell.connectors.frontDir, "backDir");
            /*
               if (y < gridSize - 1): sprawdza, czy skolapsowana komórka nie jest
                na skrajnie górnej krawędzi siatki. Jeśli nie jest, aktualizujemy
                 sąsiada powyżej (y + 1).
            */
        }

        private void UpdateCellBasedOnConnector(int x, int y, string dirValue, string oppositeDir)
        {
            /*
            Eliminowanie niemożliwych stanów komórek na podstawie stanu komórki sąsiedniej
            */
            List<Cell> possibleCells = grid[x, y];
            for (int i = possibleCells.Count - 1; i >= 0; i--)
            {
                Cell cell = possibleCells[i];
                string connectorValue = GetConnectorValue(cell, oppositeDir);
                if (connectorValue != dirValue)
                {
                    possibleCells.RemoveAt(i);
                }
            }
        }

        private string GetConnectorValue(Cell cell, string direction)
        {
            switch (direction)
            {
                case "frontDir": return cell.connectors.frontDir;
                case "backDir": return cell.connectors.backDir;
                case "leftDir": return cell.connectors.leftDir;
                case "rightDir": return cell.connectors.rightDir;
                default: return null;
            }
        }

        public Vector2Int? FindLowestEntropyCell()
        {
            Vector2Int? lowestEntropyCoord = null;
            float minEntropy = float.MaxValue;

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (entropyGrid[x, y] < minEntropy && grid[x, y].Count > 1) // Sprawdzamy czy komórka ma więcej niż 1 możliwość
                    {
                        minEntropy = entropyGrid[x, y];
                        lowestEntropyCoord = new Vector2Int(x, y);
                    }
                }
            }
            return lowestEntropyCoord;
        }
        public Vector2Int? CollapseLowestEntropyCell()
        {
            Vector2Int? lowestEntropyCoord = FindLowestEntropyCell();
            if (lowestEntropyCoord.HasValue)
            {
                List<Cell> possibleCells = grid[lowestEntropyCoord.Value.x, lowestEntropyCoord.Value.y];

                // Wybór komórki na podstawie jej prawdopodobieństwa
                Cell selectedCell = ChooseCellBasedOnProbability(possibleCells);

                grid[lowestEntropyCoord.Value.x, lowestEntropyCoord.Value.y].Clear();
                grid[lowestEntropyCoord.Value.x, lowestEntropyCoord.Value.y].Add(selectedCell);

                // Debug.Log($"Komórka w ({lowestEntropyCoord.Value.x}, {lowestEntropyCoord.Value.y}) została skolapsowana do prefabu: {selectedCell.gameObject.name}");
                lastCollapsed = lowestEntropyCoord;
                return lastCollapsed;

            }
            return null;
        }

        public Cell ChooseCellBasedOnProbability(List<Cell> possibleCells)
        {
            float totalProbability = possibleCells.Sum(cell => cell.probability);
            float randomValue = Random.Range(0, totalProbability);

            float accumulatedProbability = 0;
            foreach (Cell cell in possibleCells)
            {
                accumulatedProbability += cell.probability;
                if (randomValue <= accumulatedProbability)
                {
                    return cell;
                }
            }
            return possibleCells.Last();  // Fallback, nie powinno się zdarzyć, ale dla pewności
        }

        public bool IsAllCollapsed()
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (grid[x, y].Count > 1)
                        return false;  // Komórka nie została jeszcze zkolapsowana
                }
            }
            return true;
        }

        public void UpdateEntropyGridForCell(int x, int y)
        {
            if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
            {
                entropyGrid[x, y] = CalculateEntropy(x, y);
            }
        }

        public void UpdateEntropyGrid()
        {
          
            UpdateEntropyGridForCell(lastCollapsed.Value.x, lastCollapsed.Value.y);
            UpdateEntropyGridForCell(lastCollapsed.Value.x + 1, lastCollapsed.Value.y);
            UpdateEntropyGridForCell(lastCollapsed.Value.x - 1, lastCollapsed.Value.y);
            UpdateEntropyGridForCell(lastCollapsed.Value.x, lastCollapsed.Value.y + 1);
            UpdateEntropyGridForCell(lastCollapsed.Value.x, lastCollapsed.Value.y - 1);
        }



        //Sprawdzanie entropi dla Gridu 
        public void InitializeEntropyGrid()
        {
            entropyGrid = new float[gridSize, gridSize];
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    entropyGrid[x, y] = CalculateEntropy(x, y);
                }
            }
        }

        public float CalculateEntropy(int x, int y)
        {
            List<Cell> possibleCells = grid[x, y];
            float totalProbability = 0;
            foreach (var cell in possibleCells)
            {
                totalProbability += cell.probability;
            }
            return totalProbability / possibleCells.Count;  // Średnia wartość probability dla wszystkich możliwych komórek
        }




        // Metoda do instancjonowania prefabów na podstawie siatki
        public void InstantiatePrefabsFromGrid(Transform parentTransform)
        {
            if (grid == null) return;

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    List<Cell> possibleCells = grid[x, y];
                    if (possibleCells != null && possibleCells.Count > 0)
                    {
                        
                        Cell selectedCellPrefab = possibleCells[0];

                        if (selectedCellPrefab)
                        {

                        
                            Vector3 position = new Vector3(x * spacing.x, 0, y * spacing.y);
                            // Instancjonujemy prefab komórki na odpowiedniej pozycji
                            GameObject cellInstance = GameObject.Instantiate(selectedCellPrefab.gameObject, position, Quaternion.identity, parentTransform);
                            // Zmiana nazwy instancji prefabrykatu
                            cellInstance.name = $"{selectedCellPrefab.name}_X{x}_Y{y}";
                        }
                    }
                }
            }
        }

        public IEnumerator InstantiatePrefabsFromGridCoroutine(Transform parentTransform, float delay)
        {
            if (grid == null) yield break;

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    List<Cell> possibleCells = grid[x, y];
                    if (possibleCells != null && possibleCells.Count > 0)
                    {
                        Cell selectedCellPrefab = possibleCells[0];

                        if (selectedCellPrefab)
                        {
                            Vector3 position = new Vector3(x * spacing.x, 0, y * spacing.y);
                            GameObject cellInstance = GameObject.Instantiate(selectedCellPrefab.gameObject, position, Quaternion.identity, parentTransform);
                            cellInstance.name = $"{selectedCellPrefab.name}_X{x}_Y{y}";

                         
                            yield return new WaitForSeconds(delay);
                        }
                    }
                }
            }
        }

        public IEnumerator InstantiatePrefabsFromGridCoroutine(Transform parentTransform, float delay, int startX, int startY, int endX, int endY)
        {
            if (grid == null) yield break;

            for (int x = startX; x != endX; x = x < endX ? x + 1 : x - 1)
            {
                for (int y = startY; y != endY; y = y < endY ? y + 1 : y - 1)
                {
                    List<Cell> possibleCells = grid[x, y];
                    if (possibleCells != null && possibleCells.Count > 0)
                    {
                        Cell selectedCellPrefab = possibleCells[0];

                        if (selectedCellPrefab)
                        {
                            Vector3 position = new Vector3(x * spacing.x, 0, y * spacing.y);
                            GameObject cellInstance = GameObject.Instantiate(selectedCellPrefab.gameObject, position, Quaternion.identity, parentTransform);
                            cellInstance.name = $"{selectedCellPrefab.name}_X{x}_Y{y}";

                            yield return new WaitForSeconds(delay);
                        }
                    }
                }
            }
        }
        public void DestroyAllChildPrefabs(GameObject parentObject)
        {
           
            int childCount = parentObject.transform.childCount;
            GameObject[] children = new GameObject[childCount];

            for (int i = 0; i < childCount; i++)
            {
               
                children[i] = parentObject.transform.GetChild(i).gameObject;
            }

          
            foreach (GameObject child in children)
            {
                GameObject.Destroy(child);
            }
        }


        public void ShowContentOfEntropyGrid()
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    Debug.Log($"Entropia dla komórki ({x}, {y}) wynosi: {entropyGrid[x, y]}");
                }
            }
        }

        public void ShowContentOfGrid()
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    ShowCellContent(x, y);
                }
            }
        }

    
        public void ShowContentOfGrid(int row, int column)
        {
            if (row >= 0 && row < gridSize && column >= 0 && column < gridSize)
            {
                ShowCellContent(row, column);
            }
            else
            {
                Debug.LogError("Nieprawidłowe indeksy komórki!");
            }
        }

        private void ShowCellContent(int x, int y)
        {
            string content = $"Komórka ({x},{y}): ";
            foreach (var cell in grid[x, y])
            {
                content += $"{cell.gameObject.name}, "; 
            }
            Debug.Log(content);
        }

    }
}
