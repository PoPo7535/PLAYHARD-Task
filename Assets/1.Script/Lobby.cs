using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    void Start()
    {
        startBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("0.Scene/GameScene");
        });
    }

}
