// Importa a biblioteca de coleções genéricas do C#, necessária para usar "List<>".
using System.Collections.Generic;
// Importa a biblioteca principal do motor da Unity.
using UnityEngine;

// Define a classe para o Prefab da arma de área. Este script será anexado ao objeto que
// é criado na cena para representar a área de dano.
public class AreaWeaponPrefab : MonoBehaviour
{
    // Uma referência pública ao script principal da arma (AreaWeapon).
    // Isso conecta o prefab instanciado com os dados da arma que o criou.
    public AreaWeapon weapon;

    // Variável privada para guardar o tamanho alvo que a área de efeito deve atingir.
    private Vector3 targetSize;
    // Um cronômetro privado para controlar a duração total da arma na cena.
    private float timer;
    // Uma lista pública que armazena todos os inimigos que estão atualmente dentro do alcance da arma.
    public List<Enemy> enemiesInRange;
    // Um contador privado usado para controlar a frequência do dano periódico (dano por segundo).
    private float counter;

    // A função Start é chamada uma vez na vida do script, quando o objeto é criado (instanciado).
    void Start()
    {
        // Encontra o GameObject na cena que se chama "Area Weapon" e pega o componente (script) "AreaWeapon" dele.
        // Isso estabelece a comunicação entre o prefab e o controlador da arma no jogador.
        weapon = GameObject.Find("Area Weapon").GetComponent<AreaWeapon>();

        // Define o tamanho alvo da área. Ele pega um vetor base (1,1,1) e multiplica pelo "range" (alcance)
        // definido nos stats do nível atual da arma.
        targetSize = Vector3.one * weapon.stats[weapon.weaponLevel].range;
        // Inicia a escala do objeto como zero, para que ele possa crescer visualmente até o tamanho alvo.
        transform.localScale = Vector3.zero;
        // Define a duração total da arma, buscando o valor nos stats do nível atual.
        timer = weapon.stats[weapon.weaponLevel].duration;
    }

    // A função Update é chamada a cada frame do jogo.
    void Update()
    {
        // Chama a função que controla o crescimento e encolhimento da área da arma.
        GrowAndShrinkWeaponArea();

        // Comentário original: dano periódico.
        // Diminui o contador do dano a cada segundo que passa.
        counter -= Time.deltaTime;
        // Se o contador chegar a zero ou menos, significa que é hora de aplicar o dano.
        if (counter <= 0)
        {
            // Reinicia o contador com o valor de "speed" dos stats do nível atual.
            // Neste contexto, "speed" está sendo usado como o intervalo entre os "ticks" de dano.
            counter = weapon.stats[weapon.weaponLevel].speed;

            // Percorre cada inimigo que está atualmente na lista de "inimigos no alcance".
            foreach (Enemy enemy in enemiesInRange)
            {
                // Chama a função "TakeDamage" do inimigo, passando a quantidade de dano
                // definida nos stats do nível atual da arma.
                enemy.TakeDamage(weapon.stats[weapon.weaponLevel].damage);
            }
        }
    }

    // Função privada para controlar a animação de crescimento e encolhimento da área.
    private void GrowAndShrinkWeaponArea()
    {
        // Comentário original: cresce e encolhe em direção ao targetSize.
        // Move suavemente a escala atual do objeto em direção à escala alvo (targetSize).
        // "Time.deltaTime * 6" controla a velocidade com que a área cresce.
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, Time.deltaTime * 6);

        // Diminui o cronômetro da duração total da arma.
        timer -= Time.deltaTime;
        // Se o tempo de duração da arma acabar...
        if (timer <= 0)
        {
            // ...o tamanho alvo é definido como zero, fazendo com que a área comece a encolher.
            targetSize = Vector3.zero;
            // Se a área já encolheu completamente (sua escala no eixo X é zero)...
            if (transform.localScale.x == 0f)
            {
                // ...destrói o objeto da arma, removendo-o da cena.
                Destroy(gameObject);
            }
        }
    }

    // Este é um bloco de código comentado. Provavelmente era uma forma antiga de aplicar dano.
    // OnTriggerStay2D é chamado a cada frame em que outro colisor permanece dentro do trigger.
    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")){
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage(weapon.damage);
        }
    } */

    // Esta função é chamada pela Unity quando um outro colisor entra na área de trigger deste objeto.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que entrou tem a tag "Enemy".
        if (collision.CompareTag("Enemy"))
        {
            // Se for um inimigo, adiciona o componente "Enemy" dele à lista de inimigos no alcance.
            enemiesInRange.Add(collision.GetComponent<Enemy>());
        }
    }

    // Esta função é chamada pela Unity quando um outro colisor sai da área de trigger deste objeto.
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Verifica se o objeto que saiu tem a tag "Enemy".
        if (collision.CompareTag("Enemy"))
        {
            // Se for um inimigo, remove o componente "Enemy" dele da lista de inimigos no alcance.
            enemiesInRange.Remove(collision.GetComponent<Enemy>());
        }
    }
}