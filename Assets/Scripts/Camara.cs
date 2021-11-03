using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{

    public Vector3 desplazamiento;
    public Transform personaje;

    void Update()
    {
        if (personaje)
        {
            Vector3 posicion;

            float posX = Mathf.Min(PlayerPrefs.GetInt("numColumnasMapa")+4f, Mathf.Max(13f, personaje.position.x));
            posicion = new Vector3(posX, personaje.position.y, personaje.position.z);
            posicion += desplazamiento;
            transform.position = posicion;
        }
    }

    public void establecerPersonaje(Transform pPersonaje)
    {
        this.personaje = pPersonaje;
    }
}
