import cv2
import imutils
import numpy as np
import random
import sys
import math


def generar(rutaFoto, enemigos, monedas):
    deslizadorEnemigos = enemigos
    deslizadorMonedas = monedas
    img = cv2.imread(rutaFoto)
    img = imutils.resize(img, width=1280)

    multiplicadorParaDimensionesDeseadas = 150/img.shape[1] # percent of original size
    alturaFoto, anchoFoto, channels = img.shape

    #PASAR LA IMAGEN A BLANCO Y NEGRO, DIFUMINARLA Y DETECTAR BORDES EN LA FOTO
    gris = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    grisBlur = cv2.GaussianBlur(gris, (5, 5), 0)
    canny = cv2.Canny(grisBlur, 10, 100)


    #OBTENER LAS LINEAS DE LA IMAGEN
    lineas = cv2.HoughLinesP(canny, 1, 2*np.pi/180, 5, np.array([]), 5, 10)

    #CREAR LA MATRIZ DE LAS DIMENSIONES DESEADAS
    numLineas = 0
    matriz = np.zeros((round(alturaFoto*multiplicadorParaDimensionesDeseadas), round(anchoFoto*multiplicadorParaDimensionesDeseadas)))

    #DIBUJAR LAS LINEAS EN LA MATRIZ
    for linea in lineas:
        numLineas = numLineas + 1
        for x1, y1, x2, y2 in linea:
            matriz = draw_line(matriz, math.floor(y1*multiplicadorParaDimensionesDeseadas),math.floor(x1*multiplicadorParaDimensionesDeseadas),math.floor(y2*multiplicadorParaDimensionesDeseadas),math.floor(x2*multiplicadorParaDimensionesDeseadas))

    #PASAR LA MATRIZ DE NUMEROS A CARACTERES
    matrizChar = [[' ' for columna in range(round(anchoFoto*multiplicadorParaDimensionesDeseadas))] for fila in range(round(alturaFoto*multiplicadorParaDimensionesDeseadas))]
    for i in range(matriz.__len__()):
        for j in range(matriz[0].size):
            if round(matriz[i][j]) == 0:
                matrizChar[i][j] = '_'
            else:
                matrizChar[i][j] = 'X'

    filaVacia = ['_'] * len(matrizChar[0])

    matrizChar.insert(0,filaVacia)

    #PONER 'O'(LUGAR DE INICIO) EN LA MATRIZ
    buclesFin = False
    for j in range(len(matrizChar[0])):
        for i in range(len(matrizChar)):
            if matrizChar[i][j] == 'X':
                matrizChar[i-1][j] = 'O'

                buclesFin = True
            if buclesFin:
                columnaInicial = j
                break
        if buclesFin:
            break

    # PONER 'P'(LUGAR DE FIN) EN LA MATRIZ
    buclesFin = False
    for j in reversed(range(len(matrizChar[0]))):
        for i in range(len(matrizChar)):
            if matrizChar[i][j] == 'X':
                matrizChar[i - 1][j] = 'P'
                buclesFin = True
            if buclesFin:
                columnaFinal = j
                break
        if buclesFin:
            break


    #AJUSTAR MATRIZ PARA QUE NO HAYA COLUMNAS VACIAS POR LA IZQUIERDA Y LA DERECHA
    for fila in matrizChar:
        for i in range((round(anchoFoto*multiplicadorParaDimensionesDeseadas) - 1) - columnaFinal):
            fila.pop()
        for i in range(columnaInicial):
            fila.pop(0)



    # RELLENAR VACIO DEBAJO DE LA LINEA
    matrizChar = rellenarCasillas(matrizChar, len(matrizChar)-1, len(matrizChar[0])-1)


    #PONER ENEMIGOS Y MONEDAS
    matrizChar = ponerEnemigosMonedas(matrizChar, deslizadorEnemigos, deslizadorMonedas)


    nivelString = ""
    for linea in range(len(matrizChar)):
        for numero in range(len(matrizChar[0])):
            nivelString = nivelString + matrizChar[linea][numero]
        nivelString = nivelString + "/"

    #filas = nivelString.split('/')
    #for fila in filas:
    #    print(fila)

    print(nivelString)
    sys.stdout.flush()

def ponerEnemigosMonedas(matriz, numEnemigosDeslizador, numMonedasDeslizador):
    alturaActual = 0
    plataformaInvalida = False
    plataformaActual = []
    for j in range(len(matriz[0])):
        for i in range(len(matriz)):
            if matriz[i][j] == 'O' or matriz[i][j] == 'P':
                plataformaInvalida = True
                plataformaActual.append([i + 1, j])
                alturaActual = i + 1
                break
            elif matriz[i][j] == 'X':
                if i == alturaActual:
                    plataformaActual.append([i, j])
                else:
                    if not plataformaInvalida:
                        longitudPlataforma = len(plataformaActual)

                        ### ENEMIGOS ###
                        if(numEnemigosDeslizador != 0):
                            numMaxEnemigosEnPlataforma = round(longitudPlataforma / 3)
                            porcentajePonerEnemigo = (numEnemigosDeslizador * 100 / 10) - 1
                            numEnemigosEnPlataformaActual = 0
                            sePuedePoner = True
                            quitarBloqueoPoner = False
                            for casX, casY in plataformaActual:
                                if not sePuedePoner:
                                    quitarBloqueoPoner = True
                                numAleatorio = random.randint(0, 100)
                                if numEnemigosEnPlataformaActual < numMaxEnemigosEnPlataforma and numAleatorio < porcentajePonerEnemigo and sePuedePoner:
                                    matriz[casX - 1][casY] = 'E'
                                    numEnemigosEnPlataformaActual = numEnemigosEnPlataformaActual + 1
                                    sePuedePoner = False
                                if quitarBloqueoPoner:
                                    sePuedePoner = True
                                    quitarBloqueoPoner = False


                        ### MONEDAS ###
                        if (numMonedasDeslizador != 0 and longitudPlataforma > 4 and plataformaActual[0][1] > 2):
                            huecoVacio = True
                            for casX, casY in plataformaActual:
                                if matriz[casX-2][casY] != '_' or matriz[casX-3][casY] != '_':
                                    huecoVacio = False

                            if huecoVacio:
                                longitudMonedas = longitudPlataforma - 2
                                matrizMonedas = [['M']*longitudMonedas]
                                matrizMonedas.append(['M']*longitudMonedas)

                                # XXXXX
                                # _____
                                probabilidad = 100 - (15 * numMonedasDeslizador)
                                numAleatorio = random.randint(0, 100)
                                if numAleatorio < probabilidad / 2:
                                    for i in range(len(matrizMonedas[0])):
                                        matrizMonedas[0][i] = '_'
                                elif numAleatorio < probabilidad:
                                    for i in range(len(matrizMonedas[0])):
                                        matrizMonedas[1][i] = '_'


                                # _XXX_
                                # X___X
                                probabilidad = 100 - (15 * numMonedasDeslizador)
                                numAleatorio = random.randint(0, 100)
                                if numAleatorio < probabilidad / 2:
                                    for x in range(len(matrizMonedas)):
                                        for y in range(len(matrizMonedas[0])):
                                            if y == 0 or y == len(matrizMonedas[0]) - 1:
                                                matrizMonedas[0][y] = '_'
                                            else:
                                                matrizMonedas[1][y] = '_'
                                elif numAleatorio < probabilidad:
                                    for x in range(len(matrizMonedas)):
                                        for y in range(len(matrizMonedas[0])):
                                            if y == 0 or y == len(matrizMonedas[0]) - 1:
                                                matrizMonedas[1][y] = '_'
                                            else:
                                                matrizMonedas[0][y] = '_'


                                # XX_XX
                                # XX_XX
                                mitad = []
                                mitad.append(math.ceil(longitudMonedas / 2) - 1)
                                if longitudMonedas % 2 == 0:
                                    mitad.append(math.ceil(longitudMonedas / 2))
                                if longitudMonedas > 6:
                                    mitad.append(mitad[0] - 1)
                                    if longitudMonedas % 2 == 0:
                                        mitad.append(mitad[1] + 1)
                                    else:
                                        mitad.append(mitad[0] + 1)
                                probabilidad = 100 - (15 * numMonedasDeslizador)
                                numAleatorio = random.randint(0, 100)
                                if (numAleatorio < probabilidad):
                                    for num in mitad:
                                        matrizMonedas[0][num] = '_'
                                        matrizMonedas[1][num] = '_'


                                # _XXX_
                                # _XXX_
                                probabilidad = 100 - (15 * numMonedasDeslizador)
                                numAleatorio = random.randint(0, 100)
                                if (numAleatorio < probabilidad):
                                    matrizMonedas[0][0] = '_'
                                    matrizMonedas[1][0] = '_'
                                    matrizMonedas[0][len(matrizMonedas[0]) - 1] = '_'
                                    matrizMonedas[1][len(matrizMonedas[1]) - 1] = '_'


                                xIni = plataformaActual[1][0] - 2
                                yIni = plataformaActual[1][1]
                                for x in range(2):
                                    for y in range(longitudMonedas):
                                        matriz[xIni - x][yIni + y] = matrizMonedas[x][y]

                    plataformaActual = []
                    alturaActual = i
                    plataformaActual.append([i, j])
                    plataformaInvalida = False
                break
    return matriz



def rellenarCasillas(array, xIni, yIni):
    listaPorMirar = []
    listaPorMirar.append([xIni,yIni])

    while len(listaPorMirar) > 0:
        actual = listaPorMirar.pop(0)
        x, y = actual
        if array[x][y] == '_':
            array[x][y] = "X"
            if x < len(array)-1:
                if array[x + 1][y] == '_':
                    listaPorMirar.append([x+1,y])

            if x > 0:
                if array[x - 1][y] == '_':
                    listaPorMirar.append([x-1,y])

            if y < len(array[0])-1:
                if array[x][y + 1] == '_':
                    listaPorMirar.append([x,y+1])

            if y > 0:
                if array[x][y - 1] == '_':
                    listaPorMirar.append([x,y-1])
    return array




################################################################################################################################
#                                                                                                                              #
#   https://stackoverflow.com/questions/50387606/python-draw-line-between-two-coordinates-in-a-matrix - Answered by: jdehesa   #
#                                                                                                                              #
################################################################################################################################
def draw_line(mat, x0, y0, x1, y1, inplace=False):
    if not (0 <= x0 < mat.shape[0] and 0 <= x1 < mat.shape[0] and
            0 <= y0 < mat.shape[1] and 0 <= y1 < mat.shape[1]):
        raise ValueError('Invalid coordinates.')
    if not inplace:
        mat = mat.copy()
    if (x0, y0) == (x1, y1):
        mat[x0, y0] = 2
        return mat if not inplace else None
    # Swap axes if Y slope is smaller than X slope
    transpose = abs(x1 - x0) < abs(y1 - y0)
    if transpose:
        mat = mat.transpose()
        x0, y0, x1, y1 = y0, x0, y1, x1
    # Swap line direction to go left-to-right if necessary
    if x0 > x1:
        x0, y0, x1, y1 = x1, y1, x0, y0
    # Write line ends
    mat[x0, y0] = 1
    mat[x1, y1] = 1
    # Compute intermediate coordinates using line equation
    x = np.arange(x0 + 1, x1)
    y = np.round(((y1 - y0) / (x1 - x0)) * (x - x0) + y0).astype(x.dtype)
    # Write intermediate coordinates
    mat[x, y] = 1
    #return mat
    if not inplace:
        return mat if not transpose else mat.T


if __name__ == '__main__':
    enemigos = int(sys.argv[1])
    monedas = int(sys.argv[2])
    rutaFoto = "/root/TFG/foto.png"
    generar(rutaFoto, enemigos, monedas)
