using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wChangePond : MonoBehaviour
{
    public GameObject[] oldPond;
    public GameObject[] newPond;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            oldPond = GameObject.FindGameObjectsWithTag("OldPond");
            foreach (GameObject pondObject in oldPond)
            {
                pondObject.SetActive(false);
                pondObject.GetComponent<MeshRenderer>().enabled = false;
            }
            newPond = GameObject.FindGameObjectsWithTag("NewPond");
            foreach (GameObject pondObject in newPond)
            {
                pondObject.SetActive(true);
                pondObject.GetComponent<MeshRenderer>().enabled = true;
            }

        }
    }
}
