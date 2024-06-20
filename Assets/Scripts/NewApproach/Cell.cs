using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC
{

    public class Cell : MonoBehaviour
    {
        public Connector connectors = new Connector();
        public bool isUsed = true;
        public float probability;
        public bool randomRotate = false;
        public float rotationAngle = 0.0f;

        void Start()
        {
            RandomRotate();
            transform.Rotate(0, rotationAngle, 0);

        }
        // void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red; // Ustaw kolor kropki
        //     float size = 0.1f; // Ustaw rozmiar kropki

        //     // Rysuj kropkę
        //     Gizmos.DrawSphere(transform.position, size);
        // }


        private void RandomRotate()
        {
            int tmp = (int)Random.Range(0, 4);
            // Debug.Log("Warotść tmp: " + tmp);
            if (randomRotate)
            {
                switch (tmp)
                {
                    case 1:
                        transform.Rotate(0, 90, 0);
                        break;
                    case 2:
                        transform.Rotate(0, 180, 0);
                        break;
                    case 3:
                        transform.Rotate(0, 270, 0);
                        break;
                    default:
                        transform.Rotate(0, 0, 0);
                        break;
                }
            }

        }
        public string GetConnectorState(string direction)
        {
            switch (direction)
            {
                case "frontDir":
                    return connectors.frontDir;
                case "backDir":
                    return connectors.backDir;
                case "leftDir":
                    return connectors.leftDir;
                case "rightDir":
                    return connectors.rightDir;
                default:
                    return null;
            }
        }




    }
}