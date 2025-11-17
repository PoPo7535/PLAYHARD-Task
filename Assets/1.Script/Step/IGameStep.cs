using UnityEngine;
public interface IGameStep
{
    public void GameSteUpdate();
    public void Enter();
    public void Exit();
}