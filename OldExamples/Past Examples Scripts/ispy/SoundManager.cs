using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
  [SerializeField] AudioSource mainSource;
  [SerializeField] AudioClip coin;
  [SerializeField] AudioClip pop;

  public void PlaySound(string clipName){
        if(clipName == "coin"){
            mainSource.clip = coin;
        }else if(clipName == "pop"){
            mainSource.clip = pop;
        }
        mainSource.Play();
  }
}
