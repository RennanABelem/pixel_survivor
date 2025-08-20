using UnityEngine;

public class DamageNumberController : MonoBehaviour
{
    public static DamageNumberController instance;
    public DamageNumber prefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreateNumber(float value, Vector2 location)
    {
        DamageNumber damageNumer = Instantiate(prefab, location, transform.rotation, transform);
        damageNumer.SetValue(Mathf.RoundToInt(value));   
    }
}
