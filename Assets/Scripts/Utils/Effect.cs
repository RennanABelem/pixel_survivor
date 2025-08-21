// Importa a biblioteca principal do motor da Unity, que � fundamental para qualquer script.
using UnityEngine;

// Define a classe p�blica "Effect" (Efeito).
// Este script � projetado para ser anexado a GameObjects de efeitos visuais (como explos�es, fuma�a, etc.)
// que possuem uma anima��o e devem se autodestruir ap�s a anima��o terminar.
public class Effect : MonoBehaviour
{
    // O atributo [SerializeField] torna a vari�vel privada "animator" vis�vel no Inspector do Unity,
    // permitindo que voc� arraste e solte o componente Animator do GameObject neste campo.
    // Esta � uma refer�ncia ao componente que controla a anima��o do efeito.
    [SerializeField] private Animator animator;

    // A fun��o Start � chamada pela Unity uma vez, no primeiro frame em que o objeto � ativado.
    void Start()
    {
        // O objetivo deste script � garantir que o efeito visual seja removido da cena
        // exatamente quando sua anima��o terminar, para n�o consumir recursos desnecessariamente.

        // --- EXPLICA��O DO C�DIGO ---

        // A fun��o "Destroy" agenda a destrui��o de um GameObject.
        // O segundo par�metro � o tempo de espera em segundos antes da destrui��o.
        // Destroy(gameObject, tempo_em_segundos);

        // A linha abaixo busca a dura��o da anima��o que est� tocando no Animator. Vamos analis�-la em partes:
        // 1. animator.GetCurrentAnimatorClipInfo(0): Pega as informa��es sobre o(s) clipe(s) de anima��o
        //    que est�o atualmente tocando na primeira camada (layer 0) do Animator. Isso retorna uma lista.
        // 2. [0]: Acessa o primeiro item dessa lista (assumindo que apenas uma anima��o est� tocando).
        // 3. .clip: Acessa o "Animation Clip" em si, que cont�m os dados da anima��o.
        // 4. .length: Finalmente, pega a dura��o total do clipe em segundos.
        Destroy(gameObject, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }
}