using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndStep : IGameStep
{
    
    public void GameSteUpdate()
    {
    }

    public async void Enter()
    {
        HexagonGrid.I.SpareBubblePop();
        await GameStepManager.I.shooter.SpareBubbleToScore();
        var str = GameStepManager.I.boss.IsDead ? "Win" : "Lose";
        PopUp.I.ShowPopUp($"{str}\n Score : {ScoreHelper.TotalScore}", "Go Lobby",() =>
        {
            PopUp.I.ClosePopUp();
            SceneManager.LoadScene("0.Scene/LobbyScene");
        });
    }

    public void Exit()
    {
    }
}
