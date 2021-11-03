using System;

public class NivelDatos
{
    private string idNivel;
    private string nombreNivel;
    private string creadorNivel;
    private string likesNivel;
    private string stringGenNivel;
    private string guardado;
    private string meGusta;

    public NivelDatos(string pIdNivel, string pNombreNivel, string pCreadorNivel, string pLikesNivel, string pStringGenNivel, string pMeGusta, string pGuardado)
    {
        idNivel = pIdNivel;
        nombreNivel = pNombreNivel;
        creadorNivel = pCreadorNivel;
        likesNivel = pLikesNivel;
        stringGenNivel = pStringGenNivel;
        meGusta = pMeGusta;
        guardado = pGuardado;
    }

    public string getId()
    {
        return this.idNivel;
    }
    public string getNombre()
    {
        return this.nombreNivel;
    }

    public string getCreador()
    {
        return this.creadorNivel;
    }

    public string getStrGen()
    {
        return this.stringGenNivel;
    }

    public string getLikes()
    {
        return this.likesNivel;
    }

    public string tieneMeGusta()
    {
        return this.meGusta;
    }

    public void ponerMeGusta()
    {
        this.meGusta = "1";
    }

    public void quitarMeGusta()
    {
        this.meGusta = "0";
    }

    public string esGuardado()
    {
        return this.guardado;
    }

    public void ponerGuardado()
    {
        this.guardado = "1";
    }

    public void quitarGuardado()
    {
        this.guardado = "0";
    }

    public void aumentarLikes()
    {
        this.likesNivel = (int.Parse(likesNivel) + 1).ToString();
    }

    public void reducirLikes()
    {
        this.likesNivel = (int.Parse(likesNivel) - 1).ToString();
    }
}
