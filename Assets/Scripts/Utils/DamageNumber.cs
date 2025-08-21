// Importa a biblioteca do TextMeshPro, que � necess�ria para manipular componentes de texto avan�ados (TMP_Text).
using TMPro;
// Importa a biblioteca principal do motor da Unity.
using UnityEngine;

// Define a classe p�blica "DamageNumber" (N�mero de Dano).
// Este script ser� anexado a cada prefab de n�mero de dano que aparece na tela.
public class DamageNumber : MonoBehaviour
{
    // O atributo [SerializeField] permite que esta vari�vel privada seja vis�vel e edit�vel no Inspector do Unity.
    // "damageText" � a refer�ncia para o componente de texto que ir�, de fato, exibir o n�mero.
    [SerializeField] private TMP_Text damageText;

    // Uma vari�vel privada que define a velocidade com que o n�mero de dano flutua para cima.
    private float floatSpeed = 1.2f;

    // A fun��o Start � chamada pela Unity uma �nica vez, no primeiro frame em que o objeto existe.
    void Start()
    {
        // Agenda a destrui��o deste GameObject ("gameObject") ap�s 1 segundo.
        // Isso � crucial para limpar os n�meros da tela e n�o sobrecarregar o jogo.
        Destroy(gameObject, 1);
    }

    // A fun��o Update � chamada pela Unity a cada frame do jogo.
    void Update()
    {
        // A cada frame, chama a fun��o que move o n�mero para cima.
        AddEffectMoveUp();
    }

    // Um m�todo privado para aplicar o efeito de movimento.
    private void AddEffectMoveUp()
    {
        // Coment�rio original: Adiciona um efeito de o numero ir para cima quando surge.
        // Acessa a posi��o atual do objeto ("transform.position") e adiciona um movimento a ela.
        // Vector3.up: � um atalho para (0, 1, 0), ou seja, a dire��o "para cima".
        // Time.deltaTime: � o tempo que demorou para renderizar o �ltimo frame. Multiplicar por ele
        // garante que o movimento seja suave e consistente, independentemente da taxa de frames do computador.
        // floatSpeed: � a velocidade que definimos anteriormente.
        transform.position += Vector3.up * Time.deltaTime * floatSpeed;
    }

    // Um m�todo p�blico que ser� chamado pelo "DamageNumberController" para definir qual n�mero exibir.
    // Ele recebe um valor inteiro ("value") como par�metro.
    public void SetValue(int value)
    {
        // Acessa a propriedade "text" do nosso componente de texto (damageText).
        // "value.ToString()" converte o n�mero inteiro (ex: 50) para uma string de texto ("50"),
        // que � o formato necess�rio para ser exibido na tela.
        damageText.text = value.ToString();
    }
}