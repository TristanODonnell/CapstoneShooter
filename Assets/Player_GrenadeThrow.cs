using gricel;
using System.Collections;
using System.Collections.Generic;
using throwables;
using UnityEngine;

public class Player_GrenadeThrow : MonoBehaviour
{
    [SerializeField] private Transform g_pivot;
    [SerializeField] private float g_force;
    [System.Serializable]
    private class Pocket
    {
        public ThrowableItem prefab;
        public int ammount;
        public bool AddGrenade()
        {
            if (ammount < 2)
            {
                ammount++;
                return true;
            }
            return false;
        }
    }
    [SerializeField] private Pocket[] g_grenadesInPocket;
    private int g_indexOfGrenade;

    private ThrowableItem g_currentGrenade => g_grenadesInPocket[g_indexOfGrenade].prefab;
    public int g_currentAmmount { 
        get => g_grenadesInPocket[g_indexOfGrenade].ammount;
        set => g_grenadesInPocket[g_indexOfGrenade].ammount = value;
    }
    public Sprite g_currentIcon => g_currentGrenade.icon;

    void GoToIndexOfAvailableGrenade()
    {
        for (int i = 0; i < g_grenadesInPocket.Length; i++)
            if (g_grenadesInPocket[i].ammount > 0)
            {
                g_indexOfGrenade = i;
                return;
            }
    }
    void ThrowGrenade()
    {
        if (g_currentAmmount == 0)
            return;
        g_currentGrenade.Throw(GetComponent<HealthSystem>(), g_pivot.transform.position, g_pivot.forward, g_force);
        g_currentAmmount--;
        if (g_currentAmmount == 0)
            GoToIndexOfAvailableGrenade();
    }

    // Update is called once per frame
    void Update()
    {
        KeyCode[] keycodes ={
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
        };

        for (int i = 0; i < g_grenadesInPocket.Length; i++)
        {
            if (Input.GetKeyDown(keycodes[i]))
                g_indexOfGrenade = i;
        }

        if (Input.GetKeyDown(KeyCode.G))
            ThrowGrenade();

    }

    void GoToIndexOfGrenade(ThrowableItem prefab)
    {
        for (int i = 0; i < g_grenadesInPocket.Length; i++)
            if(prefab == g_grenadesInPocket[i].prefab)
            {
                g_indexOfGrenade = i;
                return;
            }
    }
    public bool TryAddGrenade(ThrowableItem prefab)
    {
        GoToIndexOfGrenade(prefab);
        return g_grenadesInPocket[g_indexOfGrenade].AddGrenade();

	}
}
