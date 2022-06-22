using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCollider : MonoBehaviour
{
    public SoundManager soundManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent.position = new Vector3(transform.position.x, 0.67f, transform.position.z);
            soundManager.playAudioClip(1);
        }
    }
}
