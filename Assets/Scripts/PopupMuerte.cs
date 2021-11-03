using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupMuerte : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("popupMuerte").transform.localScale = new Vector3(0, 0, 0);
    }

    public void volverAJugar()
    {
        SceneManager.LoadScene("Juego");
    }

}
