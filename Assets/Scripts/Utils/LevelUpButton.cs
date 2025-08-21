// Importa a biblioteca do TextMeshPro para usar os componentes de texto avançado.
using TMPro;
// Importa a biblioteca principal do motor da Unity.
using UnityEngine;
// Importa a biblioteca de UI (Interface de Usuário) da Unity, necessária para usar o componente "Image".
using UnityEngine.UI;

// Define a classe "LevelUpButton" (Botão de Subir de Nível).
// Este script irá controlar um botão individual na tela de seleção de upgrades.
public class LevelUpButton : MonoBehaviour
{
    // Uma referência pública para o componente de texto que exibirá o nome da arma.
    public TMP_Text weaponName;
    // Uma referência pública para o componente de texto que exibirá a descrição do upgrade da arma.
    public TMP_Text weaponDescription;
    // Uma referência pública para o componente de Imagem que exibirá o ícone da arma.
    public Image weaponIcon;

    // Uma variável privada para guardar a referência da arma que este botão representa no momento.
    private Weapon assignedWeapon;

    // Um método público para "ativar" ou configurar o botão com os dados de uma arma específica.
    // O parâmetro "weapon" é a arma cujas informações serão exibidas.
    public void ActivateButton(Weapon weapon)
    {
        // Define o texto do nome da arma. "weapon.name" pega o nome do GameObject ao qual o script da arma está anexado.
        weaponName.text = weapon.name;
        // Define o texto da descrição. Ele busca a descrição nos "stats" da arma,
        // usando o "weaponLevel" atual para encontrar a descrição correta na lista de stats.
        weaponDescription.text = weapon.stats[weapon.weaponLevel].description;
        // Define o sprite do ícone. Ele usa a imagem ("weaponImage") configurada no script da arma.
        weaponIcon.sprite = weapon.weaponImage;

        // Armazena a referência da arma na variável "assignedWeapon". Isso é importante para que,
        // quando o jogador clicar, o botão "saiba" qual arma deve ser melhorada.
        assignedWeapon = weapon;
    }

    // Método público que será chamado quando o jogador clicar neste botão.
    // No editor do Unity, esta função será vinculada ao evento "OnClick" do componente Button.
    public void SelectUpgrade()
    {
        // Chama o método "LevelUp()" na arma que foi atribuída a este botão.
        assignedWeapon.LevelUp();
        // Acessa a instância global do "UIController" e chama o método para fechar o painel de level up,
        // já que o jogador acabou de fazer sua escolha.
        UIController.instance.LevelUpPanelClose();
    }
}