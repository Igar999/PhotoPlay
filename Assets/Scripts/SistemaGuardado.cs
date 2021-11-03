using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//https://forum.unity.com/threads/simple-local-data-storage.468936/
public static class SistemaGuardado
{
    public static void guardar()
    {
        DatosGuardado datosGuardado = Datos.guardarDatos();

        BinaryFormatter bf = new BinaryFormatter();
        string ruta = Application.persistentDataPath + "/datos.ig";
        FileStream stream = new FileStream(ruta, FileMode.Create);

        bf.Serialize(stream, datosGuardado);
        stream.Close();
    }

    public static void cargar()
    {
        string ruta = Application.persistentDataPath + "/datos.ig";
        if (File.Exists(ruta))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(ruta, FileMode.Open);

            if (stream.Length > 0)
            {
                DatosGuardado datosGuardado = bf.Deserialize(stream) as DatosGuardado;
                stream.Close();

                Datos.cargarDatos(datosGuardado);
            }
            else
            {
                Datos.cargarDatos(new DatosGuardado());
            } 
        }
        else
        {
            Debug.Log("No se encuentran datos en " + ruta + "   Creando...");
            FileStream stream = new FileStream(ruta, FileMode.Create);
            stream.Close();
            Datos.cargarDatos(new DatosGuardado());
        }


        

    }
}
