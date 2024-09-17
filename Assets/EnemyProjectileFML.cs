using gricel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class EnemyProjectileFML : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    float eP_duration;
    float eP_velocity;
    ProtectionValues eP_Damage;
    static Queue<EnemyProjectileFML> eP_list = new();
    static EnemyProjectileFML prefab;
    public static EnemyProjectileFML EP_Call(WeaponData data, Vector3 pos, Vector3 forward)
    {
        if (prefab == null)
            prefab = Resources.Load<EnemyProjectileFML>("Enemy Projectile");
        EnemyProjectileFML projectile = null;
        if(eP_list.Count > 0)
			projectile = eP_list.Dequeue();
		if (projectile == null)
            projectile = Instantiate(prefab);

        projectile.gameObject.SetActive(true);
		projectile.transform.position = pos;
        projectile.transform.forward = forward;
        projectile.eP_duration = data.range;
        projectile.eP_velocity = data.bulletSpeed;
        if (projectile.eP_velocity == 0)
            projectile.eP_velocity = 1f;

        projectile.eP_Damage = data.ProtectionValues;
        projectile.transform.localScale = Vector3.one * data.bulletScale;

        return projectile;
    }

    void Despawn()
    {
		gameObject.SetActive(false);
        eP_list.Enqueue(this);
	}

	void Update()
    {
        rb.velocity = transform.forward * eP_velocity;
        eP_duration -= eP_velocity * Time.deltaTime;
        if (eP_duration < 0)
            Despawn();

    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.GetComponentInParent<gricel.Enemy>())
            return;

        if (other.GetComponentInParent<gricel.HealthSystem>() != null)
            if (other.TryGetComponent<PlayerController>(out var player))
            {
                player.healthSystem.HS_Damage(eP_Damage);
                Despawn();
            }
        else
			Despawn();
	}
}
