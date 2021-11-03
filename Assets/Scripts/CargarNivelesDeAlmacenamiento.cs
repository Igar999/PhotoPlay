using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CargarNivelesDeAlmacenamiento : MonoBehaviour
{

    public GameObject contenedorNiveles;
    public GameObject prefabNivel;

    // Start is called before the first frame update
    void Start()
    {
        SistemaGuardado.cargar();

        PlayerPrefs.SetString("lista", "nivelesMundiales");
        PlayerPrefs.SetInt("actualizar", 0);
        cargarNivelesMundiales();
    }

    private void Update()
    {
        if(PlayerPrefs.GetInt("actualizar") == 1)
        {
            PlayerPrefs.SetInt("actualizar", 0);

            if (PlayerPrefs.GetString("lista").Equals("misNiveles"))
            {
                cargarMisNiveles();
            }
            else if (PlayerPrefs.GetString("lista").Equals("nivelesGuardados"))
            {
                cargarNivelesGuardados();
            }
        }
    }

    private void cargarNivel(NivelDatos nivelDatos)
    {
        GameObject nivelX = Instantiate(prefabNivel);
        nivelX.GetComponent<NivelPrefab>().setDatos(nivelDatos);
        nivelX.transform.SetParent(contenedorNiveles.transform, false);
    }

    public void cargarMisNiveles()
    {
        PlayerPrefs.SetString("lista", "misNiveles");
        GameObject.FindGameObjectWithTag("tituloLista").GetComponent<UnityEngine.UI.Text>().text = "Mis niveles";
        StartCoroutine(obtenerNiveles());
    }

    public void cargarNivelesGuardados()
    {
        PlayerPrefs.SetString("lista", "nivelesGuardados");
        GameObject.FindGameObjectWithTag("tituloLista").GetComponent<UnityEngine.UI.Text>().text = "Niveles favoritos";
        StartCoroutine(obtenerNiveles());
    }

    public void cargarNivelesMundiales()
    {
        PlayerPrefs.SetString("lista", "nivelesMundiales");
        GameObject.FindGameObjectWithTag("tituloLista").GetComponent<UnityEngine.UI.Text>().text = "Niveles mundiales";
        StartCoroutine(obtenerNiveles());

    }

    public void cargarNivelesDeUsuario(string nombreUsuario)
    {
        PlayerPrefs.SetString("lista", "nivelesJugador");
        PlayerPrefs.SetString("usuarioBuscar", nombreUsuario);
        GameObject.FindGameObjectWithTag("tituloLista").GetComponent<UnityEngine.UI.Text>().text = "Niveles de " + nombreUsuario;
        StartCoroutine(obtenerNiveles());
    }


    class aceptarcertificado : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }


    private IEnumerator obtenerNiveles()
    {
        GameObject.FindGameObjectWithTag("cargando").transform.localScale = new Vector3(1, 1, 1);
        foreach (Transform nivel in contenedorNiveles.transform)
        {
            Destroy(nivel.gameObject);
        }
        WWWForm datos = new WWWForm();
        datos.AddField("usuarioActual", PlayerPrefs.GetString("usuarioActual"));

        UnityWebRequest peticion = new UnityWebRequest();
        if (PlayerPrefs.GetString("lista").Equals("nivelesMundiales"))
        {
            peticion = UnityWebRequest.Post("https://igar999.me/getNivelesMundiales", datos);
        }
        else if (PlayerPrefs.GetString("lista").Equals("misNiveles"))
        {
            datos.AddField("usuario", PlayerPrefs.GetString("usuarioActual"));
            peticion = UnityWebRequest.Post("https://igar999.me/getNivelesDeUsuario", datos);
        }else if (PlayerPrefs.GetString("lista").Equals("nivelesGuardados"))
        {
            peticion = UnityWebRequest.Post("https://igar999.me/getNivelesFavoritos", datos);
        }else if (PlayerPrefs.GetString("lista").Equals("nivelesJugador"))
        {
            datos.AddField("usuario", PlayerPrefs.GetString("usuarioBuscar"));
            PlayerPrefs.DeleteKey("usuarioBuscar");
            peticion = UnityWebRequest.Post("https://igar999.me/getNivelesDeUsuario", datos);
        }

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
            string cadena = "";
            if (peticion.downloadHandler.text.Length > 0)
            {
                cadena = peticion.downloadHandler.text.Substring(0, peticion.downloadHandler.text.Length - 1);

                string[] datosNiveles = cadena.Split('-');

                foreach (string datosNivel in datosNiveles)
                {

                    string[] datosSeparados = datosNivel.Split(',');
                    string id = datosSeparados[0];
                    string strGen = datosSeparados[1];
                    string nombre = datosSeparados[2];
                    string creador = datosSeparados[3];
                    string numLikes = datosSeparados[4];
                    string likeYo = datosSeparados[5];
                    string favoritoYo = datosSeparados[6];


                    NivelDatos nivelDatos = new NivelDatos(id, nombre, creador, numLikes, strGen, likeYo, favoritoYo);
                    GameObject nivelX = Instantiate(prefabNivel);
                    nivelX.GetComponent<NivelPrefab>().setDatos(nivelDatos);
                    nivelX.transform.SetParent(contenedorNiveles.transform, false);
                    //id, string, nombre, creador, numLikes, likeYo, favoritoYo
                }
            }
        }
    }
}
