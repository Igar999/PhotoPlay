using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PopupInfo : MonoBehaviour
{

    public GameObject fotoNivelComp;
    public GameObject nombreNivelComp;
    public GameObject creadorNivelComp;
    public GameObject likesNivelComp;
    public GameObject textoBotonGuardarComp;
    public GameObject textoBotonMeGustaComp;
    public UnityEngine.UI.Button botonGuardarComp;
    public UnityEngine.UI.Button botonMeGustaComp;


    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("popupInfoNivelContenedor").transform.localScale = new Vector3(0, 0, 0);
        GameObject.FindGameObjectWithTag("cuadroBorrar").transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(obtenerNivel());
        if (PlayerPrefs.GetString("lista").Equals("misNiveles"))
        {
            GameObject.FindGameObjectWithTag("botonBorrar").transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            GameObject.FindGameObjectWithTag("botonBorrar").transform.localScale = new Vector3(0, 0, 0);
        }
    }


    private IEnumerator obtenerNivel()
    {
        GameObject.FindGameObjectWithTag("cargando").transform.localScale = new Vector3(1, 1, 1);
        WWWForm datos = new WWWForm();
        datos.AddField("id", PlayerPrefs.GetString("nivelId"));
        datos.AddField("usuarioActual", PlayerPrefs.GetString("usuarioActual"));
        UnityWebRequest peticion = UnityWebRequest.Post("https://igar999.me/getDatosNivel", datos);

        peticion.SetRequestHeader("content-Type", "application/x-www-form-urlencoded");
        peticion.SetRequestHeader("Accept", "application/x-www-form-urlencoded");
        peticion.SetRequestHeader("api-version", "1.1");

        peticion.certificateHandler = new aceptarcertificado();
        yield return peticion.Send();
        GameObject.FindGameObjectWithTag("cargando").transform.localScale = new Vector3(0, 0, 0);
        if (peticion.isNetworkError || peticion.isHttpError)
        {
            GameObject.FindGameObjectWithTag("popupErrorConexion").GetComponent<popupErrorConexion>().mostrarPopup();
            Destroy(this.gameObject);
        }
        else
        {
            if (peticion.downloadHandler.text.Length > 0)
            {
                string cadena = peticion.downloadHandler.text;

                string[] datosSeparados = cadena.Split(',');

                string id = datosSeparados[0];
                string strGen = datosSeparados[1];
                string nombre = datosSeparados[2];
                string creador = datosSeparados[3];
                string numLikes = datosSeparados[4];
                string likeYo = datosSeparados[5];
                string favoritoYo = datosSeparados[6];

                nombreNivelComp.GetComponent<UnityEngine.UI.Text>().text = nombre;
                creadorNivelComp.GetComponent<UnityEngine.UI.Text>().text = creador;
                likesNivelComp.GetComponent<UnityEngine.UI.Text>().text = "Me gusta: " + numLikes;

                fotoNivelComp.GetComponent<UnityEngine.UI.RawImage>().texture = GameObject.FindGameObjectWithTag("barraInferior").GetComponent<AccederNivel>().generarImagen(strGen);

                textoBotonGuardarComp.GetComponent<UnityEngine.UI.Text>().text = "Marcar como favorito";
                if (favoritoYo.Equals("1"))
                {
                    textoBotonGuardarComp.GetComponent<UnityEngine.UI.Text>().text = "Quitar de favoritos";
                }

                textoBotonMeGustaComp.GetComponent<UnityEngine.UI.Text>().text = "Me gusta";

                if (likeYo.Equals("1"))
                {
                    textoBotonMeGustaComp.GetComponent<UnityEngine.UI.Text>().text = "Quitar me gusta";
                }


                if (PlayerPrefs.GetString("usuarioActual").Equals(""))
                {
                    textoBotonMeGustaComp.GetComponent<UnityEngine.UI.Text>().text = "Inicia sesion";
                    textoBotonGuardarComp.GetComponent<UnityEngine.UI.Text>().text = "Inicia sesion";
                    botonMeGustaComp.interactable = false;
                    botonGuardarComp.interactable = false;
                }

                GameObject.FindGameObjectWithTag("popupInfoNivelContenedor").transform.localScale = new Vector3(1, 1, 1);
                //id, string, nombre, creador, numLikes, likeYo, favoritoYo
            }
        }
    }

    public void destruir()
    {
        Destroy(GameObject.FindWithTag("popupInformacionNivel"));
        PlayerPrefs.DeleteKey("nivelNombre");
        PlayerPrefs.DeleteKey("nivelCreador");
        PlayerPrefs.DeleteKey("nivelLikes");
        PlayerPrefs.DeleteKey("nivelId");
        PlayerPrefs.DeleteKey("nivelStringGen");
        PlayerPrefs.DeleteKey("prueba");
        PlayerPrefs.DeleteKey("guardado");
        PlayerPrefs.DeleteKey("nivelFoto");
        Datos.borrarNivelSeleccionado();
    }

    public void empezarNivel()
    {
        SceneManager.LoadScene("Juego");
    }

    public void guardarNivel()
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

    public void gustarNivel()
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
            Destroy(this.gameObject);
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
            Destroy(this.gameObject);
            GameObject.FindGameObjectWithTag("popupErrorConexion").GetComponent<popupErrorConexion>().mostrarPopup();
        }
        else
        {
            if (Datos.getNivelSeleccionado().tieneMeGusta().Equals("1"))
            {
                Datos.getNivelSeleccionado().quitarMeGusta();
                textoBotonMeGustaComp.GetComponent<UnityEngine.UI.Text>().text = "Me gusta";
                likesNivelComp.GetComponent<UnityEngine.UI.Text>().text = "Me gusta: " + (int.Parse(likesNivelComp.GetComponent<UnityEngine.UI.Text>().text.Split(' ')[likesNivelComp.GetComponent<UnityEngine.UI.Text>().text.Split(' ').Length - 1]) - 1);
                Datos.getNivelSeleccionado().reducirLikes();
            }
            else
            {
                Datos.getNivelSeleccionado().ponerMeGusta();
                textoBotonMeGustaComp.GetComponent<UnityEngine.UI.Text>().text = "Quitar Me gusta";
                likesNivelComp.GetComponent<UnityEngine.UI.Text>().text = "Me gusta: " + (int.Parse(likesNivelComp.GetComponent<UnityEngine.UI.Text>().text.Split(' ')[likesNivelComp.GetComponent<UnityEngine.UI.Text>().text.Split(' ').Length - 1]) + 1);
                Datos.getNivelSeleccionado().aumentarLikes();
            }
            SistemaGuardado.guardar();
        }
    }


    private IEnumerator coBorrarNivel()
    {
        WWWForm datos = new WWWForm();
        datos.AddField("nivel", PlayerPrefs.GetString("nivelId"));

        UnityWebRequest peticion = UnityWebRequest.Post("https://igar999.me/borrarNivel", datos);
        peticion.SetRequestHeader("content-Type", "application/x-www-form-urlencoded");
        peticion.SetRequestHeader("Accept", "application/x-www-form-urlencoded");
        peticion.SetRequestHeader("api-version", "1.1");

        peticion.certificateHandler = new aceptarcertificado();
        yield return peticion.Send();
        if (peticion.isNetworkError || peticion.isHttpError)
        {
            Destroy(this.gameObject);
            GameObject.FindGameObjectWithTag("popupErrorConexion").GetComponent<popupErrorConexion>().mostrarPopup();
        }
        print(peticion.downloadHandler.text);
    }

    public void borrarNivel()
    {
        StartCoroutine(coBorrarNivel());
        Destroy(this.gameObject);
        Datos.borrarNivelSeleccionado();
        PlayerPrefs.SetInt("actualizar",1);
    }

    public void mostrarCuadroBorrar()
    {
        GameObject.FindGameObjectWithTag("cuadroBorrar").transform.localScale = new Vector3(1, 1, 1);
    }

    public void ocultarCuadroBorrar()
    {
        GameObject.FindGameObjectWithTag("cuadroBorrar").transform.localScale = new Vector3(0, 0, 0);
    }
}
