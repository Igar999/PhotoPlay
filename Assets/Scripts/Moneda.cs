using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moneda : MonoBehaviour
{
    public Sprite[] moneda = new Sprite[8];
    private int estado;
    private float tiempoCambio;
    // Start is called before the first frame update
    void Start()
    {
        estado = 0;   
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - tiempoCambio > 0.06225f)
        {
            GetComponent<SpriteRenderer>().sprite = moneda[estado];
            estado = estado + 1;
            if (estado == 8)
            {
                estado = 0;
            }
            tiempoCambio = Time.time;
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.GetComponent<CircleCollider2D>().enabled = false;
        if (collision.gameObject.name.Equals("Bola(Clone)"))
        {
            GetComponent<AudioSource>().enabled = true;
            GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volumenSonido");
            GetComponent<AudioSource>().Play();
            transform.localScale = new Vector3(0, 0, 0);
            int monedas = int.Parse(GameObject.FindGameObjectWithTag("monedas").GetComponent<UnityEngine.UI.Text>().text.Split(' ')[1]);
            GameObject.FindGameObjectWithTag("monedas").GetComponent<UnityEngine.UI.Text>().text = "Monedas: " + (monedas + 1);
        }
        else
        {
            this.GetComponent<CircleCollider2D>().enabled = true;
        }
    }
}
