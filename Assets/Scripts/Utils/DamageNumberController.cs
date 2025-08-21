// Importa a biblioteca principal do motor da Unity.
using UnityEngine;

// Define a classe p�blica "DamageNumberController" (Controlador de N�meros de Dano).
// Ela herda de "MonoBehaviour", podendo ser anexada a um GameObject na cena.
public class DamageNumberController : MonoBehaviour
{
    // "instance" � uma vari�vel est�tica. Isso cria um ponto de acesso global para este controlador.
    // Qualquer outro script no jogo pode chamar m�todos deste controlador usando "DamageNumberController.instance".
    // Este � o n�cleo do padr�o de projeto Singleton.
    public static DamageNumberController instance;

    // Uma refer�ncia p�blica para o Prefab do "DamageNumber". 
    // Este prefab � o objeto que representa visualmente o n�mero de dano que flutua na tela.
    public DamageNumber prefab;

    // A fun��o Awake � chamada pela Unity quando o script � carregado, antes mesmo da fun��o Start.
    // � o local ideal para configurar o padr�o Singleton.
    private void Awake()
    {
        // Verifica se a vari�vel est�tica "instance" ainda n�o foi definida (ou seja, � nula).
        if (instance == null)
        {
            // Se for nula, "this" (esta inst�ncia espec�fica do script) se torna a inst�ncia global.
            instance = this;
        }
        // Caso a vari�vel "instance" j� exista...
        else
        {
            // ...significa que j� h� um DamageNumberController na cena. Para evitar duplicatas,
            // este novo objeto � destru�do, garantindo que sempre haja apenas uma inst�ncia.
            Destroy(gameObject);
        }
    }

    // M�todo p�blico que pode ser chamado por outros scripts para criar um n�mero de dano na tela.
    // Ele recebe dois par�metros: "value" (o valor do dano) e "location" (onde o n�mero deve aparecer).
    public void CreateNumber(float value, Vector2 location)
    {
        // "Instantiate" cria uma nova c�pia (inst�ncia) do prefab.
        // 1. prefab: O que ser� criado.
        // 2. location: A posi��o no mundo onde ser� criado.
        // 3. transform.rotation: A rota��o que ser� usada (neste caso, a mesma do pr�prio controlador).
        // 4. transform: Define este objeto (o controlador) como o "pai" do novo n�mero de dano na Hierarquia.
        DamageNumber damageNumer = Instantiate(prefab, location, transform.rotation, transform);

        // Depois de criar o n�mero, este c�digo chama o m�todo "SetValue" no script do pr�prio n�mero de dano.
        // "Mathf.RoundToInt(value)" arredonda o valor do dano (que � um float) para o n�mero inteiro mais pr�ximo,
        // garantindo que o texto exibido seja um n�mero inteiro (ex: 10 em vez de 10.2).
        damageNumer.SetValue(Mathf.RoundToInt(value));
    }
}