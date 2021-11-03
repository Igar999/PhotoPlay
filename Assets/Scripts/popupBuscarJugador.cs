using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popupBuscarJugador : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("popupBuscarUsuario").transform.localScale = new Vector3(0, 0, 0);
        GameObject.FindGameObjectWithTag("barreraBuscarUsuario").transform.localScale = new Vector3(0, 0, 0);
    }


    public void mostrarPopup()
    {
        GameObject.FindGameObjectWithTag("popupBuscarUsuario").transform.localScale = new Vector3(1, 1, 1);
        GameObject.FindGameObjectWithTag("barreraBuscarUsuario").transform.localScale = new Vector3(1, 1, 1);
        GameObject.FindGameObjectWithTag("nombreBuscarUsuario").GetComponent<UnityEngine.UI.InputField>().text = "";
    }

    public void ocultarPopup()
    {
        GameObject.FindGameObjectWithTag("popupBuscarUsuario").transform.localScale = new Vector3(0, 0, 0);
        GameObject.FindGameObjectWithTag("barreraBuscarUsuario").transform.localScale = new Vector3(0, 0, 0);
    }


    public void cargarNivelesJugador()
    {
        string nombreUsuario = GameObject.FindGameObjectWithTag("nombreBuscarUsuario").GetComponent<UnityEngine.UI.InputField>().text;
        if (!nombreUsuario.Equals(""))
        {
            ocultarPopup();
            GameObject.FindGameObjectWithTag("listaNivelesCanvas").GetComponent<CargarNivelesDeAlmacenamiento>().cargarNivelesDeUsuario(nombreUsuario);
        }
    }


}
