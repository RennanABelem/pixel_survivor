// Importa a biblioteca de cole��es gen�ricas do C#, que � necess�ria para usar "List<>".
using System.Collections.Generic;
// Importa a biblioteca principal do motor da Unity, essencial para criar componentes como este.
using UnityEngine;

// Define uma classe p�blica chamada "Weapon" (Arma).
// Ela herda de "MonoBehaviour", o que significa que este script pode ser anexado a um GameObject no Unity.
public class Weapon : MonoBehaviour
{
    // Vari�vel p�blica para armazenar o n�vel atual da arma. 
    // Por ser "public", ela aparecer� no Inspector do Unity para ser ajustada.
    public int weaponLevel;

    // Uma lista p�blica de "WeaponStats". Cada item nesta lista representar� os atributos 
    // da arma em um n�vel espec�fico (n�vel 1, n�vel 2, etc.).
    public List<WeaponStats> stats;

    // Vari�vel p�blica para guardar a imagem (�cone) da arma.
    // �til para mostrar na interface do usu�rio (UI).
    public Sprite weaponImage;

    // Um m�todo (fun��o) p�blico chamado "LevelUp" que pode ser chamado por outros scripts.
    public void LevelUp()
    {
        // Verifica se o n�vel atual da arma � menor que o n�mero total de n�veis configurados.
        // "stats.Count" � o n�mero total de itens na lista "stats".
        // O "- 1" � usado porque as listas come�am no �ndice 0. Isso impede que a arma passe do n�vel m�ximo.
        if (weaponLevel < stats.Count - 1)
        {
            // Se a condi��o for verdadeira, aumenta o n�vel da arma em 1.
            weaponLevel++;
        }
    }
}

// Atributo especial que permite que a Unity "serialize" esta classe.
// Em termos simples, isso faz com que os campos da classe "WeaponStats" apare�am no Inspector do Unity
// quando voc� a usa em uma lista p�blica, como a lista "stats" acima.
[System.Serializable]
public class WeaponStats
{
    // O tempo de espera (em segundos) entre um ataque e outro.
    public float cooldown;
    // A dura��o (em segundos) de um ataque ou efeito (ex: uma po��o de veneno).
    public float duration;
    // A quantidade de dano que a arma causa.
    public float damage;
    // O alcance do ataque.
    public float range;
    // A velocidade do ataque ou do proj�til.
    public float speed;
    // Uma descri��o em texto do que o upgrade deste n�vel faz. �til para a UI.
    public string description;
}