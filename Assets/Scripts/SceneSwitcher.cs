using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSwitcher : MonoBehaviour
{

     public Camera cameraToControl;
 void Update()
    {
         if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            CustomPostProcess customPostProcess = cameraToControl.GetComponent<CustomPostProcess>();
            if (customPostProcess != null)
            {
                customPostProcess.enabled = !customPostProcess.enabled;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("NewApproach");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Scene2");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("Scene3");
        }
    }
}
