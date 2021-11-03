using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class popupRegistro : MonoBehaviour
{

    public UnityEngine.UI.InputField campoUsuario;
    public UnityEngine.UI.InputField campoContra;
    public UnityEngine.UI.InputField campoContraVerificacion;


    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("popupRegistro").transform.localScale = new Vector3(0, 0, 0);
        GameObject.FindGameObjectWithTag("barreraRegistro").transform.localScale = new Vector3(0, 0, 0);
    }


    public void mostrarPopup()
    {
        GameObject.FindGameObjectWithTag("popupRegistro").transform.localScale = new Vector3(1, 1, 1);
        GameObject.FindGameObjectWithTag("barreraRegistro").transform.localScale = new Vector3(1, 1, 1);
        campoUsuario.text = "";
        campoContra.text = "";
        campoContraVerificacion.text = "";
        GameObject.FindGameObjectWithTag("errorRegistro").GetComponent<UnityEngine.UI.Text>().text = "";
    }

    public void ocultarPopup()
    {
        GameObject.FindGameObjectWithTag("popupRegistro").transform.localScale = new Vector3(0, 0, 0);
        GameObject.FindGameObjectWithTag("barreraRegistro").transform.localScale = new Vector3(0, 0, 0);
    }

    public void registrar()
    {
        string usuario = campoUsuario.text;
        string contra = campoContra.text;
        string contraVerificacion = campoContraVerificacion.text;

        if (usuario.Equals("") || contra.Equals("") || contraVerificacion.Equals(""))
        {
            GameObject.FindGameObjectWithTag("errorRegistro").GetComponent<UnityEngine.UI.Text>().text = "Rellena los campos";
        }
        else if (!contra.Equals(contraVerificacion))
        {
            GameObject.FindGameObjectWithTag("errorRegistro").GetComponent<UnityEngine.UI.Text>().text = "Las contraseñas no coinciden";
        }
        else
        {
            StartCoroutine(registro(usuario, contra));
        }
    }


    class aceptarcertificado : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    private IEnumerator registro(string usuario, string contra)
    {
        GameObject.FindGameObjectWithTag("cargando").transform.localScale = new Vector3(1, 1, 1);
        string contraEnc = System.Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(contra)));

        WWWForm datos = new WWWForm();
        datos.AddField("nombre", usuario);
        datos.AddField("contra", contraEnc);

        UnityWebRequest peticion = UnityWebRequest.Post("https://igar999.me/addUsuario", datos);
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
                GameObject.FindGameObjectWithTag("errorRegistro").GetComponent<UnityEngine.UI.Text>().text = "";
                GameObject.FindGameObjectWithTag("botonInicioSesion").transform.localScale = new Vector3(0, 0, 0);
                GameObject.FindGameObjectWithTag("botonRegistro").transform.localScale = new Vector3(0, 0, 0);
                GameObject.FindGameObjectWithTag("popupRegistro").transform.localScale = new Vector3(0, 0, 0);
                GameObject.FindGameObjectWithTag("barreraRegistro").transform.localScale = new Vector3(0, 0, 0);

                GameObject.FindGameObjectWithTag("textoNombreUsuario").GetComponent<UnityEngine.UI.Text>().text = usuario;
                GameObject.FindGameObjectWithTag("cuadroNombreUsuario").transform.localScale = new Vector3(1, 1, 1);
                GameObject.FindGameObjectWithTag("botonCerrarSesion").transform.localScale = new Vector3(1, 1, 1);
                
                SistemaGuardado.guardar();

            }
            else
            {
                GameObject.FindGameObjectWithTag("errorRegistro").GetComponent<UnityEngine.UI.Text>().text = mensaje;
            }
        }

    }
}