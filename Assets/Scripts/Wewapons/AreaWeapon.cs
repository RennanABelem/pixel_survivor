using UnityEngine;

// A classe AreaWeapon herda da classe Weapon.
// Isso significa que ela possui todos os campos e métodos de Weapon,
// além dos que são definidos aqui. Ela é uma especialização de uma arma.
public class AreaWeapon : Weapon
{

    // Organiza os campos no Inspector da Unity.
    [Header("Components")]

    // [SerializeField] permite que o campo privado 'prefab' seja visível e atribuído no Inspector da Unity.
    // 'prefab' é o GameObject que será criado repetidamente (ex: a aura de dano, o escudo, etc.).
    [SerializeField] private GameObject prefab;

    // Um cronômetro privado para controlar o intervalo entre a criação de cada 'prefab'.
    private float spawnCouter;

    // O método Update é chamado uma vez por frame.
    void Update()
    {
        // A cada frame, subtrai o tempo que passou desde o último frame do contador.
        // Isso efetivamente cria uma contagem regressiva.
        spawnCouter -= Time.deltaTime;

        // Verifica se o cronômetro chegou a zero (ou menos).
        if (spawnCouter < 0)
        {
            // Quando o tempo acaba, o cronômetro é reiniciado.
            // O valor do cooldown é pego de uma estrutura de dados 'stats', provavelmente definida na classe pai 'Weapon'.
            // O cooldown específico depende do 'weaponLevel' atual, permitindo que a arma ataque mais rápido em níveis mais altos.
            spawnCouter = stats[weaponLevel].cooldown;

            // Esta é a linha principal: ela cria uma nova instância do 'prefab'.
            // Parâmetros do Instantiate:
            // 1. prefab: O que criar.
            // 2. transform.position: Onde criar (na mesma posição do objeto que tem este script).
            // 3. transform.rotation: Com qual rotação criar (a mesma do objeto pai).
            // 4. transform: Define o 'parent' (pai) do novo objeto. Ao fazer isso, o objeto criado se moverá junto com o jogador.
            Instantiate(prefab, transform.position, transform.rotation, transform);
        }
    }
}
