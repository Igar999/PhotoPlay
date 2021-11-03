using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonesMovimiento : MonoBehaviour
{

    private bool derechaPulsado = false;
    private bool izquierdaPulsado = false;
    private bool ganchoPulsado = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Quit the application
            if (Time.timeScale == 1)
            {
                pausa();
            }
            else
            {
                GameObject.FindGameObjectWithTag("botonPausa").transform.localScale = new Vector3(1, 1, 1);
                Time.timeScale = 1;
                GameObject.FindGameObjectWithTag("popupPausa").transform.localScale = new Vector3(0, 0, 0);
            }
            
            
        }
    }

    public void saltar()
    {
        GameObject.FindGameObjectWithTag("jugador").GetComponent<Personaje>().saltar();
    }

    public bool derecha()
    {
        return derechaPulsado;
    }

    public bool izquierda()
    {
        return izquierdaPulsado;
    }
    public bool gancho()
    {
        return ganchoPulsado;
    }

    public void botonDerechaEnter()
    {
        derechaPulsado = true;
    }

    public void botonDerechaExit()
    {
        derechaPulsado = false;
    }

    public void botonIzquierdaEnter()
    {
        izquierdaPulsado = true;
    }

    public void botonIzquierdaExit()
    {
        izquierdaPulsado = false;
    }

    public void botonGanchoEnter()
    {
        ganchoPulsado = true;
    }

    public void botonGanchoExit()
    {
        ganchoPulsado = false;
    }

    public void pausa()
    {
        GameObject.FindGameObjectWithTag("popupPausa").transform.localScale = new Vector3(1, 1, 1);
        Time.timeScale = 0;
        GameObject.FindGameObjectWithTag("botonPausa").transform.localScale = new Vector3(0, 0, 0);
    }

}
