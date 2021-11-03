using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    bool vaDerecha = true;
    public Transform detectorSuelo;
    public BoxCollider2D colisionMuertePersonaje;
    public CapsuleCollider2D colisionMuerteEnemigo;
    private int estado;
    private float tiempoCambio;
    public Sprite[] enemigo = new Sprite[4];
    public AudioClip muerteEnemigo;
    public AudioClip muertePersonaje;

    // Update is called once per frame
    void Update()
    {
        if (Time.time - tiempoCambio > 0.25f)
        {
            GetComponent<SpriteRenderer>().sprite = enemigo[estado];
            estado = estado + 1;
            if (estado == 4)
            {
                estado = 0;
            }
            tiempoCambio = Time.time;
        }


        Collider2D personaje = GameObject.FindGameObjectWithTag("jugador").GetComponent<CapsuleCollider2D>();
        //print(personaje);

        if (colisionMuertePersonaje.IsTouching(personaje))
        {
            Component script = GameObject.FindGameObjectWithTag("jugador").GetComponent("Personaje");
            Destroy(script);
            colisionMuertePersonaje.transform.localScale = new Vector3(0, 0, 0);
            GameObject.FindGameObjectWithTag("popupMuerte").transform.localScale = new Vector3(1, 1, 1);
            GetComponent<AudioSource>().enabled = true;
            GetComponent<AudioSource>().clip = muertePersonaje;
            GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volumenSonido");
            GetComponent<AudioSource>().Play();
            Time.timeScale = 0;
        }
        else if(colisionMuerteEnemigo.IsTouching(personaje))
        {
            GetComponent<AudioSource>().enabled = true;
            GetComponent<AudioSource>().clip = muerteEnemigo;
            GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volumenSonido");
            GetComponent<AudioSource>().Play();
            //Destroy(this.gameObject);
            transform.localScale = new Vector3(0, 0, 0);
            personaje.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(personaje.gameObject.GetComponent<Rigidbody2D>().velocity.x, 10, 0);
        } 

        transform.Translate(Vector2.right * Time.deltaTime * (3/2));
        RaycastHit2D raycastFinPlataforma = Physics2D.Raycast(detectorSuelo.position, Vector2.down, 1);
        RaycastHit2D raycastChoquePared = Physics2D.Raycast(detectorSuelo.position, Vector2.down, 0);
        bool comprobacionPlataforma = true;
        if (raycastFinPlataforma.collider)
        {
            if (raycastFinPlataforma.collider.name == "Hierba(Clone)" || raycastFinPlataforma.collider.name == "Tierra(Clone)")
            {
                comprobacionPlataforma = false;
            }
        }
        if (comprobacionPlataforma || raycastChoquePared.collider != false)
        {
            Vector3 rotacion = transform.eulerAngles;
            if(vaDerecha)
            {
                rotacion.y = -180;
            }
            else if (!vaDerecha)
            {
                rotacion.y = 0;
            }
            transform.eulerAngles = rotacion;
            vaDerecha = !vaDerecha;
        }

    }
}
