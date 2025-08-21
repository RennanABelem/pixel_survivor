// Importa a biblioteca do TextMeshPro para usar os componentes de texto avan�ado.
using TMPro;
// Importa a biblioteca principal do motor da Unity.
using UnityEngine;
// Importa a biblioteca de UI (Interface de Usu�rio) da Unity, necess�ria para usar o componente "Image".
using UnityEngine.UI;

// Define a classe "LevelUpButton" (Bot�o de Subir de N�vel).
// Este script ir� controlar um bot�o individual na tela de sele��o de upgrades.
public class LevelUpButton : MonoBehaviour
{
    // Uma refer�ncia p�blica para o componente de texto que exibir� o nome da arma.
    public TMP_Text weaponName;
    // Uma refer�ncia p�blica para o componente de texto que exibir� a descri��o do upgrade da arma.
    public TMP_Text weaponDescription;
    // Uma refer�ncia p�blica para o componente de Imagem que exibir� o �cone da arma.
    public Image weaponIcon;

    // Uma vari�vel privada para guardar a refer�ncia da arma que este bot�o representa no momento.
    private Weapon assignedWeapon;

    // Um m�todo p�blico para "ativar" ou configurar o bot�o com os dados de uma arma espec�fica.
    // O par�metro "weapon" � a arma cujas informa��es ser�o exibidas.
    public void ActivateButton(Weapon weapon)
    {
        // Define o texto do nome da arma. "weapon.name" pega o nome do GameObject ao qual o script da arma est� anexado.
        weaponName.text = weapon.name;
        // Define o texto da descri��o. Ele busca a descri��o nos "stats" da arma,
        // usando o "weaponLevel" atual para encontrar a descri��o correta na lista de stats.
        weaponDescription.text = weapon.stats[weapon.weaponLevel].description;
        // Define o sprite do �cone. Ele usa a imagem ("weaponImage") configurada no script da arma.
        weaponIcon.sprite = weapon.weaponImage;

        // Armazena a refer�ncia da arma na vari�vel "assignedWeapon". Isso � importante para que,
        // quando o jogador clicar, o bot�o "saiba" qual arma deve ser melhorada.
        assignedWeapon = weapon;
    }

    // M�todo p�blico que ser� chamado quando o jogador clicar neste bot�o.
    // No editor do Unity, esta fun��o ser� vinculada ao evento "OnClick" do componente Button.
    public void SelectUpgrade()
    {
        // Chama o m�todo "LevelUp()" na arma que foi atribu�da a este bot�o.
        assignedWeapon.LevelUp();
        // Acessa a inst�ncia global do "UIController" e chama o m�todo para fechar o painel de level up,
        // j� que o jogador acabou de fazer sua escolha.
        UIController.instance.LevelUpPanelClose();
    }
}