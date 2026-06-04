using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaLucesGuia : MonoBehaviour
{
    [Header("Grupos de luces")]
    public GrupoLuces grupo1;
    public GrupoLuces grupo2;
    public GrupoLuces grupo3;

    void Start()
    {
        // El primer grupo arranca apenas comienza la etapa
        IniciarGrupo1();
    }

    public void IniciarGrupo1()
    {
        if (grupo1 != null) grupo1.IniciarSecuencia();
    }

    public void IniciarGrupo2()
    {
        if (grupo2 != null) grupo2.IniciarSecuencia();
    }

    public void IniciarGrupo3()
    {
        if (grupo3 != null) grupo3.IniciarSecuencia();
    }
}