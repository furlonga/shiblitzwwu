using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;

public class DeckManager
{
    List<Type> basicMovementCards = new List<Type> { typeof(KingMove), typeof(CardinalOneSquare) };
    List<Type> advancedMovementCards = new List<Type> { typeof(GodMove) };
    List<Type> spellCards = new List<Type> { typeof(ThrowFireball) };

    public DeckManager()
    {

    }

    public Type getRandomBasicMovementCard()
    {
        return basicMovementCards[UnityEngine.Random.Range(0, basicMovementCards.Count)];
    }
    public Type getRandomAdvancedMovementCard()
    {
        return advancedMovementCards[UnityEngine.Random.Range(0, advancedMovementCards.Count)];
    }
    public Type getRandomSpellCard()
    {
        return spellCards[UnityEngine.Random.Range(0, spellCards.Count)];
    }

}