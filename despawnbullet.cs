using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class despawnbullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "bulletdestroy")
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
