using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource musicSource;
    public AudioSource windSource;
    public Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        if (musicSource != null)
        {
            musicSource.loop = true;
            musicSource.Play();
        }

        if (windSource != null)
        {
            windSource.loop = true;
            windSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraTransform != null)
        {
            transform.position = cameraTransform.position;
        }
    }
}
