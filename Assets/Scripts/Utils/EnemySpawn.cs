using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    // [System.Serializable] permite que instâncias desta classe sejam visíveis e editáveis no Inspector da Unity.
    // Isso é extremamente útil para configurar cada onda individualmente.
    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;    // O prefab do inimigo que será gerado nesta onda.
        public float spawnTimer;          // O cronômetro interno para esta onda.
        public float spawnInterval;       // O intervalo de tempo (em segundos) entre cada inimigo.
        public int enemiesPerWave;        // Quantos inimigos devem ser gerados no total para esta onda.
        public int spawnedEnemyCount;     // Contador de quantos inimigos já foram gerados na onda atual.
    }

    [Header("Spawn Settings")]
    // Uma lista que conterá todas as ondas de inimigos configuradas no Inspector.
    [SerializeField] private List<Wave> waves = new();
    // Transforms (objetos vazios na cena) que definem a área retangular onde os inimigos podem aparecer.
    [SerializeField] private Transform minPos;
    [SerializeField] private Transform maxPos;

    // Índice para controlar qual onda da lista está ativa no momento.
    public int currentWaveIndex = 0;

    private void Update()
    {
        // Medida de segurança: se o jogador não existir ou estiver inativo (morto), para de gerar inimigos.
        if (PlayerController.instance == null || !PlayerController.instance.gameObject.activeSelf)
            return;

        // Se já passamos por todas as ondas da lista...
        if (currentWaveIndex >= waves.Count)
        {
            // ...reinicia o ciclo, voltando para a primeira onda (criando um loop infinito de ondas).
            currentWaveIndex = 0;
            return;
        }

        // Pega a onda atual da lista com base no índice.
        Wave currentWave = waves[currentWaveIndex];
        // Incrementa o cronômetro da onda atual.
        currentWave.spawnTimer += Time.deltaTime;

        // Se o tempo do cronômetro ultrapassou o intervalo definido...
        if (currentWave.spawnTimer >= currentWave.spawnInterval)
        {
            // ...reseta o cronômetro e gera um novo inimigo.
            currentWave.spawnTimer = 0;
            spawnenemy(currentWave);
        }

        // Se o número de inimigos gerados atingiu o total planejado para a onda...
        if (currentWave.spawnedEnemyCount >= currentWave.enemiesPerWave)
        {
            // ...prepara a transição para a próxima onda.
            PrepareNextWave(currentWave);
        }
    }

    // Prepara a próxima onda e aplica um aumento de dificuldade.
    private void PrepareNextWave(Wave wave)
    {
        // Reseta o contador de inimigos gerados para a próxima vez que esta onda for ativada.
        wave.spawnedEnemyCount = 0;

        // Mecanismo de dificuldade: se o intervalo de spawn for maior que 0.3s...
        if (wave.spawnInterval > 0.3f)
        {
            // ...reduz o intervalo em 10% (multiplicando por 0.9).
            // Isso fará com que os inimigos apareçam mais rápido no próximo ciclo.
            wave.spawnInterval *= 0.9f;
        }

        // Avança para a próxima onda na lista.
        currentWaveIndex++;
    }

    // Cria (instancia) um inimigo na cena.
    private void spawnenemy(Wave wave)
    {
        // Verificação para evitar erros caso o prefab do inimigo não tenha sido definido no Inspector.
        if (wave.enemyPrefab == null)
        {
            Debug.LogWarning("Enemy prefab is null in wave " + currentWaveIndex);
            return;
        }

        // Instancia o prefab do inimigo em uma posição aleatória e com a rotação padrão do spawner.
        Instantiate(wave.enemyPrefab, generateRandomSpawn(), transform.rotation);
        // Incrementa o contador de inimigos gerados para a onda atual.
        wave.spawnedEnemyCount++;
    }

    // Gera uma posição aleatória nas bordas da área definida por minPos e maxPos.
    private Vector2 generateRandomSpawn()
    {
        float x = 0, y = 0;
        // Sorteia se o inimigo vai nascer em uma borda horizontal (topo/baixo) ou vertical (esquerda/direita).
        bool horizontal = Random.Range(0f, 1f) > 0.5f;

        if (horizontal)
        {
            // Se for horizontal:
            // Sorteia uma posição X qualquer dentro dos limites.
            x = Random.Range(minPos.position.x, maxPos.position.x);
            // Sorteia se a posição Y será no topo (maxPos.y) ou na base (minPos.y).
            y = Random.Range(0f, 1f) > 0.5f ? minPos.position.y : maxPos.position.y;
        }
        else
        {
            // Se for vertical:
            // Sorteia uma posição Y qualquer dentro dos limites.
            y = Random.Range(minPos.position.y, maxPos.position.y);
            // Sorteia se a posição X será na esquerda (minPos.x) ou na direita (maxPos.x).
            x = Random.Range(0f, 1f) > 0.5f ? minPos.position.x : maxPos.position.x;
        }

        // Retorna a posição calculada.
        return new Vector2(x, y);
    }
}