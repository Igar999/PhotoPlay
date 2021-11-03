using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SacarFoto : MonoBehaviour {

    public RawImage foto;
    private WebCamTexture webCamTexture;

    void Start()
    {
        WebCamDevice[] camaras = WebCamTexture.devices;

        if (camaras.Length > 0)
        {
            for (int i = 0; i < camaras.Length; i++)
            {
                var actual = camaras[i];
                if (actual.isFrontFacing)
                {
                    webCamTexture = new WebCamTexture(actual.name, Screen.width, Screen.height);
                    break;
                }
            }
        }
        if (webCamTexture != null)
        {
            webCamTexture.Play();
            foto.texture = webCamTexture;
            Debug.Log(webCamTexture.videoRotationAngle);
            
        }
        
    }

    void Update()
    {

        float scaleY = webCamTexture.videoVerticallyMirrored ? -1f : 1f;
        foto.transform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -webCamTexture.videoRotationAngle;
        foto.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

        foto.texture = webCamTexture;
    }
}
