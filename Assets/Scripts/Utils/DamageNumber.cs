using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{

    [SerializeField] private TMP_Text damageText;
    private float floatSpeed = 1.2f;

    void Start()
    {
        Destroy(gameObject, 1);
    }

    void Update()
    {
        AddEffectMoveUp();
    }

    private void AddEffectMoveUp()
    {
        //Adiciona um efeito de o numero ir para cima quando surgi
        transform.position += Vector3.up * Time.deltaTime * floatSpeed;
    }

    public void SetValue(int value)
    {
        damageText.text = value.ToString();
    }
}
