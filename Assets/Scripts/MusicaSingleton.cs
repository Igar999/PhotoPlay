using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicaSingleton : MonoBehaviour
{
    public AudioClip musicaMenu;
    public AudioClip musicaNivel;
    private static MusicaSingleton instancia = null;
    public static MusicaSingleton getMusica()
    {
            return instancia;
    }

    private bool escenaEsNivel;

    private AudioSource musica;
    // Start is called before the first frame update
    void Start()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            escenaEsNivel = SceneManager.GetActiveScene().name.Equals("Juego");
            instancia = this;
            musica = GetComponent<AudioSource>();
            musica.clip = musicaMenu;
            musica.volume = PlayerPrefs.GetFloat("volumenMusica");
            musica.playOnAwake = true;
            musica.loop = true;
            musica.Play();
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        musica.volume = PlayerPrefs.GetFloat("volumenMusica");
        if (SceneManager.GetActiveScene().name.Equals("Juego") != escenaEsNivel)
        {
            musica.Stop();
            if (SceneManager.GetActiveScene().name.Equals("Juego"))
            {
                musica.clip = musicaNivel;
                escenaEsNivel = true;
            }
            else
            {
                musica.clip = musicaMenu;
                escenaEsNivel = false;
            }
            musica.Play();
        }
    }
}
