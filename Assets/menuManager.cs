using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuNavegacao : MonoBehaviour
{
    public void IrParaNovoJogo()
    {
        SceneManager.LoadScene("novoJogo");
        
        Debug.Log("Carregando NOVO JOGO...");
    }
    public void IrParaContinuar()
    {
        SceneManager.LoadScene("continuar");
        
        Debug.Log("Carregando jogo SALVO...");
        
    }
    
    public void IrParaOpcoes()
    {
        SceneManager.LoadScene("opcoes");
        
        Debug.Log("Abrindo CONFIGURAÇÕES...");
    }
    
    public void IrParaHistoriaPersonagens()
    {
        SceneManager.LoadScene("historiaPersonagens");
        
        Debug.Log("Mostrando HISTÓRIA e PERSONAGENS...");
    }

    public void Voltar()
    {
        SceneManager.LoadScene("telaInicial");
        
        Debug.Log("Voltando");
    }
    
    public void IrParaInventario()
    {
        SceneManager.LoadScene("inventario");
        
        Debug.Log("Mostrando INVENTARIO");
    }
    public void VoltarFase()
    {
        // TODO        
        Debug.Log("Voltando pra fase");
    }

    public void IrParaMapa()
    {
        SceneManager.LoadScene("mapaJogo");

        Debug.Log("Mostrando MAPA");
    }

    public void JogarFase1()
    {
        SceneManager.LoadScene("fase1Jogo");

        Debug.Log("Mostrando tela inicial fase1");
    }
    public void JogarFase2()
    {
        SceneManager.LoadScene("fase2Jogo");

        Debug.Log("Mostrando tela inicial fase2");
    }
     public void JogarFase3()
    {
        SceneManager.LoadScene("fase3Jogo");

        Debug.Log("Mostrando tela inicial fase3");
    }
    public void JogarFase4()
    {
        SceneManager.LoadScene("fase4Jogo");

        Debug.Log("Mostrando tela inicial fase4");
    }
}