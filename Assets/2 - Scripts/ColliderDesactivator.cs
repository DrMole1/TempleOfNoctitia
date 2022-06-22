using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDesactivator : MonoBehaviour
{
    public GameObject objectToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            objectToActivate.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectToActivate.SetActive(false);
        }
    }
}
