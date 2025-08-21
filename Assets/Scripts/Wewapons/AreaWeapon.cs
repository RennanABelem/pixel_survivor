using UnityEngine;

// A classe AreaWeapon herda da classe Weapon.
// Isso significa que ela possui todos os campos e m�todos de Weapon,
// al�m dos que s�o definidos aqui. Ela � uma especializa��o de uma arma.
public class AreaWeapon : Weapon
{

    // Organiza os campos no Inspector da Unity.
    [Header("Components")]

    // [SerializeField] permite que o campo privado 'prefab' seja vis�vel e atribu�do no Inspector da Unity.
    // 'prefab' � o GameObject que ser� criado repetidamente (ex: a aura de dano, o escudo, etc.).
    [SerializeField] private GameObject prefab;

    // Um cron�metro privado para controlar o intervalo entre a cria��o de cada 'prefab'.
    private float spawnCouter;

    // O m�todo Update � chamado uma vez por frame.
    void Update()
    {
        // A cada frame, subtrai o tempo que passou desde o �ltimo frame do contador.
        // Isso efetivamente cria uma contagem regressiva.
        spawnCouter -= Time.deltaTime;

        // Verifica se o cron�metro chegou a zero (ou menos).
        if (spawnCouter < 0)
        {
            // Quando o tempo acaba, o cron�metro � reiniciado.
            // O valor do cooldown � pego de uma estrutura de dados 'stats', provavelmente definida na classe pai 'Weapon'.
            // O cooldown espec�fico depende do 'weaponLevel' atual, permitindo que a arma ataque mais r�pido em n�veis mais altos.
            spawnCouter = stats[weaponLevel].cooldown;

            // Esta � a linha principal: ela cria uma nova inst�ncia do 'prefab'.
            // Par�metros do Instantiate:
            // 1. prefab: O que criar.
            // 2. transform.position: Onde criar (na mesma posi��o do objeto que tem este script).
            // 3. transform.rotation: Com qual rota��o criar (a mesma do objeto pai).
            // 4. transform: Define o 'parent' (pai) do novo objeto. Ao fazer isso, o objeto criado se mover� junto com o jogador.
            Instantiate(prefab, transform.position, transform.rotation, transform);
        }
    }
}
