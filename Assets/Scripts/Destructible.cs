using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TypeObject
{
    CRATE, BOTTLE, NIL
}
public class Destructible : MonoBehaviour
{
    public BrokenPrefabs brokenObjects;
    TypeObject typeObject = TypeObject.NIL;
    private void Start()
    {
        brokenObjects = GameObject.Find("BrokenPrefabs").GetComponent<BrokenPrefabs>();
        string name =gameObject.tag;
        switch (name)
        {
            case "WCrate":
                typeObject = TypeObject.CRATE;
                break;
            case "Bottle":
                typeObject = TypeObject.BOTTLE;
                break;
        }
    }
    private void OnMouseDown()
    {
       
            Instantiate(brokenObjects.GetObject(typeObject) /*brokenObject*/, transform.position, Quaternion.identity);
            Destroy(gameObject);
        
    }
}
