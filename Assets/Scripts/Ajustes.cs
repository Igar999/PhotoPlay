using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using UnityEngine.Networking;

public class Ajustes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("deslizadorMusica").GetComponent<UnityEngine.UI.Slider>().value = PlayerPrefs.GetFloat("volumenMusica");
        GameObject.FindGameObjectWithTag("deslizadorSonido").GetComponent<UnityEngine.UI.Slider>().value = PlayerPrefs.GetFloat("volumenSonido");

        if (PlayerPrefs.GetString("usuarioActual").Equals(""))
        {
            GameObject.FindGameObjectWithTag("botonInicioSesion").transform.localScale = new Vector3(1, 1, 1);
            GameObject.FindGameObjectWithTag("botonRegistro").transform.localScale = new Vector3(1, 1, 1);
            GameObject.FindGameObjectWithTag("botonCerrarSesion").transform.localScale = new Vector3(0, 0, 0);
            GameObject.FindGameObjectWithTag("cuadroNombreUsuario").transform.localScale = new Vector3(0, 0, 0);
            GameObject.FindGameObjectWithTag("textoNombreUsuario").GetComponent<UnityEngine.UI.Text>().text = "";

        }
        else
        {
            GameObject.FindGameObjectWithTag("botonInicioSesion").transform.localScale = new Vector3(0, 0, 0);
            GameObject.FindGameObjectWithTag("botonRegistro").transform.localScale = new Vector3(0, 0, 0);
            GameObject.FindGameObjectWithTag("botonCerrarSesion").transform.localScale = new Vector3(1, 1, 1);
            GameObject.FindGameObjectWithTag("cuadroNombreUsuario").transform.localScale = new Vector3(1, 1, 1);
            GameObject.FindGameObjectWithTag("textoNombreUsuario").GetComponent<UnityEngine.UI.Text>().text = PlayerPrefs.GetString("usuarioActual");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            volverAMenu();
        }
    }

    public void volverAMenu()
    {
        SistemaGuardado.guardar();
        SceneManager.LoadScene("Menú");
    }

    public void actualizarVolumenMusica()
    {
        PlayerPrefs.SetFloat("volumenMusica", GameObject.FindGameObjectWithTag("deslizadorMusica").GetComponent<UnityEngine.UI.Slider>().value);
        SistemaGuardado.guardar();
    }

    public void actualizarVolumenSonido()
    {
        PlayerPrefs.SetFloat("volumenSonido", GameObject.FindGameObjectWithTag("deslizadorSonido").GetComponent<UnityEngine.UI.Slider>().value);
        SistemaGuardado.guardar();
    }

    public void cerrarSesion()
    {
        PlayerPrefs.SetString("usuarioActual", "");
        GameObject.FindGameObjectWithTag("botonInicioSesion").transform.localScale = new Vector3(1, 1, 1);
        GameObject.FindGameObjectWithTag("botonRegistro").transform.localScale = new Vector3(1, 1, 1);
        GameObject.FindGameObjectWithTag("botonCerrarSesion").transform.localScale = new Vector3(0, 0, 0);
        GameObject.FindGameObjectWithTag("cuadroNombreUsuario").transform.localScale = new Vector3(0, 0, 0);
        GameObject.FindGameObjectWithTag("textoNombreUsuario").GetComponent<UnityEngine.UI.Text>().text = "";

        SistemaGuardado.guardar();
    }

}
