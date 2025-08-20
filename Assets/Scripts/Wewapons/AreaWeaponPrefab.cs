using System.Collections.Generic;
using UnityEngine;

public class AreaWeaponPrefab : MonoBehaviour
{

    public AreaWeapon weapon;
    private Vector3 targetSize;
    private float timer;
    public List<Enemy> enemiesInRange;
    private float counter;

    void Start()
    {
        weapon = GameObject.Find("Area Weapon").GetComponent<AreaWeapon>();

        targetSize = Vector3.one * weapon.stats[weapon.weaponLevel].range;
        transform.localScale = Vector3.zero;
        timer = weapon.stats[weapon.weaponLevel].duration;
    }

    // Update is called once per frame
    void Update()
    {
        GrowAndShrinkWeaponArea();

        //periodic damage
        counter -= Time.deltaTime;
        if (counter <= 0) 
        {
            counter = weapon.stats[weapon.weaponLevel].speed;    

            foreach(Enemy enemy in enemiesInRange)
            {
                enemy.TakeDamage(weapon.stats[weapon.weaponLevel].damage);
            }
        }
    }

    private void GrowAndShrinkWeaponArea()
    {
        // grow and shrink towards targetSize
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, Time.deltaTime * 6);

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            targetSize = Vector3.zero;
            if (transform.localScale.x == 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")){
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage(weapon.damage);
        }
    } */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collision.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.GetComponent<Enemy>());
        }
    }
}
