using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int damage;
    private void OnCollisionEnter(Collision collision)
    {
        //HealthSystem healthSystem = collision.gameObject.GetComponent<HealthSystem>();
     //   if (healthSystem != null)
        {
           // healthSystem.Damage(damage);

        }
        Destroy(gameObject, 2f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    

}
