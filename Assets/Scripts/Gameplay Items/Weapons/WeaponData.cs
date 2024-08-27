using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Weapon Data")]
[System.Serializable]
public class WeaponData : ScriptableObject, IInteractable

{

    
    public enum AmmoType { Shrapnel, Energy, Melee }
    public enum WeaponType { Projectile, Hitscan, Melee }

    private bool isReloading = false;


    public WeaponType weaponType;
    public AmmoType ammoType;


    [SerializeField] protected string weaponName;
    [SerializeField] protected GameObject weaponModel;
    [SerializeField] protected int damage;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float range;
    [SerializeField] protected float fireRate;
    [SerializeField] protected float reloadTime;
    [SerializeField] private GameObject projectilePrefab;

    public string WeaponName => weaponName;
    public GameObject GetWeaponModel() => weaponModel;

    public int Damage => damage;

    public float BulletSpeed => bulletSpeed;

    public float Range => range;

    public float FireRate => fireRate;

    public float ReloadTime => reloadTime;

    





    //VISUALS, change out later for specific variable we need 
    [SerializeField] protected GameObject reticle;
    [SerializeField] protected GameObject weaponVisual;

    
    public virtual void Shoot(Vector3 position, Quaternion rotation)
    {



        if (weaponType == WeaponType.Projectile) //PROJECTILE WEAPON
        {
            Vector3 bulletSpawnPosition = position;
            Quaternion bulletSpawnRotation = rotation;
            GameObject bullet = Instantiate(projectilePrefab, bulletSpawnPosition, bulletSpawnRotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = rotation * Vector3.forward * bulletSpeed;
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.damage = GetDamage();
        }
        else if (weaponType == WeaponType.Hitscan)  //HITSCAN WEAPON 
        {
            Debug.Log("Shooting hitscan...");
            RaycastHit hit;

            if (Physics.Raycast(position, rotation * Vector3.forward, out hit, range))
            {
                Debug.Log("Hitscan hit something...");
               // HealthSystem healthSystem = hit.transform.GetComponent<HealthSystem>();
             //   if (healthSystem != null)
              //  {
                 //   healthSystem.Damage(damage);
             //   }
            }
            else
            {
                Debug.Log("Hitscan did not hit anything.");
            }
        }
        else if (weaponType == WeaponType.Melee)  //MELEE WEAPON
        {
            Debug.Log("Shooting melee...");
            Collider[] hits = Physics.OverlapSphere(position, range);
            foreach (Collider hit in hits)
            {
                {
                    Debug.Log("Melee hit something...");
                  //  HealthSystem healthSystem = hit.transform.GetComponent<HealthSystem>();
                   // if (healthSystem != null)
                   // {
                   //     healthSystem.Damage(damage);
                  //  }
                }
            }
        }

    }

    public IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        //ANY VISUAL  ELEMENT FOR WEAPON WOULD BE HERE 
        isReloading = false;
    }


    protected virtual int GetDamage()
    {
        // Calculate and return damage, for example:
        return damage;
    }

    public void OnHoverEnter()
    {
        Debug.Log("Weapon pickup available");
    }

    public void OnHoverExit()
    {

    }
    
    public void Interact(PlayerController player, WeaponHolder weaponHolder)
    {

        if (player.shoot.currentWeapon != null)
        {
            // Drop the current weapon
            player.shoot.DropWeapon(player.shoot.currentWeaponIndex);
  
        }

        player.shoot.PickUpWeapon(weaponHolder.myweaponData);

        //PICK UP WEAPON 
        //ADD A FUMCTIOM IN THE SHOOTBEHAVIOR 
    }

    public void Interact(PlayerController player, EquipmentHolder equipmentHolder)
    {
        throw new System.NotImplementedException();
    }

    public void Interact(PlayerController player, EquipmentData equipmentData)
    {
        throw new System.NotImplementedException();
    }
}
 