using UnityEngine;
using System.Collections;

public enum GunType
{
    ASSAULT,
    PISTLE,
    SHOTGUN
}

public class Gun : MonoBehaviour
{
    [Header("Características del Arma")]
    ///<summary>
    ///Daño del arma
    ///</summary>
    public float damage = 1f;
    
    /// <summary>
    /// Rango de disparo
    /// </summary>
    public float range = 100f;
    
    /// <summary>
    /// Fuerza al chocar con un objeto
    /// </summary>
    public float impulseForce = 50f;
   
    /// <summary>
    /// Cadencia de tiro
    /// </summary>
    public float fireRate;
    
    /// <summary>
    /// Velocidad de disparo
    /// </summary>
    public float reloadSpeed = 1f;
    

    /// <summary>
    /// Munición máxima en el cartucho
    /// </summary>
    public int maxAmmoInCartridge;
    
    [SerializeField]
    /// <summary>
    /// Tipo de arma
    /// </summary>
    public GunType gunType;

    /// <summary>
    /// Munición actual en el cartucho
    /// </summary>
    private int currentAmmoInCartridge;

    /// <summary>
    /// Se encuentra recargando
    /// </summary>
    private bool isReloading = false;

    /// <summary>
    /// Siguiente tiempo para disparar
    /// </summary>
    private float nextTimeToFire = 0f;
    [Header("Cámara del jugador")]
    /// <summary>
    /// Cámara del jugador
    /// </summary>
    public Camera fpsCam;
    [Header("Flash del arma")]
    /// <summary>
    /// Flash del disparo del arma
    /// </summary>
    public GameObject muzzleFlash;
    [Header("Flash de impacto")]
    /// <summary>
    /// Efecto de impacto
    /// </summary>
    public GameObject impactEffect;

    public Animator animator;
    public WeaponSwitching weaponSwitching;

    Player player;
    private void Start()
    {
        player = Player.Instance; ///La variable de Player es igual a la instancia del juego
        fpsCam = GameObject.Find("PlayerCamera").GetComponent<Camera>(); ///La cámara del jugador se guarda en esta variable
        switch (gunType)
        {
            case GunType.ASSAULT:
                {
                    maxAmmoInCartridge = 30; ///Munición máxima en el cartucho por defecto es de 30
                    fireRate = 10; ///FireRate del arma
                    damage = 1; ///Daño del arma es 1
                    break;
                }

            case GunType.PISTLE:
                {
                   maxAmmoInCartridge = 15; ///Munición máxima del cartucho en la pistola
                   fireRate = 2; ///FireRate del arma
                   damage = 1; ///Daño es igual a 1
                    break;
                }

            case GunType.SHOTGUN:
                {
                   maxAmmoInCartridge = 8; ///Munición máxima del cartucho para la escopeta
                    fireRate = 1; ///Firerate de la escopeta
                    damage = 10; ///Daño del arma es de 10
                    break;
                }
        }
        currentAmmoInCartridge = maxAmmoInCartridge;
    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    // Update is called once per frame
    void Update()
    {

        ///Si está activado
        if (enabled)
        {
            ///Si se encuentra recargando se termina el update
            if (isReloading) return;
            ///Pregunta si la munición máxima es mayor a cero y si la munición es menor o igual a cero, además de que puede el usuario presionar
            ///R y que la munición sea diferente a la munición máxima en el cartcuho
            if(player.GetMaxAmmo(gunType) > 0 &&( currentAmmoInCartridge <= 0 || (Input.GetKeyDown(KeyCode.R) && currentAmmoInCartridge != maxAmmoInCartridge)))
            {
                ///Comienza la corutina de recarga
                StartCoroutine(Reload());
                return; ///Termina el update
            }
            //Si se presioan el botón izquierdo y las balas son mayores a cero, dispara
            if(currentAmmoInCartridge > 0 && Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
            {
                if (currentAmmoInCartridge.Equals(-1)) currentAmmoInCartridge = maxAmmoInCartridge;
                muzzleFlash.SetActive(true);
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
            else
            {
                muzzleFlash.SetActive(false);
                //impactEffect.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Función que nos permitirá recargar el arma después de terminarnos la munición
    /// </summary>
    IEnumerator Reload()
    {
        ///El booleano Reloading se hace true
        #region Antes de recargar
        isReloading = true;
        ///Esta animación se hace true e inicia el ciclo de la animación
        animator.SetBool("Reloading", true);
        weaponSwitching.SetCanSwitch(false);
        float defaultTransitionTime = 0.25f; ///Tiempo default para transicionar entre animaciones
        #endregion

        #region Recargando
        yield return new WaitForSeconds(reloadSpeed - defaultTransitionTime);
        PlayerAmmoUpdate();
        animator.SetBool("Reloading", false); ///Se termina la animación

        yield return new WaitForSeconds(defaultTransitionTime);
        #endregion


        weaponSwitching.SetCanSwitch(true);
        ///El booleano Reloading se hace false
        isReloading = false;

    }

    void PlayerAmmoUpdate()
    {
        int playerMaxAmmo = player.GetMaxAmmo(gunType);
        if (playerMaxAmmo > 0)
        {
            int ammo = maxAmmoInCartridge - currentAmmoInCartridge;
            if (playerMaxAmmo - ammo > 0) currentAmmoInCartridge += ammo;
            else
                currentAmmoInCartridge += playerMaxAmmo;

            if (playerMaxAmmo - ammo > 0) player.SetMaxAmmo(gunType, ammo);
            else player.SetMaxAmmo(gunType, playerMaxAmmo);
        }
    }

    /// <summary>
    /// Regresa la munición actual en el cartucho
    /// </summary>
    /// <returns></returns>
    public int GetCurrentAmmo()
    {
        return currentAmmoInCartridge;
    }

    /// <summary>
    /// Se detecta una colisión con algún objeto que tenga algún componente de colisión
    /// </summary>
    void Shoot()
    {
        ///Se reduce en 1 la munición actual
        currentAmmoInCartridge--;
        muzzleFlash.GetComponent<ParticleSystem>().Play(); //Reproduce el sistema de partículas
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){ ///Crea un raycast desde la cámara con el rango especificado en las variables globales

            if (hit.collider.GetComponent<Target>())
            { ///Si el objeto tiene el scritp de target, ponle daño
                hit.collider.GetComponent<Target>().TakeDamage(10f);
            }
            if (hit.rigidbody)
            { //Si el objeto tiene rigidbody, añadir una fuerza que empuje al objeto
                hit.rigidbody.AddForce(-hit.normal * impulseForce);
            }
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
        }
       
    }

}
