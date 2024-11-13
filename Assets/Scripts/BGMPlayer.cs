using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    //BGMPlayer instance;
    AudioSource audioSource;

    private void Awake()
    {
        //if (!instance)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(this);
        //    audioSource = this.GetComponent<AudioSource>();
        //}
        //else GameObject.Destroy(this);
    }

    private void Start()
    {
        audioSource.Play();
        audioSource.loop = true;
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    private void OnDestroy()
    {
        Debug.Log("On Destroy Pressed");
        StopMusic();
    }

    void OnDisable()
    {
        Debug.Log("On Disable pressed!");
        StopMusic();
    }

}
