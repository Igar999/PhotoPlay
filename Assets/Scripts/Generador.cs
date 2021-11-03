using UnityEngine;

public class Generador : MonoBehaviour{
	
	private string stringGen;
	char[,] matrizNivel = new char[0, 0];


	public DiccionarioGeneracionPrefabs[] diccionarioCaracteres;
	
    void Start(){
		stringGen = PlayerPrefs.GetString("nivelStringGen");
		GenerarMatriz();
        GenerarNivel();
    }

	public void GenerarMatriz(){
		int numLineas = stringGen.Split('/').Length;
		int numColumnas = stringGen.Split('/')[0].Length;
		PlayerPrefs.SetInt("numColumnasMapa", numColumnas);
		PlayerPrefs.SetInt("numFilasMapa", numLineas);
		matrizNivel = new char[numColumnas, numLineas];
		int x = 0, y = numLineas - 1;
		foreach (char caracter in stringGen.ToCharArray()){
            if (caracter.Equals('/')){
				x = 0;
				y--;
            }else{
				if (y == numLineas-1)
                {
					if(caracter == 'X')
                    {
						matrizNivel[x, y] = 'Y';
					}
                    else
                    {
						matrizNivel[x, y] = caracter;
					}
				}
                else
                {
					if (matrizNivel[x,y+1] != 'X' && matrizNivel[x, y + 1] != 'Y' && caracter == 'X')
                    {
						matrizNivel[x, y] = 'Y';
                    }
					else
					{
						matrizNivel[x, y] = caracter;
					}
				}				
				x++;
			}
		}

		char[,] matrizNivelExtendida = new char[numColumnas+20, numLineas];
		for(int i = 0; i < numLineas; i++)
        {
			for (int j = 0; j < numColumnas; j++)
            {
				matrizNivelExtendida[j+10,i] = matrizNivel[j,i];
            }
        }

		for(int i = 0; i < numLineas; i++)
        {
			char caracterInicial = matrizNivelExtendida[10, i];
			if(caracterInicial == 'O')
            {
				caracterInicial = '_';
            }
			char caracterFinal = matrizNivelExtendida[numColumnas+20 - 11, i];
			if(caracterFinal == 'P')
            {
				caracterFinal = '_';
            }
			for(int j = 0; j < 10; j++)
            {
				matrizNivelExtendida[j, i] = caracterInicial;
				matrizNivelExtendida[numColumnas+20 - 1 - j, i] = caracterFinal;
            }
        }
		matrizNivel = matrizNivelExtendida;
	}

	void GenerarNivel(){
		int totalCaracteres = matrizNivel.Length;
		int columnas = matrizNivel.GetLength(0);
		int filas = totalCaracteres / columnas;
		for (int j = 0; j < columnas; j++)
		{
			for (int i = 0; i < filas; i++)
			{
				GenerarCasilla(j,i);
            }
        }
	}
	
	void GenerarCasilla(int x, int y){
		//Debug.Log(x + "," + y);
		char caracterPixel = matrizNivel[x, y];

		if (caracterPixel.Equals('_'))
		{
			return;
		}

		foreach (DiccionarioGeneracionPrefabs caracter in diccionarioCaracteres) {
			if (caracter.caracter.Equals(caracterPixel)){
				Vector3 posicion = new Vector3(x, y, 0);
				Instantiate(caracter.prefab, posicion, caracter.prefab.transform.rotation);
			}
		}
	}
}
