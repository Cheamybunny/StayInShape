using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioLogic : MonoBehaviour
{

    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public DecoData ToDecoData()
    {
        return new DecoData(transform.localPosition, 5);
    }

    private void OnDestroy()
    {
        DecoManager.instance.InsertDecoration(this.ToDecoData());
    }
}
