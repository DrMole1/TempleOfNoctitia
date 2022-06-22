using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCollider : MonoBehaviour
{
    public Transform btnReset;
    public Vector3[] startPos;
    public Transform[] plates;

    public SoundManager soundManager;

    public int plateInTruePlace = 0;

    private bool isWin = false;

    public Transform autel;
    public GameObject magicMirror;
    public GameObject gameplay1;
    public GameObject gameplay2;
    public Transform[] stairs;

    public GameObject[] objectToDesactivate;

    private void Start()
    {
        for(int i = 0; i < plates.Length; i++)
        {
            startPos[i] = plates[i].position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            btnReset.localPosition = new Vector3(btnReset.localPosition.x, 1.35f, btnReset.localPosition.z);

            for (int i = 0; i < plates.Length; i++)
            {
                plates[i].position = startPos[i];
            }

            soundManager.playAudioClip(0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            btnReset.localPosition = new Vector3(btnReset.localPosition.x, 1.7f, btnReset.localPosition.z);
        }
    }

    public void addPlates()
    {
        plateInTruePlace++;

        if(plateInTruePlace == 4) { win(); }
    }

    public void substractPlates()
    {
        plateInTruePlace--;
    }

    public void win()
    {
        if(!isWin)
        {
            isWin = true;
            soundManager.playAudioClip(2);
            StartCoroutine(IMoveAutel());        
        }
    }

    private IEnumerator IMoveAutel()
    {
        while(autel.localPosition.y < 0)
        {
            autel.localPosition = new Vector3(autel.localPosition.x, autel.localPosition.y + 0.05f, autel.localPosition.z);

            yield return new WaitForSeconds(0.01f);
        }

        while(stairs[0].localScale.x < 10)
        {
            for(int i = 0; i < stairs.Length; i++)
            {
                stairs[i].localScale = new Vector3(stairs[i].localScale.x + 0.1f, stairs[i].localScale.y, stairs[i].localScale.z + 0.1f);
            }

            yield return new WaitForSeconds(0.01f);
        }

        magicMirror.SetActive(false);
        gameplay2.SetActive(true);

        for(int i = 0; i < objectToDesactivate.Length; i++)
        {
            objectToDesactivate[i].SetActive(false);
        }

        gameplay1.SetActive(false);
    }
}
