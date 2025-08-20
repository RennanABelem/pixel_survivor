using UnityEngine;

public class AreaWeapon : Weapon
{

    [Header("Components")]
    [SerializeField] private GameObject prefab;
    private float spawnCouter;

    void Update()
    {
        spawnCouter -= Time.deltaTime;

        if(spawnCouter < 0)
        {
            spawnCouter = stats[weaponLevel].cooldown;
            Instantiate(prefab, transform.position, transform.rotation, transform);
        }
    }
}
