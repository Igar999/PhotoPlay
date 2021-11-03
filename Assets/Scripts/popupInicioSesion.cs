using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class popupInicioSesion : MonoBehaviour
{ 
    public UnityEngine.UI.InputField campoUsuario;
    public UnityEngine.UI.InputField campoContra;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("popupInicioSesion").transform.localScale = new Vector3(0, 0, 0);
        GameObject.FindGameObjectWithTag("barreraInicioSesion").transform.localScale = new Vector3(0, 0, 0);

    }


    public void mostrarPopup()
    {
        GameObject.FindGameObjectWithTag("popupInicioSesion").transform.localScale = new Vector3(1, 1, 1);
        GameObject.FindGameObjectWithTag("barreraInicioSesion").transform.localScale = new Vector3(1, 1, 1);
        campoUsuario.text = "";
        campoContra.text = "";
        GameObject.FindGameObjectWithTag("errorInicioSesion").GetComponent<UnityEngine.UI.Text>().text = "";
    }

    public void ocultarPopup()
    {
        GameObject.FindGameObjectWithTag("popupInicioSesion").transform.localScale = new Vector3(0, 0, 0);
        GameObject.FindGameObjectWithTag("barreraInicioSesion").transform.localScale = new Vector3(0, 0, 0);
    }

    public void iniciarSesion()
    {
        string usuario = campoUsuario.text;
        string contra = campoContra.text;

        if(usuario.Equals("") || contra.Equals(""))
        {
            GameObject.FindGameObjectWithTag("errorInicioSesion").GetComponent<UnityEngine.UI.Text>().text = "Rellena los campos";
        }
        else
        {
            StartCoroutine(inicioSesion(usuario, contra));
        }

    }


    class aceptarcertificado : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    private IEnumerator inicioSesion(string usuario, string contra)
    {
        GameObject.FindGameObjectWithTag("cargando").transform.localScale = new Vector3(1, 1, 1);
        string contraEnc = System.Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(contra)));

        WWWForm datos = new WWWForm();
        datos.AddField("nombre", usuario);
        datos.AddField("contra", contraEnc);

        UnityWebRequest peticion = UnityWebRequest.Post("https://igar999.me/inicioSesion", datos);
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
            string mensaje = peticion.downloadHandler.text.ToString();
            if (mensaje.Equals("Correcto"))
            {
                PlayerPrefs.SetString("usuarioActual", usuario);
                GameObject.FindGameObjectWithTag("errorInicioSesion").GetComponent<UnityEngine.UI.Text>().text = "";
                GameObject.FindGameObjectWithTag("botonRegistro").transform.localScale = new Vector3(0, 0, 0);
                GameObject.FindGameObjectWithTag("botonInicioSesion").transform.localScale = new Vector3(0, 0, 0);
                GameObject.FindGameObjectWithTag("popupInicioSesion").transform.localScale = new Vector3(0, 0, 0);
                GameObject.FindGameObjectWithTag("barreraInicioSesion").transform.localScale = new Vector3(0, 0, 0);

                GameObject.FindGameObjectWithTag("textoNombreUsuario").GetComponent<UnityEngine.UI.Text>().text = usuario;
                GameObject.FindGameObjectWithTag("cuadroNombreUsuario").transform.localScale = new Vector3(1, 1, 1);
                GameObject.FindGameObjectWithTag("botonCerrarSesion").transform.localScale = new Vector3(1, 1, 1);

                SistemaGuardado.guardar();
            }
            else
            {
                GameObject.FindGameObjectWithTag("errorInicioSesion").GetComponent<UnityEngine.UI.Text>().text = mensaje;
            }
        }

    }
}