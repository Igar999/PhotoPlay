using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupPausa : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("popupPausa").transform.localScale = new Vector3(0, 0, 0);
        Time.timeScale = 1;
    }

    public void seguirJugando()
    {
        GameObject.FindGameObjectWithTag("botonPausa").transform.localScale = new Vector3(1, 1, 1);
        Time.timeScale = 1;
        GameObject.FindGameObjectWithTag("popupPausa").transform.localScale = new Vector3(0, 0, 0);
    }

    public void volverAJugar()
    {
        SceneManager.LoadScene("Juego");
    }

}
