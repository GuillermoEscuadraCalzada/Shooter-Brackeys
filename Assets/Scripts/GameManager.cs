using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> pickableWeapons;
    public Transform pickableParent;
    private void Start()
    {
        if (pickableWeapons.Count > 0)
        { //El tamaño de la lista es mayor a cero
            foreach (GameObject weapon in pickableWeapons)
            { ///Por cada una de las armas
                Gun gunScript = weapon.GetComponent<Gun>(); ///Obten el componente de gun
                if (gunScript)
                {
                    gunScript.enabled = false; ///Desactivalo
                }
            }
        }
    }
}
