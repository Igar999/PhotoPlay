using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NivelPrefab : MonoBehaviour
{

    public GameObject fotoNivelComp;
    public GameObject nombreNivelComp;
    public GameObject creadorNivelComp;
    public GameObject likesNivelComp;

    public GameObject prefabPopupInfo;

    public NivelDatos datos;


    public void setDatos(NivelDatos datos)
    {
        this.datos = datos;
        rellenarDatos(datos.getNombre(), datos.getCreador(), datos.getLikes(), datos.getId(), datos.getStrGen());
    }

    public void setFotoNivel(GameObject pFotoNivel) {
        this.fotoNivelComp = pFotoNivel;
    }

    public void setNombreNivel(GameObject pNombreNivel)
    {
        this.nombreNivelComp = pNombreNivel;
    }

    public void setCreadorNivel(GameObject pCreadorNivel)
    {
        this.creadorNivelComp = pCreadorNivel;
    }

    public void setLikesNivel(GameObject pLikesNivel)
    {
        this.likesNivelComp = pLikesNivel;
    }

    public string esNivelGuardado()
    {
        return this.datos.esGuardado();
    }

    private void rellenarDatos(string nombre, string creador, string likes, string id, string stringGen)
    {
        nombreNivelComp.GetComponent<UnityEngine.UI.Text>().text = nombre;
        creadorNivelComp.GetComponent<UnityEngine.UI.Text>().text = creador;
        likesNivelComp.GetComponent<UnityEngine.UI.Text>().text = "Me gusta: " + likes;
        fotoNivelComp.GetComponent<UnityEngine.UI.RawImage>().texture = GameObject.FindGameObjectWithTag("barraInferior").GetComponent<AccederNivel>().generarImagen(stringGen);
    }

    public void mostrarPopup()
    {
        PlayerPrefs.SetString("nivelNombre", datos.getNombre());
        PlayerPrefs.SetString("nivelCreador", datos.getCreador());
        PlayerPrefs.SetString("nivelLikes", datos.getLikes());
        PlayerPrefs.SetString("nivelId", datos.getId());
        PlayerPrefs.SetString("nivelStringGen", datos.getStrGen());
        PlayerPrefs.SetString("nivelLike", datos.tieneMeGusta());
        PlayerPrefs.SetString("nivelFavorito", datos.esGuardado());
        PlayerPrefs.SetString("prueba", "false");
        
        Texture2D textura = fotoNivelComp.GetComponent<UnityEngine.UI.RawImage>().texture as Texture2D;
        byte[] bytes = textura.EncodeToPNG();
        PlayerPrefs.SetString("nivelFoto", System.Convert.ToBase64String(bytes));

        Datos.setNivelSeleccionado(datos);
        Instantiate(prefabPopupInfo, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
