using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PopupNivelCompletado : MonoBehaviour
{

    public UnityEngine.UI.Text textoBotonGuardarComp;
    public UnityEngine.UI.Text textoBotonMeGustaComp;
    public UnityEngine.UI.Button botonGuardarComp;
    public UnityEngine.UI.Button botonMeGustaComp;


    void Start()
    {

        textoBotonGuardarComp.GetComponent<UnityEngine.UI.Text>().text = "Marcar como favorito";
        if (Datos.getNivelSeleccionado() != null)
        {
            if (Datos.getNivelSeleccionado().esGuardado().Equals("1"))
            {
                textoBotonGuardarComp.GetComponent<UnityEngine.UI.Text>().text = "Quitar de favoritos";
            }

            textoBotonMeGustaComp.GetComponent<UnityEngine.UI.Text>().text = "Me gusta";

            if (Datos.getNivelSeleccionado().tieneMeGusta().Equals("1"))
            {
                textoBotonMeGustaComp.GetComponent<UnityEngine.UI.Text>().text = "Quitar me gusta";
            }
        }
        
        if (PlayerPrefs.GetString("usuarioActual").Equals(""))
        {
            textoBotonMeGustaComp.GetComponent<UnityEngine.UI.Text>().text = "Inicia sesion";
            textoBotonGuardarComp.GetComponent<UnityEngine.UI.Text>().text = "Inicia sesion";
            botonMeGustaComp.interactable = false;
            botonGuardarComp.interactable = false;
        }

    }


    public void guardarNivel()
    {
        if (Datos.getNivelSeleccionado() != null)
        {
            if (Datos.getNivelSeleccionado().esGuardado().Equals("1"))
            {
                StartCoroutine(coGuardarNivel("0"));
            }
            else
            {
                StartCoroutine(coGuardarNivel("1"));
            }
        }
    }

    public void gustarNivel()
    {
        if (Datos.getNivelSeleccionado() != null)
        {
            if (Datos.getNivelSeleccionado().tieneMeGusta().Equals("1"))
            {
                StartCoroutine(coGustarNivel("0"));
            }
            else
            {
                StartCoroutine(coGustarNivel("1"));
            }
        }
    }

    class aceptarcertificado : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    private IEnumerator coGuardarNivel(string estadoGuardado)
    {
        GameObject.FindGameObjectWithTag("cargando").transform.localScale = new Vector3(1, 1, 1);
        WWWForm datos = new WWWForm();
        datos.AddField("estado", estadoGuardado);
        datos.AddField("usuario", PlayerPrefs.GetString("usuarioActual"));
        datos.AddField("nivel", PlayerPrefs.GetString("nivelId"));

        UnityWebRequest peticion = UnityWebRequest.Post("https://igar999.me/guardarNivel", datos);
        peticion.SetRequestHeader("content-Type", "application/x-www-form-urlencoded");
        peticion.SetRequestHeader("Accept", "application/x-www-form-urlencoded");
        peticion.SetRequestHeader("api-version", "1.1");

        peticion.certificateHandler = new aceptarcertificado();
        yield return peticion.Send();
        GameObject.FindGameObjectWithTag("cargando").transform.localScale = new Vector3(0, 0, 0);
        if (peticion.isNetworkError || peticion.isHttpError)
        {
            GameObject.FindGameObjectWithTag("popupErrorConexion").GetComponent<popupErrorConexion>().mostrarPopup();
        }
        else
        {
            if (Datos.getNivelSeleccionado().esGuardado().Equals("1"))
            {
                Datos.getNivelSeleccionado().quitarGuardado();
                textoBotonGuardarComp.GetComponent<UnityEngine.UI.Text>().text = "Marcar como favorito";
                if (PlayerPrefs.GetString("lista").Equals("nivelesGuardados"))
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                Datos.getNivelSeleccionado().ponerGuardado();
                textoBotonGuardarComp.GetComponent<UnityEngine.UI.Text>().text = "Quitar de favoritos";
            }
            PlayerPrefs.SetInt("actualizar", 1);
            SistemaGuardado.guardar();
        }
    }

    private IEnumerator coGustarNivel(string estadoGustado)
    {
        GameObject.FindGameObjectWithTag("cargando").transform.localScale = new Vector3(1, 1, 1);
        WWWForm datos = new WWWForm();
        datos.AddField("estado", estadoGustado);
        datos.AddField("usuario", PlayerPrefs.GetString("usuarioActual"));
        datos.AddField("nivel", PlayerPrefs.GetString("nivelId"));

        UnityWebRequest peticion = UnityWebRequest.Post("https://igar999.me/gustarNivel", datos);
        peticion.SetRequestHeader("content-Type", "application/x-www-form-urlencoded");
        peticion.SetRequestHeader("Accept", "application/x-www-form-urlencoded");
        peticion.SetRequestHeader("api-version", "1.1");

        peticion.certificateHandler = new aceptarcertificado();
        yield return peticion.Send();
        GameObject.FindGameObjectWithTag("cargando").transform.localScale = new Vector3(0, 0, 0);
        if (peticion.isNetworkError || peticion.isHttpError)
        {
            GameObject.FindGameObjectWithTag("popupErrorConexion").GetComponent<popupErrorConexion>().mostrarPopup();
        }
        else
        {
            if (Datos.getNivelSeleccionado().tieneMeGusta().Equals("1"))
            {
                Datos.getNivelSeleccionado().quitarMeGusta();
                textoBotonMeGustaComp.GetComponent<UnityEngine.UI.Text>().text = "Me gusta";
                Datos.getNivelSeleccionado().reducirLikes();
            }
            else
            {
                Datos.getNivelSeleccionado().ponerMeGusta();
                textoBotonMeGustaComp.GetComponent<UnityEngine.UI.Text>().text = "Quitar Me gusta";
                Datos.getNivelSeleccionado().aumentarLikes();
            }
            SistemaGuardado.guardar();
        }
    }
}
