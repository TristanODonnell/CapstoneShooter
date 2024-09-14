using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public PooledObject pooledObject;
    public int poolSize;


    public List<PooledObject> availableObjects = new List<PooledObject>();
    [SerializeField] private List<PooledObject> unavailableObjects = new List<PooledObject>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < poolSize; i++)
        {
            PooledObject tempObject = Instantiate(pooledObject, transform);
            tempObject.gameObject.SetActive(false);
            tempObject.LinkPooledObject(this);
            availableObjects.Add(tempObject);
        }
    }

    public void InitializePool(PooledObject objectToPool, int size)
    {
        pooledObject = objectToPool;
        poolSize = size;

        foreach (var obj in availableObjects)
        {
            Destroy(obj.gameObject); // Destroy previously created objects in the pool
        }

        foreach (var obj in unavailableObjects)
        {
            Destroy(obj.gameObject); // Destroy previously unavailable objects
        }
        // Clear existing lists in case of re-initialization
        availableObjects.Clear();
        unavailableObjects.Clear();

        // Create pool objects 
        for (int i = 0; i < poolSize; i++)
        {
            PooledObject tempObject = Instantiate(pooledObject, transform);
            tempObject.gameObject.SetActive(false);
            tempObject.LinkPooledObject(this);
            availableObjects.Add(tempObject);
        }
    }



    public PooledObject RetrievePoolObject()
    {
        if (availableObjects.Count > 0)
        {
            PooledObject tempObject = availableObjects[0];
            availableObjects.RemoveAt(0);
            tempObject.gameObject.SetActive(true);
            unavailableObjects.Add(tempObject);
            Debug.Log($"Retrieved: {tempObject.name}. Available: {availableObjects.Count}, Unavailable: {unavailableObjects.Count}");
            return tempObject;
        }
        else
        {
            Debug.LogWarning("No objects available in the pool!");
            return null; // Prevent new instantiations here
        }
    }

     
    public void SendBackToPool(PooledObject obj)
    {
        if (!availableObjects.Contains(obj))
        {
            obj.gameObject.SetActive(false);
            unavailableObjects.Remove(obj);
            availableObjects.Add(obj);
            Debug.Log($"Returned: {obj.name}. Available: {availableObjects.Count}, Unavailable: {unavailableObjects.Count}");
        }
        else
        {
            Debug.LogWarning($"Object {obj.name} is already in the available pool!");
        }
    }
}

