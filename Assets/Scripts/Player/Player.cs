using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private static Player instance; 
    public List<GameObject> weaponsList;
    public Transform playerTransform;
    [Header("Imagen de puntería")]
    ///Imagen de puntería que referencia al jugador donde apunta
    public Image aimImage;
    [Header("Tamaño máximo del contenedor")]
    /// <summary>
    /// Tamaño que el contenedor debe de tener
    /// </summary>
    public int weaponHolderSize = 2;
    [Header("Arma actual")]
    public int selectedWeapon = 0;

    /// <summary>
    /// Canitdad de balas para rifle de asalto
    /// </summary>
    private int maxAssaultRifleAmmo;
    private int currentAssaultAmmo;

    /// <summary>
    /// Canitdad de balas para pistola
    /// </summary>
    private int maxPistleAmmo;
    private int currentPistleAmmo;

    /// <summary>
    /// Canitdad de balas para escopeta
    /// </summary>
    private int maxShotgunAmmo;
    private int currentShotgunAmmo;

    private void Awake()
    {
        if (!instance)
        { ///Si la instancia es diferente de nula, crea una instancia con los valores del gameObject
            instance = this;
            DontDestroyOnLoad(instance); ///No destruyas este objeto
        }
        else
        {
            Destroy(this); ///Si ya existe la instancia, crea el objeto
        }
    }

    public static Player Instance {
        get {
            return instance; ///Regresa la instancia del jugador
        }
    }

    private void Start()
    {
        ///Munición máxima en el rifle de asalto
        currentAssaultAmmo = maxAssaultRifleAmmo = 210;
        ///Munición máxima de la pistola
        currentPistleAmmo = maxPistleAmmo = 60;
        ///Munición máxima de la escopeta
        currentShotgunAmmo = maxShotgunAmmo = 64;
        ///Empieza desactivada la imagen del crosshair
        aimImage.enabled = false;
        ///Crea una lista de armas nueva
        weaponsList = new List<GameObject>();
        playerTransform = transform;
    }
   
    private void Update()
    {
        if(weaponsList.Count > 0 && !aimImage.enabled)
        {
            aimImage.enabled = true;
        }
       
    }

    /// <summary>
    /// Regresa la cantidad actual máxima de munición que el jugador tiene del tipo de arma
    /// </summary>
    /// <param name="gun"></param>
    /// <returns></returns>
    public int GetMaxAmmo(GunType gun)
    {
        switch (gun)
        {
            case GunType.ASSAULT:
                {
                    return currentAssaultAmmo; ///Regresa la munición de rifle
                }

            case GunType.PISTLE:
                {
                    return currentPistleAmmo; ///Regrea la munición de pistola
                }

            case GunType.SHOTGUN:
                {
                    return currentShotgunAmmo; ///Regresa la munición de escopeta
                }
            default:
                return 0;
        }
    }

    /// <summary>
    /// Actualiza la munición máxima dependiendo del tipo de arma que se haya usado
    /// </summary>
    /// <param name="gun"></param>
    /// <param name="ammoToSubstract"></param>
    public void SetMaxAmmo(GunType gun, int ammoToSubstract)
    {
        switch (gun)
        {
            case GunType.ASSAULT:
                {
                    currentAssaultAmmo -= ammoToSubstract; ///Se actualiza la munción del rifle de asalto
                    return;
                }

            case GunType.PISTLE:
                {
                    currentPistleAmmo-= ammoToSubstract; ///Se actualiza la munción de la pistola
                    return;
                }

            case GunType.SHOTGUN:
                {
                    currentShotgunAmmo-= ammoToSubstract; ///Se actualiza la munción de la escopeta
                    return;
                }
            default:
                return;
        }
    }

}
