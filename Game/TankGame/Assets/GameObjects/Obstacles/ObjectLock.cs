using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectLock : MonoBehaviour
{
    public List<GameObject> Locks = new List<GameObject>();
    int count = 1;

    void Start() => StartCoroutine(_checkLock());
    
    IEnumerator _checkLock() {
        while(true) {
            if (Locks.All(e => e == null)) {
                Destroy(this.gameObject);
                if (count == 1)
                {
                    SoundManager.Instance?.PlaySound(SoundAsset.theme2, Vector3.zero);
                }
                if (count == 2)
                {
                    SoundManager.Instance?.PlaySound(SoundAsset.theme3, Vector3.zero);
                }
                if (count == 3)
                {
                    SoundManager.Instance?.PlaySound(SoundAsset.theme4, Vector3.zero);
                }
                count += 1;

            }
            yield return new WaitForSeconds(1f);
        }
    }
}
