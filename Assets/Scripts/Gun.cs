using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impulseForce = 50f;
    public float fireRate = 50f;
    private float nextTimeToFire = 0f;
    public Camera fpsCam;
    public GameObject muzzleFlash;
    public GameObject impactEffect;
    private void Start()
    {
        fpsCam = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        //muzzleFlash.SetActive(false);
        //impactEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            if(Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
            {
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
    /// Se detecta una colisión con algún objeto que tenga algún componente de colisión
    /// </summary>
    void Shoot()
    {
        muzzleFlash.GetComponent<ParticleSystem>().Play(); //Reproduce el sistema de partículas
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){ ///Crea un raycast desde la cámara con el rango especificado en las variables globales
            Debug.Log(hit.transform.name);
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
