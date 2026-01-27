using UnityEngine;
using UnityEngine.InputSystem;

public class TurnManager : MonoBehaviour
{
    private int currentPlayerIndex = 0;
    private Player currentPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartTurn();
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            EndTurn();
        }
    }

    public void StartTurn()
    {
        currentPlayer = GameManager.Instance.players[currentPlayerIndex];
        currentPlayer.RefreshActionTokens();

        GameManager.Instance.SwitchCameraToPlayer(currentPlayerIndex);

        Debug.Log($"Player {currentPlayerIndex + 1}'s turn started. Press Space to end turn.");
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
