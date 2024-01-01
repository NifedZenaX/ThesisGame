using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public List<Pool> pools;
    public Dictionary<string, List<PooledObject>> poolDictionary;
    public Dictionary<string, GameObject> parentDictionary;
    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            #region Initialization
            Instance.poolDictionary = new Dictionary<string, List<PooledObject>>();
            Instance.parentDictionary = new Dictionary<string, GameObject>();

            foreach (Pool pool in Instance.pools)
            {
                GameObject parent = Instance.CreateNewPool(pool.tag);
                Instance.Proliferate(pool, parent);
            }
            #endregion
        }
        else if (this != Instance)
        {
            #region Data Transfer
            foreach (Pool pool in pools)
            {
                GameObject parent = Instance.CreateNewPool(pool.tag);
                if (parent == null)
                    continue;
                else
                    Instance.pools.Add(pool);
                Instance.Proliferate(pool, parent);
            }
            foreach (GameObject go in Instance.parentDictionary.Values)
            {
                Debug.Log("instance.parentDictionary = " + go.ToString());
            }
            #endregion

            Destroy(gameObject);
        }
        #endregion

        
    }

    private void Proliferate(Pool pool, GameObject parent)
    {
        if (pool.isUIElement)
        {
            Canvas canvas = parent.AddComponent<Canvas>();
            canvas.renderMode = pool.renderMode;
            canvas.worldCamera = pool.camera;

            CanvasScaler canvasScaler = parent.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = pool.uiScaleMode;
            canvasScaler.referenceResolution = (pool.referenceResolution != Vector2.zero) ? pool.referenceResolution : new Vector2(800, 600);

        }

        for (int i = 0; i < pool.size; i++)
        {
            PooledObject obj = Instance.CreateNewObject(pool.prefab.gameObject, parent.transform);
            Instance.poolDictionary[pool.tag].Add(obj);
        }
    }

    /// <summary>
    /// Creating a GameObject that stores GameObjects with the same tag
    /// </summary>
    /// <param name="tag">The tag of the GameObject</param>
    /// <returns>The parent GameObject that can store GameObjects with the same tag name as the GameObject's name if there aren't any parent GameObject with the same name yet.</returns>
    private GameObject CreateNewPool(string tag)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("The tag named: " + tag + " is already defined, are you meaning to use another name?");
            return null;
        }

        GameObject parent = new GameObject(tag);
        parent.transform.SetParent(transform);

        List<PooledObject> list = new List<PooledObject>();

        // TODO: remove this debug log later on
        //Debug.Log("same with Instance = " + (Instance == this));

        poolDictionary.Add(tag, list);
        parentDictionary.Add(tag, parent);
        return parent;
    }

    private PooledObject CreateNewObject(GameObject prefab, Transform parent)
    {
        GameObject newObj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        newObj.SetActive(false);
        newObj.transform.SetParent(parent);
        newObj.transform.localScale = prefab.transform.localScale;
        return newObj.GetComponent<PooledObject>();
    }

    public PooledObject GetGameObjectFromPool(string tag)
    {
        return GetGameObjectFromPool(tag, -1f);
    }

    public PooledObject GetGameObjectFromPool(string tag, float time)
    {
        if (!Instance.poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("No GameObject with tag " + tag);
            return null;
        }

        PooledObject obj = null;

        // find gameobject that's ready to use
        foreach (PooledObject pooledObject in Instance.poolDictionary[tag])
        {
            if (!pooledObject.isActiveAndEnabled)
            {
                obj = pooledObject;
                break;
            }
        }

        // if there are none that's ready to use, make one
        if (obj == null)
        {
            foreach (Pool pool in pools)
            {
                if (pool.tag == tag)
                {
                    obj = Instance.CreateNewObject(pool.prefab.gameObject, Instance.parentDictionary[tag].transform);
                    Instance.poolDictionary[tag].Add(obj.GetComponent<PooledObject>());
                    
                    break;
                }
            }
        }

        obj.SpawnObject(time);

        return obj;
    }

    public GameObject DestroyGameObject(GameObject obj)
    {
        return Instance.DestroyGameObject(obj, false);
    }

    public GameObject DestroyGameObject(GameObject obj, bool saveToObjectPool)
    {
        if (obj.TryGetComponent(out PooledObject pooledObject)) // If it has PooledObject Component
        {
            Instance.DestroyGameObject(pooledObject);
        }
        else if (saveToObjectPool) // If it doesn't have PooledObject Component but wants to be saved in ObjectPool
        {
            // Add the PooledObject component
            // If there aren't any parent GameObject with the same name as the GameObject's tag, make one
            // Save the object into the pool dictionary
            // Destroy the gameobject

            pooledObject = obj.AddComponent<PooledObject>();

            if (!Instance.poolDictionary.ContainsKey(obj.tag))
            {
                CreateNewPool(obj.tag);
            }

            Instance.poolDictionary[obj.tag].Add(pooledObject);
            pooledObject.DestroyObject();
        }
        else // If it doesn't want to be saved in ObjectPool
        {
            Destroy(obj);
        }

        return obj;
    }

    private GameObject DestroyGameObject(PooledObject pooledObject)
    {
        if (!Instance.poolDictionary.ContainsKey(pooledObject.tag))
        {
            Instance.CreateNewPool(pooledObject.tag);
        }

        if (pooledObject.transform.parent != Instance.parentDictionary[pooledObject.tag].transform)
        {
            pooledObject.transform.SetParent(Instance.parentDictionary[pooledObject.tag].transform);
        }

        pooledObject.DestroyObject();
        
        return pooledObject.gameObject;
    }

    public void DestroyAllGameObjectInPool(bool saveToObjectPool)
    {
        foreach (List<PooledObject> list in Instance.poolDictionary.Values)
        {
            foreach (PooledObject obj in list)
            {
                if (obj != null && obj.isActiveAndEnabled)
                {
                    Instance.DestroyGameObject(obj);
                }
            }
        }
    }

    public void DestroyAllGameObjectInPool()
    {
        Instance.DestroyAllGameObjectInPool(false);
    }
}

[System.Serializable]
public class Pool
{
    [Header("Pool")]
    public string tag;
    public PooledObject prefab;
    public int size;
    public bool isUIElement;

    [Header("Canvas")]
    public RenderMode renderMode;
    public Camera camera;

    [Header("Canvas Scaler")]
    public CanvasScaler.ScaleMode uiScaleMode;
    public Vector2 referenceResolution;
}

