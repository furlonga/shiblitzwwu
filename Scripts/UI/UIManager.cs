using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager
{
    public enum Card { LEFT, CENTER, RIGHT };

    public Slider playerHealth;
    public Slider playerMana;

    public Button leftCard;
    public ShiblitzMove leftCardMove;

    public Button centerCard;
    public ShiblitzMove centerCardMove;

    public Button rightCard;
    public ShiblitzMove rightCardMove;

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

    public void setInitialCards()
    {
        setCard(typeof(KingMove), leftCard);
        setCard(typeof(ThrowFireball), centerCard);
        setCard(typeof(GodMove), rightCard);
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
        Game.getInputManager().selectCard(Card.LEFT);
    }
    public void clickedCenterCard()
    {
        Game.getInputManager().selectCard(Card.CENTER);
    }
    public void clickedRightCard()
    {
        Game.getInputManager().selectCard(Card.RIGHT);
    }

    public void setCard(Type cardType, Button card)
    {
        ShiblitzMove move = (ShiblitzMove)Activator.CreateInstance(cardType, Game.getPlayer());
        card.transform.GetChild(0).GetComponent<Text>().text = cardType.Name + "\n(" + move.manaCost + " mana" + (move.isCantrip ? " - instant)" : ")");
        if (card == leftCard)
            leftCardMove = move;
        if (card == centerCard)
            centerCardMove = move;
        if (card == rightCard)
            rightCardMove = move;
    }

    public ShiblitzMove getMove(Card card)
    {
        switch(card)
        {
            case Card.LEFT:
                return leftCardMove;
            case Card.CENTER:
                return centerCardMove;
            case Card.RIGHT:
                return rightCardMove;
            default:
                return leftCardMove;
        }
    }

    public void drawIntoSlot(Card card)
    {
        switch(card)
        {
            case Card.LEFT:
                setCard(Game.getDeckManager().getRandomBasicMovementCard(), leftCard);
                break;
            case Card.CENTER:
                setCard(Game.getDeckManager().getRandomSpellCard(), centerCard);
                break;
            case Card.RIGHT:
                setCard(Game.getDeckManager().getRandomAdvancedMovementCard(), rightCard);
                break;
        }
    }

}
