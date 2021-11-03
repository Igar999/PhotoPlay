using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonesDesactivar : MonoBehaviour
{

    public UnityEngine.UI.Button botonCrear;
    public UnityEngine.UI.Button botonFavoritos;
    public UnityEngine.UI.Button botonMisNiveles;
    public UnityEngine.UI.Text textoCrear;
    public UnityEngine.UI.Text textoFavoritos;
    public UnityEngine.UI.Text textoMisNiveles;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("usuarioActual"))
        {
            if (PlayerPrefs.GetString("usuarioActual").Equals(""))
            {
                textoCrear.text = "X";
                textoFavoritos.text = "Inicia sesion";
                textoMisNiveles.text = "Inicia sesion";
                botonCrear.interactable = false;
                botonFavoritos.interactable = false;
                botonMisNiveles.interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
