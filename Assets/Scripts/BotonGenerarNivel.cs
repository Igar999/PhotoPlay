using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonGenerarNivel : MonoBehaviour
{
    // Start is called before the first frame update

    private UnityEngine.UI.InputField textoNombreNivel;
    void Start()
    {
        textoNombreNivel = GameObject.FindGameObjectWithTag("cn_nombreNivel").GetComponent<UnityEngine.UI.InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (textoNombreNivel.text.Equals(""))
        {
            transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
