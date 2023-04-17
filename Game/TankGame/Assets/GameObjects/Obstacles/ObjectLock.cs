using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectLock : MonoBehaviour
{
    public List<GameObject> Locks = new List<GameObject>();

    void Start() => StartCoroutine(_checkLock());
    
    IEnumerator _checkLock() {
        while(true) {
            if (Locks.All(e => e == null)) {
                Destroy(this.gameObject);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
