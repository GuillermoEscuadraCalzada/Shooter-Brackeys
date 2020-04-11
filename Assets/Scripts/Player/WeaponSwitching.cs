using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Corregir y mejorar cambio de arma si la lista del jugador está llena
/// </summary>

public class WeaponSwitching : MonoBehaviour
{
    private static WeaponSwitching instance;

    public static WeaponSwitching Instance {
        get {
            return instance;
        }
    }
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
        }
    }
    [Header("Game Manager")]
    public GameManager gameManager;

    [Header("Script del jugador")]
    
    /// <summary>
    /// Guarda una variable del jugador
    /// </summary>
    Player player;
    
    [Header("UI")]
    /// <summary>
    /// Guarda una imagen de notificaciones
    /// </summary>
    public Image notificationsImage, ammoLayer;
    /// <summary>
    /// Guarda un texto para las notificaciones
    /// </summary>
    public Text notificationsText, ammoText;

    private bool canSwitch = true;
    
    private void Start()
    {
        player = Player.Instance;
        ammoLayer.enabled = !ammoLayer.enabled;
    }
    
    public void SetCanSwitch(bool change)
    {
        canSwitch = change;
    }
    
    private float weaponDistance = 2f;
    // Update is called once per frame
    void Update()
    {
        PickUpGun();
        ScrollToChange();
        DropGun();
        ShowAmmo();
    }

    /// <summary>
    /// Se muestra la munición en pantalla
    /// </summary>
    void ShowAmmo()
    {
        int i = 0; //Contador desde cero
        ///Por cada una de las armas en la lista de armas
        foreach (GameObject weapons in player.weaponsList)
        {
            ///Pregunta si "i" es igual al arma seleccionada
            if (i.Equals(player.selectedWeapon))
            {
                Gun gun = weapons.GetComponent<Gun>();
                ammoText.text = gun.GetCurrentAmmo().ToString() + "/" + player.GetMaxAmmo(gun.gunType).ToString();
            }
            else
            { ///En caso de no serlo
                weapons.SetActive(false); ///Desactiva el arma
                weapons.GetComponent<Gun>().enabled = false; ///Desactiva el script del arma
            }
            i++;
        }
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
                
                float distance = Vector3.Distance(player.playerTransform.position, obj.transform.position);
                ///Pregunta si la distancia es menor a la distancia de un arma
                if (distance < weaponDistance)
                {
                    ///Activa la imagen y texto del UI para que el jugador sepa qué hacer cerca de las imágenes
                    notificationsImage.enabled = true;
                    notificationsText.enabled = true;
                    notificationsText.text = "Press F to pick up Gun"; ///Cambia el texto de las notificaciones
                                                                       ///El usuario presiona la tecla F
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        obj.GetComponent<Collider>().enabled = false; //Desactiva cualquier collider que el objeto tenga
                        obj.GetComponent<Gun>().enabled = true;
                        ///Si el contado de armas es menor a las que se pueden agarrar
                        if (player.weaponsList.Count < player.weaponHolderSize)
                        {
                            if(player.weaponsList.Count > 0)
                            {
                                player.weaponsList[0].SetActive(false); 
                                player.weaponsList[0].GetComponent<Gun>().enabled = false;
                            }
                            obj.transform.parent = transform; ///Haz que el parent de este Objeto sea 
                            obj.transform.position = transform.position;
                            obj.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
                            player.weaponsList.Add(obj); ///Se añade a la lista de armas   
                            gameManager.pickableWeapons.Remove(obj); //Quita el arma de la lista del game manager
                        }
                        else
                        {
                            SwitchGunsFromPlayerToFloor(obj);
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
    void SwitchGunsFromPlayerToFloor(GameObject newGun)
    {
        int i = 0;
        foreach (GameObject weapons in player.weaponsList)
        {
            if (i.Equals(player.selectedWeapon))
            {
                weapons.SetActive(true); ///Se activa el arma en la jerarquía de Unity
                weapons.GetComponent<Gun>().enabled = false; ///Se apaga el script de Gun
                weapons.transform.position = newGun.transform.position; ///Se cambia la posición actual del arma con la del arma que se recogió
                weapons.transform.parent = newGun.transform.parent; ///Se cambia el padre del transform del arma 
                weapons.transform.rotation = Quaternion.identity; ///Se regresa a la identidad del quaternión
                
                newGun.transform.parent = transform; ///El padre del transform del arma es el transform del script
                newGun.transform.position = transform.position; ///Se cambia la posición del arma nueva al transform del script
                newGun.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
                player.weaponsList[player.selectedWeapon] = newGun;      

                gameManager.pickableWeapons.Add(weapons);
                gameManager.pickableWeapons.Remove(newGun);

            }
            else
            {
                i++;
            }
        }
    }

    void DropGun()
    {
        if(player.weaponsList.Count > 0)
        {
            
        }
    }

    /// <summary>
    /// El usuario usa la rueda del mouse para poder cambiar entre armas en caso de que tenga más de una
    /// </summary>
    void ScrollToChange()
    {
        if (canSwitch)
        { 
                ///El tamaño de la lista de armas es mayor a cero
            if (player.weaponsList.Count > 0)
            {

                int previouseWeapon = player.selectedWeapon;
                ///El scroll se usa hacia arriba
                if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                {
                    if (player.selectedWeapon >= player.weaponsList.Count - 1)
                    {///El arma seleccionada es mayor al último elemento de la lista
                        player.selectedWeapon = 0; //Regresa al primer elemento
                    }
                    else
                        player.selectedWeapon++; ///Aumenta en 1
                }if(Input.GetAxis("Mouse ScrollWheel") < 0f)
                { ///El scroll se usa hacia abajo
                    if (player.selectedWeapon <= 0 )
                    {
                        player.selectedWeapon = player.weaponsList.Count - 1;
                    }
                    else
                        player.selectedWeapon--;
                }
                if (previouseWeapon != player.selectedWeapon)
                {
                    ActiveWeaponInWeaponHolder();
                }
            }
        }
    }

    /// <summary>
    /// Se activan y desactivan las armas que se estén usando/guardando
    /// </summary>
    void ActiveWeaponInWeaponHolder()
    {
       
        int i = 0; //Contador desde cero
        ///Por cada una de las armas en la lista de armas
        foreach (GameObject weapons in player.weaponsList)
        {
        ///Pregunta si "i" es igual al arma seleccionada
            if (i.Equals(player.selectedWeapon))
            {
                Gun gun = weapons.GetComponent<Gun>();
                weapons.SetActive(true); ///Activa el arma
                gun.enabled = true; ///Activa el script de arma
                ammoText.text = gun.GetCurrentAmmo().ToString() + "/" + player.GetMaxAmmo(gun.gunType).ToString();
            }
            else
            { ///En caso de no serlo
                weapons.SetActive(false); ///Desactiva el arma
                weapons.GetComponent<Gun>().enabled = false; ///Desactiva el script del arma
            }
            i++;
        }
        
    }
}
