using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje: MonoBehaviour
{

    public float velocidad = 9.0f;
    private Rigidbody2D personaje;
    public Transform comprobacionSuelo;
    public Transform comprobacionPared;
    public Transform centroGancho;
    public float salto = 3f;
    public Transform yo;
    private float tiempoSalto;
    private bool estaEnSuelo = true;
    bool puedeSaltar = false;
    bool chocaPared = false;
    private int numColumnas;
    private int estadoGancho;
    private Transform puntoCuerda;
    private LineRenderer cuerdaRender;
    private Vector3 puntoFinGancho;
    private bool mirandoDerecha = true;
    private bool ganchoDerecha;
    public AudioClip audioLanzarGancho;
    public AudioClip audioSaltoGancho;
    public AudioClip audioSalto;

    //private bool estaEnSuelo;

    // Start is called before the first frame update
    void Start()
    {
        puntoCuerda = GameObject.FindGameObjectWithTag("puntoCuerda").transform;
        puntoCuerda.position = new Vector3(puntoCuerda.position.x, puntoCuerda.position.y, 0);
        cuerdaRender = GameObject.FindGameObjectWithTag("cuerda").GetComponent<LineRenderer>();
        numColumnas = PlayerPrefs.GetInt("numColumnasMapa");
        personaje = GetComponent<Rigidbody2D>();
        tiempoSalto = Time.time;
        GameObject.FindGameObjectWithTag("camara").GetComponent<Camara>().establecerPersonaje(transform);
        estadoGancho = 0;
    }

    // Update is called once per frame
    void Update()
    {
        puedeSaltar = false;
        if (Time.time - tiempoSalto > 0.5f)
        {
            puedeSaltar = true;
        }

        RaycastHit2D raycastSuelo = Physics2D.Raycast(comprobacionSuelo.position, Vector2.right, (4/10));
        RaycastHit2D raycastPared = Physics2D.Raycast(comprobacionPared.position, Vector2.down, (6/10));
        estaEnSuelo = true;
        if (raycastSuelo.collider == false)
        {
            estaEnSuelo = false;
        }

        chocaPared = false;
        if(raycastPared.collider != false)
        {
            if (raycastPared.collider.name == "Hierba(Clone)" || raycastPared.collider.name == "Tierra(Clone)")
            {
                chocaPared = true;
            }
        }

        if (GameObject.FindGameObjectWithTag("botonesMovimiento").GetComponent<BotonesMovimiento>().derecha())
        {
            moverDerecha();
        }
        if (GameObject.FindGameObjectWithTag("botonesMovimiento").GetComponent<BotonesMovimiento>().izquierda())
        {
            moverIzquierda();
        }
        if (GameObject.FindGameObjectWithTag("botonesMovimiento").GetComponent<BotonesMovimiento>().gancho())
        {
            gancho();
        }

        if (Input.GetKey("a")){
            moverIzquierda();
        }
        if (Input.GetKey("d")){
            moverDerecha();
        }
        if (Input.GetKey("space")){
            saltar();
        }
        if (Input.GetKey("w"))
        {
            gancho();
        }


        if (!Input.GetKey("w") && !GameObject.FindGameObjectWithTag("botonesMovimiento").GetComponent<BotonesMovimiento>().gancho())
        {
            estadoGancho = 0;
            puntoFinGancho = new Vector3(0, 0, 0);
            cuerdaRender.SetPosition(0, new Vector3(0, 0, 0));
            cuerdaRender.SetPosition(1, new Vector3(0, 0, 0));
        }

    }  


    public void gancho()
    {
        RaycastHit2D raycastGanchoDer = new RaycastHit2D();
        RaycastHit2D raycastGanchoIzq = new RaycastHit2D();
        if (estadoGancho == 0)
        {
            raycastGanchoDer = Physics2D.Raycast(centroGancho.position, new Vector3(1,1, 0), 7);
            raycastGanchoIzq = Physics2D.Raycast(centroGancho.position, new Vector3(-1, 1, 0), 7);

            Vector3 finGanchoDer = raycastGanchoDer.point;
            finGanchoDer.z = 0;
            Vector3 finGanchoIzq = raycastGanchoIzq.point;
            finGanchoIzq.z = 0;

            string ejemploNulo = "(0.0, 0.0, 0.0)";
            puntoFinGancho = new Vector3(0, 0, 0);
            if (!finGanchoDer.ToString().Equals(ejemploNulo) && finGanchoIzq.ToString().Equals(ejemploNulo))
            {
                puntoFinGancho = raycastGanchoDer.point;
                ganchoDerecha = true;
            }
            else if (!finGanchoIzq.ToString().Equals(ejemploNulo) && finGanchoDer.ToString().Equals(ejemploNulo))
            {
                puntoFinGancho = raycastGanchoIzq.point;
                ganchoDerecha = false;
            }
            else if (!finGanchoDer.ToString().Equals(ejemploNulo) && !finGanchoIzq.ToString().Equals(ejemploNulo))
            {
                if (mirandoDerecha)
                {
                    puntoFinGancho = raycastGanchoDer.point;
                    ganchoDerecha = true;
                }
                else
                {
                    puntoFinGancho = raycastGanchoIzq.point;
                    ganchoDerecha = false;
                }
            }

            if (puntoFinGancho.x != 0 || puntoFinGancho.y != 0)
            {
                puntoCuerda.position = new Vector3(transform.position.x, transform.position.y,0);
                estadoGancho = 1;
                GameObject.FindGameObjectWithTag("puntoCuerda").GetComponent<AudioSource>().enabled = true;
                GameObject.FindGameObjectWithTag("puntoCuerda").GetComponent<AudioSource>().clip = audioLanzarGancho;
                GameObject.FindGameObjectWithTag("puntoCuerda").GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volumenSonido");
                GameObject.FindGameObjectWithTag("puntoCuerda").GetComponent<AudioSource>().Play();
            }
        }
        if(estadoGancho == 1)
        {
            if (Vector3.Distance(puntoFinGancho, puntoCuerda.position) < 0.8)
            {
                GameObject.FindGameObjectWithTag("puntoCuerda").GetComponent<AudioSource>().enabled = true;
                GameObject.FindGameObjectWithTag("puntoCuerda").GetComponent<AudioSource>().clip = audioSaltoGancho;
                GameObject.FindGameObjectWithTag("puntoCuerda").GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volumenSonido");
                GameObject.FindGameObjectWithTag("puntoCuerda").GetComponent<AudioSource>().Play();
                estadoGancho = 2;
            }
            else
            {
                Vector3 lanzarGancho = new Vector3(1, 1, 0);
                if(!ganchoDerecha)
                {
                    lanzarGancho = new Vector3(-1, 1, 0);
                }
                puntoCuerda.position += lanzarGancho * 6 * Time.deltaTime;
                Vector3 posTransform = new Vector3(transform.position.x, transform.position.y + 0.35f, 0);
                Vector3 posPuntoCuerda = new Vector3(puntoCuerda.position.x, puntoCuerda.position.y, 0);
                cuerdaRender.SetPosition(0, posTransform);
                cuerdaRender.SetPosition(1, posPuntoCuerda);
            }
        }
        if(estadoGancho == 2)
        {
            Vector3 posTransform = new Vector3(transform.position.x, transform.position.y + 0.35f, 0);
            Vector3 posPuntoFinGancho = new Vector3(puntoFinGancho.x, puntoFinGancho.y, 0);
            cuerdaRender.SetPosition(0, posTransform);
            cuerdaRender.SetPosition(1, posPuntoFinGancho);
            if (Vector3.Distance(puntoFinGancho, transform.position) > 1.2)
            {
                float distX = Mathf.Abs(puntoFinGancho.x - transform.position.x);
                float distY = Mathf.Abs(puntoFinGancho.y - transform.position.y);
                float hip = Mathf.Sqrt(distX * distX + distY * distY);
                float norm = hip * 1 / Mathf.Sqrt(2);


                Vector2 impulso = new Vector2((puntoFinGancho.x - transform.position.x) * (5/2) * norm + 1, (puntoFinGancho.y - transform.position.y) * (5 / 2) * norm + 1);

                impulso.x = Mathf.Min(impulso.x, 15);
                impulso.y = Mathf.Min(impulso.y, 20);
                impulso.x = Mathf.Max(impulso.x, -15);
                impulso.y = Mathf.Max(impulso.y, -20);

                personaje.velocity = impulso;
            }
        }
    }
    public void saltar()
    {
        if(estaEnSuelo && puedeSaltar)
        {
            personaje.velocity += Vector2.up * 14;
            tiempoSalto = Time.time;
            GetComponent<AudioSource>().enabled = true;
            GetComponent<AudioSource>().clip = audioSalto;
            GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volumenSonido");
            GetComponent<AudioSource>().Play();
        }

    }
    public void moverDerecha()
    {
        Vector3 rot = transform.eulerAngles;
        rot.y = 0;
        transform.eulerAngles = rot;
        mirandoDerecha = true;
        personaje.velocity = new Vector2(0, personaje.velocity.y);
        if (!chocaPared && transform.position.x < numColumnas+9)
        {
            transform.position += transform.right * velocidad * Time.deltaTime;
        }else if (chocaPared)
        {
            personaje.velocity = new Vector2(0, personaje.velocity.y);
        }

        
    }

    public void moverIzquierda()
    {
        Vector3 rot = transform.eulerAngles;
        rot.y = -180;
        transform.eulerAngles = rot;
        mirandoDerecha = false;
        personaje.velocity = new Vector2(0, personaje.velocity.y);
        if (!chocaPared && transform.position.x > 10)
        {
            transform.position += transform.right * velocidad * Time.deltaTime;
        }
        else if (chocaPared)
        {
            personaje.velocity = new Vector2(0, personaje.velocity.y);
        }


    }    
}
