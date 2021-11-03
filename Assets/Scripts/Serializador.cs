//https://forum.unity.com/threads/simple-local-data-storage.468936/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Serializador : MonoBehaviour
{

    public void guardar(object objeto, string archivo)
    {
        string ruta = Application.persistentDataPath + "/" + archivo + ".bin";
        BinaryFormatter Formatter = new BinaryFormatter();
        FileStream stream = new FileStream(ruta, FileMode.Create);
        Formatter.Serialize(stream, objeto);
        stream.Close();
    }

    public object cargar(string archivo)
    {
        string ruta = Application.persistentDataPath + "/" + archivo + ".bin";
        if (File.Exists(ruta))
        {
            BinaryFormatter formateador = new BinaryFormatter();
            FileStream stream = new FileStream(ruta, FileMode.Open);
            object objeto = formateador.Deserialize(stream);
            stream.Close();
            // Return the uncast untyped object.
            return objeto;
        }
        else
        {
            return null;
        }
    }

}
