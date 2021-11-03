using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popupErrorConexion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("popupErrorConexion").transform.localScale = new Vector3(0, 0, 0);
        GameObject.FindGameObjectWithTag("barreraErrorConexion").transform.localScale = new Vector3(0, 0, 0);

    }


    public void mostrarPopup()
    {
        GameObject.FindGameObjectWithTag("popupErrorConexion").transform.localScale = new Vector3(1, 1, 1);
        GameObject.FindGameObjectWithTag("barreraErrorConexion").transform.localScale = new Vector3(1, 1, 1);
    }

    public void ocultarPopup()
    {
        GameObject.FindGameObjectWithTag("popupErrorConexion").transform.localScale = new Vector3(0, 0, 0);
        GameObject.FindGameObjectWithTag("barreraErrorConexion").transform.localScale = new Vector3(0, 0, 0);
    }

}
