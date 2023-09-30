using UnityEngine;
using System;
using System.Collections;

public enum Sound
{
    SnakeTurn,  
    SnakeDie,   
    EatFruit,   
    EatCoin,    
    GhostSpawn, 
    WolfSpawn,  
    BlockSpawn,
    FruitSpawn,
    AxeSpawn,
    GiftUI,     
    PointUI,
    Repeat,
    TitleUI,
    EnterName
    
}


public class SoundManager : MonoBehaviour
{
    [Serializable]
    public class SoundAudioClip
    {
        public Sound sound;
        public AudioClip audioClip;
    }

    public  SoundAudioClip[] soundAudioClipArray;

    public  void PlaySound(Sound sound)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
        if (sound == Sound.EnterName) StartCoroutine(DestroyAfterDelay(soundGameObject, 3.0f));
        else StartCoroutine(DestroyAfterDelay(soundGameObject));
    }

    private  AudioClip GetAudioClip(Sound sound)
    {
        foreach (SoundAudioClip soundAudioClip in soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " is missing!");
        return null;
    }

    private IEnumerator DestroyAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(obj);
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }

}
