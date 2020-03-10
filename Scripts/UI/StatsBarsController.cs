using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsBarsController : MonoBehaviour
{
    [SerializeField]
    private HealthBarController health;
    [SerializeField]
    private HealthBarController mana;
    private Character target;
    private Vector2 offset = new Vector2(0, 0f);

    // Start is called before the first frame update
    void Awake()
    {
        //health = transform.GetChild(0).GetComponent<HealthBarController>();
        //mana = transform.GetChild(1).GetComponent<HealthBarController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target.gameObject == null)
            Destroy(gameObject);
        else
        {
            transform.position = Camera.main.WorldToScreenPoint(target.gameObject.transform.position + new Vector3(0, target.spriteOffset.y, 0) + (Vector3)offset);
            transform.localScale = new Vector3(.2f, .4f, 1) / (Camera.main.orthographicSize / 10);
        }
    }

    public void setMaxHealth(int maxHealth)
    {
        health.setMaxHealth(maxHealth);
    }
    public void setMaxMana(int maxMana)
    {
        mana.setMaxHealth(maxMana);
    }
    public void setHealth(int health)
    {
        this.health.setHealth(health);
    }
    public void setMana(int mana)
    {
        this.mana.setHealth(mana);
    }
    public void setTarget(Character character)
    {
        target = character;
    }

}
