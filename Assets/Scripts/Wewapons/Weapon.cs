// Importa a biblioteca de coleções genéricas do C#, que é necessária para usar "List<>".
using System.Collections.Generic;
// Importa a biblioteca principal do motor da Unity, essencial para criar componentes como este.
using UnityEngine;

// Define uma classe pública chamada "Weapon" (Arma).
// Ela herda de "MonoBehaviour", o que significa que este script pode ser anexado a um GameObject no Unity.
public class Weapon : MonoBehaviour
{
    // Variável pública para armazenar o nível atual da arma. 
    // Por ser "public", ela aparecerá no Inspector do Unity para ser ajustada.
    public int weaponLevel;

    // Uma lista pública de "WeaponStats". Cada item nesta lista representará os atributos 
    // da arma em um nível específico (nível 1, nível 2, etc.).
    public List<WeaponStats> stats;

    // Variável pública para guardar a imagem (ícone) da arma.
    // Útil para mostrar na interface do usuário (UI).
    public Sprite weaponImage;

    // Um método (função) público chamado "LevelUp" que pode ser chamado por outros scripts.
    public void LevelUp()
    {
        // Verifica se o nível atual da arma é menor que o número total de níveis configurados.
        // "stats.Count" é o número total de itens na lista "stats".
        // O "- 1" é usado porque as listas começam no índice 0. Isso impede que a arma passe do nível máximo.
        if (weaponLevel < stats.Count - 1)
        {
            // Se a condição for verdadeira, aumenta o nível da arma em 1.
            weaponLevel++;
        }
    }
}

// Atributo especial que permite que a Unity "serialize" esta classe.
// Em termos simples, isso faz com que os campos da classe "WeaponStats" apareçam no Inspector do Unity
// quando você a usa em uma lista pública, como a lista "stats" acima.
[System.Serializable]
public class WeaponStats
{
    // O tempo de espera (em segundos) entre um ataque e outro.
    public float cooldown;
    // A duração (em segundos) de um ataque ou efeito (ex: uma poção de veneno).
    public float duration;
    // A quantidade de dano que a arma causa.
    public float damage;
    // O alcance do ataque.
    public float range;
    // A velocidade do ataque ou do projétil.
    public float speed;
    // Uma descrição em texto do que o upgrade deste nível faz. Útil para a UI.
    public string description;
}