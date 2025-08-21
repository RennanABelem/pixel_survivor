// Importa a biblioteca principal do motor da Unity, que é fundamental para qualquer script.
using UnityEngine;

// Define a classe pública "Effect" (Efeito).
// Este script é projetado para ser anexado a GameObjects de efeitos visuais (como explosões, fumaça, etc.)
// que possuem uma animação e devem se autodestruir após a animação terminar.
public class Effect : MonoBehaviour
{
    // O atributo [SerializeField] torna a variável privada "animator" visível no Inspector do Unity,
    // permitindo que você arraste e solte o componente Animator do GameObject neste campo.
    // Esta é uma referência ao componente que controla a animação do efeito.
    [SerializeField] private Animator animator;

    // A função Start é chamada pela Unity uma vez, no primeiro frame em que o objeto é ativado.
    void Start()
    {
        // O objetivo deste script é garantir que o efeito visual seja removido da cena
        // exatamente quando sua animação terminar, para não consumir recursos desnecessariamente.

        // --- EXPLICAÇÃO DO CÓDIGO ---

        // A função "Destroy" agenda a destruição de um GameObject.
        // O segundo parâmetro é o tempo de espera em segundos antes da destruição.
        // Destroy(gameObject, tempo_em_segundos);

        // A linha abaixo busca a duração da animação que está tocando no Animator. Vamos analisá-la em partes:
        // 1. animator.GetCurrentAnimatorClipInfo(0): Pega as informações sobre o(s) clipe(s) de animação
        //    que estão atualmente tocando na primeira camada (layer 0) do Animator. Isso retorna uma lista.
        // 2. [0]: Acessa o primeiro item dessa lista (assumindo que apenas uma animação está tocando).
        // 3. .clip: Acessa o "Animation Clip" em si, que contém os dados da animação.
        // 4. .length: Finalmente, pega a duração total do clipe em segundos.
        Destroy(gameObject, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }
}