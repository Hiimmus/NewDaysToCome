using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{

    public Material[] materials;

    void Start()
    {
       ChangeMaterialFromList(RandomMaterial());

        // GetComponent<Renderer>().material = materials[RandomMaterial()];
    }
    public void ChangeMaterialFromList(int index)
    {
        if (index >= 0 && index < materials.Length)
        {
            GetComponent<Renderer>().material = materials[index];
        }
        else
        {
            Debug.LogError("Indeks poza zakresem listy materiałów");
        }
    }
 

    private int RandomMaterial()
    {

      if(materials != null)
      {
        int randomNumber = Random.Range(0, materials.Length);
        return randomNumber;
      }

        return 0;
    }

}
