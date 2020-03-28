using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefabs : MonoBehaviour
{
    private readonly static Prefabs instance = new Prefabs();
    private Prefabs()
    {

    }
    public static Prefabs Instance {
        get {
            return instance;
        }
    }

    public GameObject crate;
    public GameObject bottle;
}
