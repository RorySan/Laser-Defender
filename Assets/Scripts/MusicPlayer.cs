using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer: MonoBehaviour
{
    public static MusicPlayer music;
      
    void Awake()
    {
        if (music == null)
        {
            DontDestroyOnLoad(gameObject);
            music = this;
        }
        else if (music != this)
        {
            Destroy(gameObject);
        }

    }

}
