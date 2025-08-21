// Importa a biblioteca de cole��es gen�ricas do C#, necess�ria para usar "List<>".
using System.Collections.Generic;
// Importa a biblioteca principal do motor da Unity.
using UnityEngine;

// Define a classe para o Prefab da arma de �rea. Este script ser� anexado ao objeto que
// � criado na cena para representar a �rea de dano.
public class AreaWeaponPrefab : MonoBehaviour
{
    // Uma refer�ncia p�blica ao script principal da arma (AreaWeapon).
    // Isso conecta o prefab instanciado com os dados da arma que o criou.
    public AreaWeapon weapon;

    // Vari�vel privada para guardar o tamanho alvo que a �rea de efeito deve atingir.
    private Vector3 targetSize;
    // Um cron�metro privado para controlar a dura��o total da arma na cena.
    private float timer;
    // Uma lista p�blica que armazena todos os inimigos que est�o atualmente dentro do alcance da arma.
    public List<Enemy> enemiesInRange;
    // Um contador privado usado para controlar a frequ�ncia do dano peri�dico (dano por segundo).
    private float counter;

    // A fun��o Start � chamada uma vez na vida do script, quando o objeto � criado (instanciado).
    void Start()
    {
        // Encontra o GameObject na cena que se chama "Area Weapon" e pega o componente (script) "AreaWeapon" dele.
        // Isso estabelece a comunica��o entre o prefab e o controlador da arma no jogador.
        weapon = GameObject.Find("Area Weapon").GetComponent<AreaWeapon>();

        // Define o tamanho alvo da �rea. Ele pega um vetor base (1,1,1) e multiplica pelo "range" (alcance)
        // definido nos stats do n�vel atual da arma.
        targetSize = Vector3.one * weapon.stats[weapon.weaponLevel].range;
        // Inicia a escala do objeto como zero, para que ele possa crescer visualmente at� o tamanho alvo.
        transform.localScale = Vector3.zero;
        // Define a dura��o total da arma, buscando o valor nos stats do n�vel atual.
        timer = weapon.stats[weapon.weaponLevel].duration;
    }

    // A fun��o Update � chamada a cada frame do jogo.
    void Update()
    {
        // Chama a fun��o que controla o crescimento e encolhimento da �rea da arma.
        GrowAndShrinkWeaponArea();

        // Coment�rio original: dano peri�dico.
        // Diminui o contador do dano a cada segundo que passa.
        counter -= Time.deltaTime;
        // Se o contador chegar a zero ou menos, significa que � hora de aplicar o dano.
        if (counter <= 0)
        {
            // Reinicia o contador com o valor de "speed" dos stats do n�vel atual.
            // Neste contexto, "speed" est� sendo usado como o intervalo entre os "ticks" de dano.
            counter = weapon.stats[weapon.weaponLevel].speed;

            // Percorre cada inimigo que est� atualmente na lista de "inimigos no alcance".
            foreach (Enemy enemy in enemiesInRange)
            {
                // Chama a fun��o "TakeDamage" do inimigo, passando a quantidade de dano
                // definida nos stats do n�vel atual da arma.
                enemy.TakeDamage(weapon.stats[weapon.weaponLevel].damage);
            }
        }
    }

    // Fun��o privada para controlar a anima��o de crescimento e encolhimento da �rea.
    private void GrowAndShrinkWeaponArea()
    {
        // Coment�rio original: cresce e encolhe em dire��o ao targetSize.
        // Move suavemente a escala atual do objeto em dire��o � escala alvo (targetSize).
        // "Time.deltaTime * 6" controla a velocidade com que a �rea cresce.
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, Time.deltaTime * 6);

        // Diminui o cron�metro da dura��o total da arma.
        timer -= Time.deltaTime;
        // Se o tempo de dura��o da arma acabar...
        if (timer <= 0)
        {
            // ...o tamanho alvo � definido como zero, fazendo com que a �rea comece a encolher.
            targetSize = Vector3.zero;
            // Se a �rea j� encolheu completamente (sua escala no eixo X � zero)...
            if (transform.localScale.x == 0f)
            {
                // ...destr�i o objeto da arma, removendo-o da cena.
                Destroy(gameObject);
            }
        }
    }

    // Este � um bloco de c�digo comentado. Provavelmente era uma forma antiga de aplicar dano.
    // OnTriggerStay2D � chamado a cada frame em que outro colisor permanece dentro do trigger.
    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")){
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage(weapon.damage);
        }
    } */

    // Esta fun��o � chamada pela Unity quando um outro colisor entra na �rea de trigger deste objeto.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que entrou tem a tag "Enemy".
        if (collision.CompareTag("Enemy"))
        {
            // Se for um inimigo, adiciona o componente "Enemy" dele � lista de inimigos no alcance.
            enemiesInRange.Add(collision.GetComponent<Enemy>());
        }
    }

    // Esta fun��o � chamada pela Unity quando um outro colisor sai da �rea de trigger deste objeto.
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