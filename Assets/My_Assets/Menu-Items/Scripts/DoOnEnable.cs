using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoOnEnable : MonoBehaviour
{
    public enum Do
    {
        None,Disable,Destroy,PlaySoundFromMusicManager, playSelfSound
    }
    public float after = 0;
    public Do @do;
    [Header("If You Watnt To Play Sound Please " +
        "Enter Sound Name" +
        "there should MusicManager In The Scene")]
    public string soundName;
    public AudioClip selfSound;   
    private void OnEnable()
    {       
        StartCoroutine(DoWork());
    }
    IEnumerator DoWork()
    {
        yield return new WaitForSeconds(after);
        if (@do == Do.Destroy)
        {
            Destroy(gameObject);
        }else
            if (@do == Do.Disable)
        {
            gameObject.SetActive(false);
        }
        else
            if (@do == Do.PlaySoundFromMusicManager)
        {
            if (soundName!=string.Empty)
            {
                MusicManager.PlaySfx(soundName);
            }
           
        }
        if (selfSound)
        {
            AudioSource.PlayClipAtPoint(selfSound, transform.position);
        }

    }
    
}
