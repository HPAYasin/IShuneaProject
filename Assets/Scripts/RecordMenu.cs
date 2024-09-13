using UnityEngine;
using UnityEngine.UI;

public class RecordMenu : MonoBehaviour
{
    public GameObject mainMenuUI;       
    public GameObject recordMenuUI;     
    public Text bestScoreText;           
    public Text bestScoreDateText;       

    private void Start()
    {
        recordMenuUI.SetActive(false);
    }

    public void OpenRecordMenu()
    {
        (int bestScore, string bestScoreDate) = ScoreManager.GetBestScore();

        bestScoreText.text = "Best Score: " + bestScore.ToString();
        bestScoreDateText.text = "Date: " + bestScoreDate;

        mainMenuUI.SetActive(false);
        recordMenuUI.SetActive(true);
    }

    public void CloseRecordMenu()
    {
        recordMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }
}
