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

    public void VoltarTelaPause()
    {
        SceneManager.LoadScene("telaPause");
        
        Debug.Log("Voltando");
    }
    
    public void IrParaMapa()
    {
        SceneManager.LoadScene("mapaJogo");

        Debug.Log("Mostrando MAPA");
    }

    public void irFase1()
    {
        SceneManager.LoadScene("fase1TelaInicial");

        Debug.Log("Mostrando tela inicial fase1");
    }
    public void irFase2()
    {
        SceneManager.LoadScene("fase2TelaInicial");

        Debug.Log("Mostrando tela inicial fase2");
    }
     public void irFase3()
    {
        SceneManager.LoadScene("fase3TelaInicial");

        Debug.Log("Mostrando tela inicial fase3");
    }
    public void irFase4()
    {
        SceneManager.LoadScene("fase4TelaInicial");

        Debug.Log("Mostrando tela inicial fase4");
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
    //CENAS PARA CONTAR HISÓRIA EM "NOVO JOGO"
    public void CarregarHistoriapt2()
    {
        SceneManager.LoadScene("novoJogoHistoria2");

        Debug.Log("Mostrando tela historiapt2 novo jogo");
    }
    public void CarregarHistoriapt3()
    {
        SceneManager.LoadScene("novoJogoHistoria3");

        Debug.Log("Mostrando tela historiapt3 novo jogo");
    }
    public void CarregarHistoriapt4()
    {
        SceneManager.LoadScene("novoJogoHistoria4");

        Debug.Log("Mostrando tela historiapt4 novo jogo");
    }
    public void CarregarHistoriapt5()
    {
        SceneManager.LoadScene("novoJogoHistoria5");

        Debug.Log("Mostrando tela historiapt5 novo jogo");
    }
     //CENAS PARA CONTAR HISÓRIA EM "HISTORIA PERSONAGENS"
    public void CarregarHistoriaLobo()
    {
        SceneManager.LoadScene("historiaPersonagensLobo");

        Debug.Log("Mostrando tela historiaPersonagem lobo");
    }
    public void CarregarHistoriaMoradorFerido()
    {
        SceneManager.LoadScene("historiaPersonagensMoradorFerido");

        Debug.Log("Mostrando tela historiaPersonagem Morador Ferido");
    }
    public void CarregarHistoriaBruxa()
    {
        SceneManager.LoadScene("historiaPersonagensBruxa");

        Debug.Log("Mostrando tela historiaPersonagem bruxa");
    }
    public void CarregarHistoriaGuerreiroFerido()
    {
        SceneManager.LoadScene("historiaPersonagensGuerreiroFerido");

        Debug.Log("Mostrando tela historiaPersonagem Guerreiro Ferido");
    }
    public void CarregarHistoriaGnomo()
    {
        SceneManager.LoadScene("historiaPersonagensGnomo");

        Debug.Log("Mostrando tela historiaPersonagem Gnomo");
    }
    public void CarregarHistoriaMago()
    {
        SceneManager.LoadScene("historiaPersonagensMago");

        Debug.Log("Mostrando tela historiaPersonagem mago");
    }
    public void CarregarHistoriaAranhaBau()
    {
        SceneManager.LoadScene("historiaPersonagensAranhaBau");

        Debug.Log("Mostrando tela historiaPersonagem aranha bau");
    }
}