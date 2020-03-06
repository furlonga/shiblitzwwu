using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    public Slider playerHealth;
    public Slider playerMana;

    public Button leftCard;
    public Button centerCard;
    public Button rightCard;

    public UIManager()
    {
        playerHealth = GameObject.Find("PlayerHealth").GetComponent<Slider>();
        playerMana = GameObject.Find("PlayerMana").GetComponent<Slider>();
        leftCard = GameObject.Find("LeftCard").GetComponent<Button>();
        centerCard = GameObject.Find("CenterCard").GetComponent<Button>();
        rightCard = GameObject.Find("RightCard").GetComponent<Button>();
        leftCard.onClick.AddListener(clickedLeftCard);
        centerCard.onClick.AddListener(clickedCenterCard);
        rightCard.onClick.AddListener(clickedRightCard);
    }

    public void setMaxStats()
    {
        playerHealth.maxValue = Game.getPlayer().maxHealth;
        playerMana.maxValue = Game.getPlayer().maxMana;
    }

    public void reflectPlayerStats()
    {
        playerHealth.value = Game.getPlayer().health;
        playerMana.value = Game.getPlayer().mana;
    }

    public void clickedLeftCard()
    {
        Game.getInputManager().selectMove(new KingMove(Game.getPlayer()));
    }
    public void clickedCenterCard()
    {
        Game.getInputManager().selectMove(new StabMove(Game.getPlayer()));
    }
    public void clickedRightCard()
    {
        Game.getInputManager().selectMove(new GodMove(Game.getPlayer()));
    }
}
