using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<ActionToken> actionTokens = new List<ActionToken>(2);

    public bool HasAvailableAction()
    {
        return actionTokens.Any(token => token.isAvailable);
    }

    public void UseActionToken()
    {
        var availableToken = actionTokens.FirstOrDefault(token => token.isAvailable);
        if (availableToken != null)
        {
            availableToken.Consume();
        }
        else
        {
            throw new InvalidOperationException("No available action tokens to use.");
        }
    }

    public void RefreshActionTokens()
    {
        foreach (var token in actionTokens)
        {
            token.Refresh();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
