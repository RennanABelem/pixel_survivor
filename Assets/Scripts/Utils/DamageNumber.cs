// Importa a biblioteca do TextMeshPro, que é necessária para manipular componentes de texto avançados (TMP_Text).
using TMPro;
// Importa a biblioteca principal do motor da Unity.
using UnityEngine;

// Define a classe pública "DamageNumber" (Número de Dano).
// Este script será anexado a cada prefab de número de dano que aparece na tela.
public class DamageNumber : MonoBehaviour
{
    // O atributo [SerializeField] permite que esta variável privada seja visível e editável no Inspector do Unity.
    // "damageText" é a referência para o componente de texto que irá, de fato, exibir o número.
    [SerializeField] private TMP_Text damageText;

    // Uma variável privada que define a velocidade com que o número de dano flutua para cima.
    private float floatSpeed = 1.2f;

    // A função Start é chamada pela Unity uma única vez, no primeiro frame em que o objeto existe.
    void Start()
    {
        // Agenda a destruição deste GameObject ("gameObject") após 1 segundo.
        // Isso é crucial para limpar os números da tela e não sobrecarregar o jogo.
        Destroy(gameObject, 1);
    }

    // A função Update é chamada pela Unity a cada frame do jogo.
    void Update()
    {
        // A cada frame, chama a função que move o número para cima.
        AddEffectMoveUp();
    }

    // Um método privado para aplicar o efeito de movimento.
    private void AddEffectMoveUp()
    {
        // Comentário original: Adiciona um efeito de o numero ir para cima quando surge.
        // Acessa a posição atual do objeto ("transform.position") e adiciona um movimento a ela.
        // Vector3.up: É um atalho para (0, 1, 0), ou seja, a direção "para cima".
        // Time.deltaTime: É o tempo que demorou para renderizar o último frame. Multiplicar por ele
        // garante que o movimento seja suave e consistente, independentemente da taxa de frames do computador.
        // floatSpeed: É a velocidade que definimos anteriormente.
        transform.position += Vector3.up * Time.deltaTime * floatSpeed;
    }

    // Um método público que será chamado pelo "DamageNumberController" para definir qual número exibir.
    // Ele recebe um valor inteiro ("value") como parâmetro.
    public void SetValue(int value)
    {
        // Acessa a propriedade "text" do nosso componente de texto (damageText).
        // "value.ToString()" converte o número inteiro (ex: 50) para uma string de texto ("50"),
        // que é o formato necessário para ser exibido na tela.
        damageText.text = value.ToString();
    }
}