using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinNivel : MonoBehaviour
{

    private void Start()
    {
        GameObject.FindGameObjectWithTag("popupNivelCompletado").transform.localScale = new Vector3(0, 0, 0);
        GameObject.FindGameObjectWithTag("bandera").transform.position += new Vector3(0, 0.5f, 0);
    }

    public GameObject popup;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string prueba = PlayerPrefs.GetString("prueba");
        GetComponent<AudioSource>().enabled = true;
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volumenSonido");
        GetComponent<AudioSource>().Play();

        if (prueba == "true")
        {
            SceneManager.LoadScene("NivelVerificado");
        }
        else
        {
            GameObject.FindGameObjectWithTag("popupNivelCompletado").transform.localScale = new Vector3(1, 1, 1);
            Component script = GameObject.FindGameObjectWithTag("jugador").GetComponent("Personaje");
            Destroy(script);
        } 
    }
}
