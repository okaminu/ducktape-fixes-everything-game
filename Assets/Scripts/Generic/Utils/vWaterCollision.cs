using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vWaterCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
	        other.gameObject.transform.position = new Vector3(4.628f, 1.52f, 13.514f);
    }

}
