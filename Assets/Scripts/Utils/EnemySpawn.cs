using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    // [System.Serializable] permite que inst�ncias desta classe sejam vis�veis e edit�veis no Inspector da Unity.
    // Isso � extremamente �til para configurar cada onda individualmente.
    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;    // O prefab do inimigo que ser� gerado nesta onda.
        public float spawnTimer;          // O cron�metro interno para esta onda.
        public float spawnInterval;       // O intervalo de tempo (em segundos) entre cada inimigo.
        public int enemiesPerWave;        // Quantos inimigos devem ser gerados no total para esta onda.
        public int spawnedEnemyCount;     // Contador de quantos inimigos j� foram gerados na onda atual.
    }

    [Header("Spawn Settings")]
    // Uma lista que conter� todas as ondas de inimigos configuradas no Inspector.
    [SerializeField] private List<Wave> waves = new();
    // Transforms (objetos vazios na cena) que definem a �rea retangular onde os inimigos podem aparecer.
    [SerializeField] private Transform minPos;
    [SerializeField] private Transform maxPos;

    // �ndice para controlar qual onda da lista est� ativa no momento.
    public int currentWaveIndex = 0;

    private void Update()
    {
        // Medida de seguran�a: se o jogador n�o existir ou estiver inativo (morto), para de gerar inimigos.
        if (PlayerController.instance == null || !PlayerController.instance.gameObject.activeSelf)
            return;

        // Se j� passamos por todas as ondas da lista...
        if (currentWaveIndex >= waves.Count)
        {
            // ...reinicia o ciclo, voltando para a primeira onda (criando um loop infinito de ondas).
            currentWaveIndex = 0;
            return;
        }

        // Pega a onda atual da lista com base no �ndice.
        Wave currentWave = waves[currentWaveIndex];
        // Incrementa o cron�metro da onda atual.
        currentWave.spawnTimer += Time.deltaTime;

        // Se o tempo do cron�metro ultrapassou o intervalo definido...
        if (currentWave.spawnTimer >= currentWave.spawnInterval)
        {
            // ...reseta o cron�metro e gera um novo inimigo.
            currentWave.spawnTimer = 0;
            spawnenemy(currentWave);
        }

        // Se o n�mero de inimigos gerados atingiu o total planejado para a onda...
        if (currentWave.spawnedEnemyCount >= currentWave.enemiesPerWave)
        {
            // ...prepara a transi��o para a pr�xima onda.
            PrepareNextWave(currentWave);
        }
    }

    // Prepara a pr�xima onda e aplica um aumento de dificuldade.
    private void PrepareNextWave(Wave wave)
    {
        // Reseta o contador de inimigos gerados para a pr�xima vez que esta onda for ativada.
        wave.spawnedEnemyCount = 0;

        // Mecanismo de dificuldade: se o intervalo de spawn for maior que 0.3s...
        if (wave.spawnInterval > 0.3f)
        {
            // ...reduz o intervalo em 10% (multiplicando por 0.9).
            // Isso far� com que os inimigos apare�am mais r�pido no pr�ximo ciclo.
            wave.spawnInterval *= 0.9f;
        }

        // Avan�a para a pr�xima onda na lista.
        currentWaveIndex++;
    }

    // Cria (instancia) um inimigo na cena.
    private void spawnenemy(Wave wave)
    {
        // Verifica��o para evitar erros caso o prefab do inimigo n�o tenha sido definido no Inspector.
        if (wave.enemyPrefab == null)
        {
            Debug.LogWarning("Enemy prefab is null in wave " + currentWaveIndex);
            return;
        }

        // Instancia o prefab do inimigo em uma posi��o aleat�ria e com a rota��o padr�o do spawner.
        Instantiate(wave.enemyPrefab, generateRandomSpawn(), transform.rotation);
        // Incrementa o contador de inimigos gerados para a onda atual.
        wave.spawnedEnemyCount++;
    }

    // Gera uma posi��o aleat�ria nas bordas da �rea definida por minPos e maxPos.
    private Vector2 generateRandomSpawn()
    {
        float x = 0, y = 0;
        // Sorteia se o inimigo vai nascer em uma borda horizontal (topo/baixo) ou vertical (esquerda/direita).
        bool horizontal = Random.Range(0f, 1f) > 0.5f;

        if (horizontal)
        {
            // Se for horizontal:
            // Sorteia uma posi��o X qualquer dentro dos limites.
            x = Random.Range(minPos.position.x, maxPos.position.x);
            // Sorteia se a posi��o Y ser� no topo (maxPos.y) ou na base (minPos.y).
            y = Random.Range(0f, 1f) > 0.5f ? minPos.position.y : maxPos.position.y;
        }
        else
        {
            // Se for vertical:
            // Sorteia uma posi��o Y qualquer dentro dos limites.
            y = Random.Range(minPos.position.y, maxPos.position.y);
            // Sorteia se a posi��o X ser� na esquerda (minPos.x) ou na direita (maxPos.x).
            x = Random.Range(0f, 1f) > 0.5f ? minPos.position.x : maxPos.position.x;
        }

        // Retorna a posi��o calculada.
        return new Vector2(x, y);
    }
}