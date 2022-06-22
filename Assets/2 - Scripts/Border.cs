using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Border : MonoBehaviour
{
    private const float DELAY = 0.01f;
    private const float ADD_ALPHA = 0.01f;

    [SerializeField] private SpriteRenderer[] borders;

    [SerializeField] private GameObject closePanel;
    [SerializeField] private GameObject openPanel;

    [SerializeField] private Transform player;
    [SerializeField] private Transform startPos;

    [SerializeField] private GameObject canvas;

    [SerializeField] private AudioSource musicManager;
    [SerializeField] private AudioClip music;



    private void Start()
    {
        DateTime now = DateTime.Now;
        int hour = now.Hour;
        
        if(hour >= 21 || hour <= 4)
        {
            openPanel.SetActive(true);
        }
        else
        {
            closePanel.SetActive(true);
        }

        StartCoroutine(IFadeBorders());
    }

    private IEnumerator IFadeBorders()
    {
        Color color = new Color(1f, 0f, 0f, 0f);
        float a = 0f;

        while (a < 1f)
        {
            yield return new WaitForSeconds(DELAY);

            color = new Color(1f, 1f, 1f, a);
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].color = color;
            }
            a += ADD_ALPHA;
        }

        while (a > 0f)
        {
            yield return new WaitForSeconds(DELAY);

            color = new Color(1f, 1f, 1f, a);
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].color = color;
            }
            a -= ADD_ALPHA;
        }

        StartCoroutine(IFadeBorders());
    }

    public void play()
    {
        FPSController controller = player.GetComponent<FPSController>();
        controller.gravityDisabled = true;
        player.position = new Vector3(startPos.position.x, startPos.position.y, startPos.position.z);

        StartCoroutine(IActivatePlayer());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        canvas.SetActive(false);

        musicManager.clip = music;
        musicManager.Play();
    }

    private IEnumerator IActivatePlayer()
    {
        yield return new WaitForSeconds(1f);

        FPSController controller = player.GetComponent<FPSController>();
        controller.camDisabled = false;
        controller.disabled = false;
        controller.lockCursor = true;
        controller.gravity = 18;
        controller.gravityDisabled = false;
    }
}
