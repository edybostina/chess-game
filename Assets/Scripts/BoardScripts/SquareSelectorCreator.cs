using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSelectorCreator : MonoBehaviour
{
    [SerializeField] private Material freeSquareMaterial;
    [SerializeField] private Material opponentSquareMaterial;
    [SerializeField] private GameObject selectorPrefab;
    private List<GameObject> instantiatedSelectors = new List<GameObject>();

    Vector3 offset = new Vector3(86.5f, -348.3f, 127.4f);

    public void ShowSelection(Dictionary<Vector3, bool> squareData)
    {
        ClearSelection();
        foreach (var data in squareData)
        {
            GameObject selector = Instantiate(selectorPrefab, data.Key + offset, Quaternion.identity);
            instantiatedSelectors.Add(selector);

            MeshRenderer[] cubes = selector.GetComponentsInChildren<MeshRenderer>();
            Material newMat = data.Value ? freeSquareMaterial : opponentSquareMaterial;
            foreach (var c in cubes) 
            {
                c.material = newMat;
            }
          
          
            
        }
    }

    public void ClearSelection()
    {
        for (int i = 0; i < instantiatedSelectors.Count; i++)
        {
            Destroy(instantiatedSelectors[i]);
        }
    }
}