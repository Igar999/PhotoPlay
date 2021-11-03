using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AccederNivel: MonoBehaviour
{
    public void cn_sacarFoto() {
        
        bool sacar = false;
        NativeCamera.Permission permiso = NativeCamera.CheckPermission();
        if (permiso == NativeCamera.Permission.Granted)
        {
            sacar = true;
        }
        else
        {
            NativeCamera.Permission permisoNuevo = NativeCamera.RequestPermission();
            if (permisoNuevo == NativeCamera.Permission.Granted)
            {
                sacar = true;
            }
        }

        if (sacar)
        {
            NativeCamera.Permission permiso2 = NativeCamera.TakePicture((foto) =>
            {
                if (foto != null)
                {
                    PlayerPrefs.SetString("rutaFoto", foto);
                    cn_ajustarNivel();
                }
                else
                {
                    SceneManager.LoadScene("Menú");
                }
            }, -1);
        }
        
    }

    public void cn_ajustarNivel()
    {
        SceneManager.LoadScene("AjustesCrearNivel");

    }

    private IEnumerator conexionWeb(int deslizadorEnemigos, int deslizadorMonedas)
    {
        GameObject.FindGameObjectWithTag("cargando").transform.localScale = new Vector3(1, 1, 1);
        byte[] foto = File.ReadAllBytes(PlayerPrefs.GetString("rutaFoto")); 

        string fotoString = Convert.ToBase64String(foto);

        WWWForm datos = new WWWForm();
        datos.AddField("foto", fotoString);
        datos.AddField("enemigos", deslizadorEnemigos);
        datos.AddField("monedas", deslizadorMonedas);

        UnityWebRequest peticion = UnityWebRequest.Post("https://igar999.me/py", datos);
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
            string nombreNivel = GameObject.FindGameObjectWithTag("cn_nombreNivel").GetComponent<UnityEngine.UI.InputField>().text;
            string strGenNivel = peticion.downloadHandler.text.ToString();

            cn_comprobarNivel(nombreNivel, strGenNivel);
        }

    }

    private void cn_comprobarNivel(string nombreNivel, string strGenNivel)
    {
        bool todoCorrecto = true;

        int casillasAire = Regex.Matches(strGenNivel, "_").Count;
        int numCasillas = strGenNivel.Length;

        if ((float)casillasAire / (float)numCasillas < 0.1f)
        {
            todoCorrecto = false;
        }

        if (todoCorrecto)
        {
            bool hayCamino = false;
            (int, int) casillaInicial = (0, 0);
            (int, int) casillaActual = (0, 0);
            string[] casillas = strGenNivel.Split('/');
            for (int i = 0; i < casillas.Length - 1; i++)
            {
                if (casillas[i][0].Equals('O'))
                {
                    casillaInicial = (i, 0);
                    break;
                }
            }
            List<(int, int)> casillasPorVer = new List<(int, int)>();
            List<(int, int)> casillasVistas = new List<(int, int)>();
            casillasPorVer.Add(casillaInicial);

            while (casillasPorVer.Count > 0 && !hayCamino)
            {
                casillaActual = casillasPorVer[0];
                casillasPorVer.RemoveAt(0);
                int h = 0;
                int w = 0;
                (h, w) = casillaActual;
                casillasVistas.Add(casillaActual);
                if (h < casillas.Length-1)
                {
                    if (casillas[h + 1][w].Equals('P'))
                    {
                        hayCamino = true;
                    }
                    else if (casillas[h + 1][w].Equals('_') && !casillasVistas.Contains((h + 1, w)))
                    {
                        casillasPorVer.Add((h + 1, w));
                        casillasVistas.Add((h + 1, w));
                    }
                }

                if (h > 0)
                {
                    if (casillas[h - 1][w].Equals('P'))
                    {
                        hayCamino = true;
                    }
                    else if (casillas[h - 1][w].Equals('_') && !casillasVistas.Contains((h - 1, w)))
                    {
                        casillasPorVer.Add((h - 1, w));
                        casillasVistas.Add((h - 1, w));
                    }
                }

                if (w < casillas[0].Length-1)
                {
                    if (casillas[h][w + 1].Equals('P'))
                    {
                        hayCamino = true;
                    }
                    else if (casillas[h][w + 1].Equals('_') && !casillasVistas.Contains((h, w + 1)))
                    {
                        casillasPorVer.Add((h, w + 1));
                        casillasVistas.Add((h, w + 1));
                    }
                }

            }
            todoCorrecto = todoCorrecto && hayCamino;
        }

        if (todoCorrecto)
        {
            cn_accederNivel(nombreNivel, strGenNivel);
        }
        else
        {
            SceneManager.LoadScene("ErrorNivel");
        }

    }

    class aceptarcertificado : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menú");

            PlayerPrefs.DeleteKey("nivelNombre");
            PlayerPrefs.DeleteKey("nivelStringGen");
        }
    }

    public void cn_subirFoto()
    {
        int enemigos = (int)GameObject.FindGameObjectWithTag("deslizadorEnemigos").GetComponent<UnityEngine.UI.Slider>().value;
        int monedas = (int)GameObject.FindGameObjectWithTag("deslizadorMonedas").GetComponent<UnityEngine.UI.Slider>().value;

        StartCoroutine(conexionWeb(enemigos, monedas));
    }

    public void cn_accederNivel(string nombreNivel, string strGenNivel)
    {
        PlayerPrefs.SetString("nivelNombre", nombreNivel);
        PlayerPrefs.SetString("nivelStringGen", strGenNivel);
        PlayerPrefs.SetString("prueba", "true");


        Texture2D textura = generarImagen(strGenNivel);
        byte[] bytes = textura.EncodeToPNG();
        PlayerPrefs.SetString("nivelFoto", System.Convert.ToBase64String(bytes));
        
        
        SceneManager.LoadScene("Juego");
    }

    public void cn_publicarNivel()
    {
        StartCoroutine(subirNivel(PlayerPrefs.GetString("nivelStringGen"), PlayerPrefs.GetString("nivelNombre")));
    }

    private IEnumerator subirNivel(string strGen, string nombre)
    {
        GameObject.FindGameObjectWithTag("cargando").transform.localScale = new Vector3(1, 1, 1);
        WWWForm datos = new WWWForm();
        datos.AddField("string", strGen);
        datos.AddField("nombre", nombre);
        datos.AddField("creador", PlayerPrefs.GetString("usuarioActual"));

        UnityWebRequest peticion = UnityWebRequest.Post("https://igar999.me/addNivel", datos);
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
                PlayerPrefs.DeleteKey("rutaFoto");
                PlayerPrefs.DeleteKey("nivelNombre");
                PlayerPrefs.DeleteKey("nivelStringGen");
                PlayerPrefs.DeleteKey("prueba");

                SceneManager.LoadScene("Menú");
            }

        }

    }



    public void abrirAjustes()
    {
        SceneManager.LoadScene("Ajustes");
    }

    public void abrirInfoJuego()
    {
        SceneManager.LoadScene("InfoJuego");
    }

    public void volverAMenu()
    {
        SceneManager.LoadScene("Menú");
    }

    public Texture2D generarImagen(string strGen)
    {
        string[] lineas = strGen.Split('/');

        Texture2D textura = new Texture2D(lineas[0].Length, lineas.Length);

        for (int i = 0; i < lineas.Length - 1; i++)
        {
            for (int j = 0; j < lineas[0].Length - 1; j++)
            {
                char caracter = lineas[i][j];
                if (caracter.Equals('_'))
                {
                    textura.SetPixel(j, lineas.Length - i - 1, new Color32(27, 180, 222, 255));
                }
                else if (caracter.Equals('X'))
                {
                    textura.SetPixel(j, lineas.Length - i - 1, new Color32(145, 57, 16, 255));
                }
                else if (caracter.Equals('Y'))
                {
                    textura.SetPixel(j, lineas.Length - i - 1, new Color32(29, 156, 12, 255));
                }
                else if (caracter.Equals('O'))
                {
                    textura.SetPixel(j, lineas.Length - i - 1, new Color32(214, 34, 34, 255));
                }
                else if (caracter.Equals('P'))
                {
                    textura.SetPixel(j, lineas.Length - i - 1, new Color32(201, 201, 22, 255));
                }
                else if (caracter.Equals('E'))
                {
                    textura.SetPixel(j, lineas.Length - i - 1, new Color32(0, 0, 0, 255));
                }
                else if (caracter.Equals('M'))
                {
                    textura.SetPixel(j, lineas.Length - i - 1, new Color32(201, 201, 22, 255));
                }
                else
                {
                    textura.SetPixel(j, lineas.Length - i - 1, new Color32(0, 0, 0, 255));
                }
            }
        }
        textura.Apply();
        return textura;
    }
}
