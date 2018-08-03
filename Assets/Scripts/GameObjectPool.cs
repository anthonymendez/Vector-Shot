using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour {

    [SerializeField] GameObject prefab;

    private List<GameObject> pool = new List<GameObject>();

    public GameObject GetGameObject() {
        print(string.Format("Count: {0}; Capacity {1};", pool.Count, pool.Capacity));
        if (pool.Count > 0) {
            GameObject GObj = pool[0];
            GObj.SetActive(true);
            pool.RemoveAt(0);
            return GObj;
        } else {
            GameObject GObj = Instantiate<GameObject>(prefab);
            return GObj;
        }
    }

    public void AddGameObject(GameObject GObj) {
        pool.Add(GObj);
        GObj.SetActive(false);
        GObj.transform.parent = transform;
    }

    public void ClearPool() {
        while (pool.Count > 0) {
            GameObject GObj = pool[0];
            Destroy(GObj);
            pool.RemoveAt(0);
        }
        pool = null;
    }

}
