using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Datos
{
    private static NivelDatos nivelSeleccionado;

    public static NivelDatos getNivelSeleccionado()
    {
        return nivelSeleccionado;
    }

    public static void setNivelSeleccionado(NivelDatos nivel)
    {
        nivelSeleccionado = nivel;
    }

    public static void borrarNivelSeleccionado()
    {
        nivelSeleccionado = null;
    }

    public static void cargarDatos(DatosGuardado datosGuardado)
    {
        PlayerPrefs.SetFloat("volumenMusica", datosGuardado.volumenMusica);
        PlayerPrefs.SetFloat("volumenSonido", datosGuardado.volumenSonido);
        PlayerPrefs.SetString("usuarioActual", datosGuardado.usuarioActual);
    }

    public static DatosGuardado guardarDatos()
    {

        DatosGuardado datosGuardado = new DatosGuardado();
        datosGuardado.volumenMusica = PlayerPrefs.GetFloat("volumenMusica");
        datosGuardado.volumenSonido = PlayerPrefs.GetFloat("volumenSonido");
        datosGuardado.usuarioActual = PlayerPrefs.GetString("usuarioActual");

        return datosGuardado;
    }
}
