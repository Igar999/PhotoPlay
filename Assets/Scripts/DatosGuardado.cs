using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DatosGuardado
{
    public float volumenMusica;
    public float volumenSonido;
    public string usuarioActual;


    public DatosGuardado()
    {
        volumenMusica = 0.5f;
        volumenSonido = 0.5f;
        usuarioActual = "";
    }

}
