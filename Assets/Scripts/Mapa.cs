using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapa : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        byte[] bytes = System.Convert.FromBase64String(PlayerPrefs.GetString("nivelFoto"));
        Texture2D foto = new Texture2D(1, 1);
        foto.LoadImage(bytes);
        foto.Apply();
        GameObject.FindGameObjectWithTag("mapa").GetComponent<UnityEngine.UI.RawImage>().texture = foto;
        GameObject.FindGameObjectWithTag("mapaContenedor").transform.localScale = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float tamañoVertical = PlayerPrefs.GetInt("numFilasMapa");
        float tamañoHorizontal = PlayerPrefs.GetInt("numColumnasMapa");

        float tamañoImagenVertical = GameObject.FindGameObjectWithTag("mapa").GetComponent<RectTransform>().rect.height;
        float tamañoImagenHorizontal = GameObject.FindGameObjectWithTag("mapa").GetComponent<RectTransform>().rect.width;

        float coordVert = GameObject.FindGameObjectWithTag("jugador").transform.position.y + 2;
        float coordHor = GameObject.FindGameObjectWithTag("jugador").transform.position.x - 7.5f;

        float posVert = (tamañoImagenVertical / tamañoVertical) * coordVert;
        float posHor = (tamañoImagenHorizontal / tamañoHorizontal) * coordHor;
        GameObject.FindGameObjectWithTag("mapaPunto").transform.position = transform.TransformPoint(new Vector2(posHor, posVert));
    }

    public void mostrarOcultarMapa()
    {
        if((int)GameObject.FindGameObjectWithTag("mapaContenedor").transform.localScale.x == 0)
        {
            GameObject.FindGameObjectWithTag("mapaContenedor").transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            GameObject.FindGameObjectWithTag("mapaContenedor").transform.localScale = new Vector3(0, 0, 0);
        }
    }


}
