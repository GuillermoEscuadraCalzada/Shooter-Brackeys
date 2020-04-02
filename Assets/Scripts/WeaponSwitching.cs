using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Corregir y mejorar cambio de arma si la lista del jugador está llena
/// </summary>

public class WeaponSwitching : MonoBehaviour
{
    [Header("Game Manager")]
    public GameManager gameManager;
    [Header("Lista de armas")]
    /// <summary>
    /// Almacena una lista de armas
    /// </summary>
    public List<GameObject> weaponsList;
    [Header("Script del jugador")]
    /// <summary>
    /// Guarda una variable del jugador
    /// </summary>
    public GameObject player;
    [Header("UI")]
    /// <summary>
    /// Guarda una imagen de notificaciones
    /// </summary>
    public Image notificationsImage;
    /// <summary>
    /// Guarda un texto para las notificaciones
    /// </summary>
    public Text notificationsText;
    [Header("Tamaño máximo del contenedor")]
    /// <summary>
    /// Tamaño que el contenedor debe de tener
    /// </summary>
    public int weaponHolderSize = 2;
    [Header("Arma actual")]
    public int selectedWeapon = 0;

    private float weaponDistance = 2f;

    
    // Start is called before the first frame update
    void Start()
    {
        weaponsList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        PickUpGun();
        ScrollToChange();
        DropGun();
    }


    /// <summary>
    /// Recoge un arma del suelo
    /// </summary>
    void PickUpGun()
    {
        if(gameManager.pickableWeapons.Count> 0)
        {
            ///Por cada uno de los objetos con el tag "Weapon" realiza lo siguiente
            foreach (GameObject obj in gameManager.pickableWeapons)
            {
                float distance = Vector3.Distance(player.transform.position, obj.transform.position);
                ///Pregunta si la distancia es menor a la distancia de un arma
                if (distance < weaponDistance)
                {
                    Debug.Log(distance);
                    ///Activa la imagen y texto del UI para que el jugador sepa qué hacer cerca de las imágenes
                    notificationsImage.enabled = true;
                    notificationsText.enabled = true;
                    notificationsText.text = "Press F to pick up Gun"; ///Cambia el texto de las notificaciones
                                                                       ///El usuario presiona la tecla F
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        obj.GetComponent<Collider>().enabled = false; //Desactiva cualquier collider que el objeto tenga
                        Destroy(obj.GetComponent<Rigidbody>()); ///Destruye el rigidbody
                        obj.GetComponent<Gun>().enabled = true;
                        ///Si el contado de armas es menor a las que se pueden agarrar
                        if (weaponsList.Count <= weaponHolderSize)
                        {
                            obj.transform.parent = transform; ///Haz que el parent de este Objeto sea 
                            obj.transform.position = transform.position;
                            obj.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
                            weaponsList.Add(obj); ///Se añade a la lista de armas   
                            gameManager.pickableWeapons.Remove(obj); //Quita el arma de la lista del game manager
                        }
                        else
                        {
                            SwitchGuns(obj);
                        }
                    }
                }
                else
            {
                notificationsImage.enabled = false;
                notificationsText.text = ""; ///Cambia el texto de las notificaciones
            }
    
            }
        }
    }

    /// <summary>
    /// Intercambia el arma actual con una que se encuentre en el suelo
    /// </summary>
    /// <param name="newGun"></param>
    void SwitchGuns(GameObject newGun)
    {
        int i = 0;
        foreach (GameObject weapons in weaponsList)
        {
            if (i.Equals(selectedWeapon))
            {
                weapons.SetActive(true);
                weapons.GetComponent<Gun>().enabled = false;
                gameManager.pickableWeapons.Add(weapons);
                weapons.transform.position = newGun.transform.position;
                newGun.transform.parent = transform;
                newGun.transform.position = transform.position;
                newGun.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
                weaponsList.Remove(weapons);
            }
            i++;
        }
    }

    void DropGun()
    {
        if(weaponsList.Count > 0)
        {
            
        }
    }

    void ScrollToChange()
    {
        ///El tamaño de la lista de armas es mayor a cero
        if (weaponsList.Count > 0)
        {
            int previouseWeapon = selectedWeapon;
            ///El scroll se usa hacia arriba
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (selectedWeapon >= weaponsList.Count - 1)
                {///El arma seleccionada es mayor al último elemento de la lista
                    selectedWeapon = 0; //Regresa al primer elemento
                }
                else
                    selectedWeapon++; ///Aumenta en 1
            }if(Input.GetAxis("Mouse ScrollWheel") < 0f)
            { ///El scroll se usa hacia abajo
                if (selectedWeapon <= 0 )
                {
                    selectedWeapon = weaponsList.Count - 1;
                }
                else
                    selectedWeapon--;
            }
            if (previouseWeapon != selectedWeapon)
            {
                ActiveWeapon();
            }
        }
    }

    void ActiveWeapon()
    {
        int i = 0;
        foreach (GameObject weapons in weaponsList)
        {
            if (i.Equals(selectedWeapon))
            {
                weapons.SetActive(true);
                weapons.GetComponent<Gun>().enabled = true;
            }
            else
            {
                weapons.SetActive(false);
                weapons.GetComponent<Gun>().enabled = false;
            }
            i++;
        }
    }
}
