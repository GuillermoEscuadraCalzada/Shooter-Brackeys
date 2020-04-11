using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPrefabs : MonoBehaviour
{
    public GameObject brokenCratePrefab;
    public GameObject brokenBottlePrefab;


    public GameObject GetObject(TypeObject typeObject)
    {
        switch (typeObject)
        {
            case TypeObject.CRATE:
                return brokenCratePrefab;
                
            case TypeObject.BOTTLE:
                return brokenBottlePrefab;
                
            default:
                return null;
        }
    }
}
