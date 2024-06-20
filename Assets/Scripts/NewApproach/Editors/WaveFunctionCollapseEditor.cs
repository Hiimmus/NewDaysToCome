// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// namespace WFC
// {
//     [CustomEditor(typeof(WaveFunctionCollapse))]
//     public class WaveFunctionCollapseEditor : Editor
//     {
//         public override void OnInspectorGUI()
//         {
//             DrawDefaultInspector();
//             WaveFunctionCollapse script = (WaveFunctionCollapse)target;
//             if (GUILayout.Button("Run"))
//             {
//                 script.Run();
//             }
//             if (GUILayout.Button("Show Content Of Grid"))
//             {
//                 script.ShowContentOfGrid();
//             }
//             //  if(GUILayout.Button("CollapseRandomCell"))
//             // {
//             //     script.CollapseRandomCell();
//             // }

//             if (GUILayout.Button("Initialize Entropy Grid"))
//             {
//                 script.InitializeEntropyGrid();
//             }
//             if (GUILayout.Button("Show Concent Of Entropy Grid"))
//             {
//                 script.ShowConcentOfEntropyGrid();
//             }
//             // if(GUILayout.Button("Collapse Lowest Entropy Cell"))
//             // {
//             //     script.CollapseLowestEntropyCell();
//             // }
//             if (GUILayout.Button("Render Grid in Unity"))
//             {
//                 script.RenderGridinUnity();
//             }
//             if (GUILayout.Button("Render With Delay"))
//             {
//                 script.InstantiatePrefabsFromGrid();
//             }
//             if (GUILayout.Button("Delete Prefabs"))
//             {
//                 script.DeletePrefabs();
//             }
//             // if(GUILayout.Button("Start Dual Instantiation"))
//             // {          //Trzeba poprawić ¯\_(ツ)_/¯
//             //     script.StartDualInstantiation();
//             // }
//             if (GUILayout.Button("Restart"))
//             {
//                 script.Restart();
//             }

//             GUILayout.Space(10);
//             // EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
//             // GUILayout.Label("Chunk Control", EditorStyles.boldLabel);

//             // if(GUILayout.Button("GenerateGridOfChunk"))
//             // {
//             //     script.GenerateGridOfChunk();
//             // }
//             //  if(GUILayout.Button("ShowContentOfChunkGrid"))
//             // {
//             //     script.ShowContentOfChunkGrid();
//             // }
//             // if(GUILayout.Button("DrawGizmonsChunk"))
//             // {
//             //     script.ShowContentOfChunkGrid();
//             // }


//         }
//     }
// }