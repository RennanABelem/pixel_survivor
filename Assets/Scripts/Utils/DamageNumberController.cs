// Importa a biblioteca principal do motor da Unity.
using UnityEngine;

// Define a classe pública "DamageNumberController" (Controlador de Números de Dano).
// Ela herda de "MonoBehaviour", podendo ser anexada a um GameObject na cena.
public class DamageNumberController : MonoBehaviour
{
    // "instance" é uma variável estática. Isso cria um ponto de acesso global para este controlador.
    // Qualquer outro script no jogo pode chamar métodos deste controlador usando "DamageNumberController.instance".
    // Este é o núcleo do padrão de projeto Singleton.
    public static DamageNumberController instance;

    // Uma referência pública para o Prefab do "DamageNumber". 
    // Este prefab é o objeto que representa visualmente o número de dano que flutua na tela.
    public DamageNumber prefab;

    // A função Awake é chamada pela Unity quando o script é carregado, antes mesmo da função Start.
    // É o local ideal para configurar o padrão Singleton.
    private void Awake()
    {
        // Verifica se a variável estática "instance" ainda não foi definida (ou seja, é nula).
        if (instance == null)
        {
            // Se for nula, "this" (esta instância específica do script) se torna a instância global.
            instance = this;
        }
        // Caso a variável "instance" já exista...
        else
        {
            // ...significa que já há um DamageNumberController na cena. Para evitar duplicatas,
            // este novo objeto é destruído, garantindo que sempre haja apenas uma instância.
            Destroy(gameObject);
        }
    }

    // Método público que pode ser chamado por outros scripts para criar um número de dano na tela.
    // Ele recebe dois parâmetros: "value" (o valor do dano) e "location" (onde o número deve aparecer).
    public void CreateNumber(float value, Vector2 location)
    {
        // "Instantiate" cria uma nova cópia (instância) do prefab.
        // 1. prefab: O que será criado.
        // 2. location: A posição no mundo onde será criado.
        // 3. transform.rotation: A rotação que será usada (neste caso, a mesma do próprio controlador).
        // 4. transform: Define este objeto (o controlador) como o "pai" do novo número de dano na Hierarquia.
        DamageNumber damageNumer = Instantiate(prefab, location, transform.rotation, transform);

        // Depois de criar o número, este código chama o método "SetValue" no script do próprio número de dano.
        // "Mathf.RoundToInt(value)" arredonda o valor do dano (que é um float) para o número inteiro mais próximo,
        // garantindo que o texto exibido seja um número inteiro (ex: 10 em vez de 10.2).
        damageNumer.SetValue(Mathf.RoundToInt(value));
    }
}