using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace WFC
{
    public class CellManager : MonoBehaviour
    {
        
        public Transform cellCollection; 
        private  List<Cell> allCells;
        public List<Cell> initializedCells;
        private bool isUsed = false;
        private void CreatListOfCells()
        {
            if (!isUsed)
            {
                allCells = new List<Cell>();
                foreach (Transform child in cellCollection)
                {
                    Cell cell = child.GetComponent<Cell>();
                    if (cell)
                    {
                        allCells.Add(cell);
                    }
                }
                isUsed = true;
                initializedCells = new List<Cell>(allCells);
            }
        }
        public List<Cell> GetInitializedCells()
        {   
            CreatListOfCells();
            return initializedCells; 

        }

        public void ResetListOfCells()
        {
            allCells.Clear();

        }

    }

}