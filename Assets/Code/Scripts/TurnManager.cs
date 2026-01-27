using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private int currentPlayerIndex = 0;
    private Player currentPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartTurn();
    }

    public void StartTurn()
    {
        currentPlayer = GameManager.Instance.players[currentPlayerIndex];
        currentPlayer.RefreshActionTokens();

        Debug.Log($"Player {currentPlayerIndex + 1}'s turn started.");
    }

    public void EndTurn()
    {
        Debug.Log($"Player {currentPlayerIndex + 1}'s turn ended.");

        currentPlayerIndex = (currentPlayerIndex + 1) % GameManager.Instance.players.Count;
        StartTurn();
    }

    public Player GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public int GetCurrentPlayerIndex()
    {
        return currentPlayerIndex;
    }

    public bool CanCurrentPlayerAct()
    {
        return currentPlayer.HasAvailableAction();
    }
}
