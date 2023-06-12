using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    public Slider volumeSlider;
    public AudioSource backgroundMusic;

    void Start()
    {
     backgroundMusic.Play();   
    }

    void Update(){
        ChangeVolume();
    }

    public void ChangeVolume(){
        backgroundMusic.volume = volumeSlider.value;
    }
}
