using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	void OnCollisionEnter(Collision s)
    {
        var h = s.gameObject.GetComponent<health>();


        if(h != null)
        {
            h.TakeD(10);
        }

        Destroy(gameObject);
    }
}
