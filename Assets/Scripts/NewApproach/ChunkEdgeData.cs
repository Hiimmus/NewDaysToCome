using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WFC
{
   public class ChunkEdgeData
{
    public List<string> TopEdgeConnectors;
    public List<string> BottomEdgeConnectors;
    public List<string> LeftEdgeConnectors; 
    public List<string> RightEdgeConnectors;

    public ChunkEdgeData()
    {
        TopEdgeConnectors = new List<string>();
        BottomEdgeConnectors = new List<string>();
        LeftEdgeConnectors = new List<string>();
        RightEdgeConnectors = new List<string>();
    }

    
}

}