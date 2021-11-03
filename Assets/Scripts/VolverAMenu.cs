using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolverAMenu : MonoBehaviour
{


    public GameObject textoBotonGuardarComp;


    private void Start()
    {
        textoBotonGuardarComp.GetComponent<UnityEngine.UI.Text>().text = "Guardar nivel";

        if(Datos.getNivelSeleccionado() != null)
        {
            if (Datos.getNivelSeleccionado().esGuardado().Equals("1"))
            {
                textoBotonGuardarComp.GetComponent<UnityEngine.UI.Text>().text = "Quitar de guardados";
            }
        }      
    }

    public void guardarNivel()
    {
        if (Datos.getNivelSeleccionado().esGuardado().Equals("1"))
        {
            Datos.getNivelSeleccionado().quitarGuardado();
            PlayerPrefs.SetInt("guardado", 0);
            textoBotonGuardarComp.GetComponent<UnityEngine.UI.Text>().text = "Guardar nivel";
        }
        else
        {
            Datos.getNivelSeleccionado().ponerGuardado();
            PlayerPrefs.SetInt("guardado", 1);
            textoBotonGuardarComp.GetComponent<UnityEngine.UI.Text>().text = "Quitar de guardados";
        }
        PlayerPrefs.SetInt("actualizar", 1);
        SistemaGuardado.guardar();
    }


    public void volverAMenu()
    {
        SceneManager.LoadScene("Menú");

        PlayerPrefs.DeleteKey("nivelNombre");
        PlayerPrefs.DeleteKey("nivelCreador");
        PlayerPrefs.DeleteKey("nivelLikes");
        PlayerPrefs.DeleteKey("nivelId");
        PlayerPrefs.DeleteKey("nivelStringGen");
        PlayerPrefs.DeleteKey("prueba");
        PlayerPrefs.DeleteKey("guardado");
        Datos.borrarNivelSeleccionado();
    }
}
