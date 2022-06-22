using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCheckerCollider : MonoBehaviour
{
    public ResetCollider resetCollider;
    public int id = 0;

    private void OnTriggerEnter(Collider other)
    {
        string tag = "Symbol" + id.ToString();

        if (other.CompareTag(tag))
        {
            resetCollider.addPlates();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        string tag = "Symbol" + id.ToString();

        if (other.CompareTag(tag))
        {
            resetCollider.substractPlates();
        }
    }
}
