using TMPro;
using UnityEngine;

public class WinScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI enemyPoints;
    [SerializeField] TextMeshProUGUI energyPoints;
    [SerializeField] TextMeshProUGUI missilePoints;
    [SerializeField] TextMeshProUGUI soldierSavePoints;
    [SerializeField] TextMeshProUGUI enemyEscapePoints;
    [SerializeField] TextMeshProUGUI soldierKillPoints;
    [SerializeField] TextMeshProUGUI extraPoints;
    [SerializeField] TextMeshProUGUI finalResult;

    private void Start()
    {
        enemyPoints.text = GameManager.Instance.enemyKillPoints.ToString();
        energyPoints.text = (GameManager.Instance.enLeft * 10).ToString();
        missilePoints.text = (GameManager.Instance.misLeft * 30).ToString();
        soldierSavePoints.text = GameManager.Instance.soldierSavedPoints.ToString();
        enemyEscapePoints.text = (GameManager.Instance.enemyEscapePoints * -1).ToString();
        soldierKillPoints.text = (GameManager.Instance.soldierKilledPoints * -1).ToString();

        float extra = 0;
        if (GameManager.Instance.hitless)
        { extra += 10000;}

        if (GameManager.Instance.playerTime < 300) { extra += 5000; }
        else if (GameManager.Instance.playerTime < 420) { extra += 3000; }
        else if (GameManager.Instance.playerTime < 600) { extra += 1000; }
        extraPoints.text = extra.ToString();

        float totalPoints = 0;
        totalPoints += GameManager.Instance.enemyKillPoints;
        totalPoints += (GameManager.Instance.enLeft * 10);
        totalPoints += (GameManager.Instance.misLeft * 30);
        totalPoints += GameManager.Instance.soldierSavedPoints;
        totalPoints += GameManager.Instance.enemyEscapePoints * -1;
        totalPoints += GameManager.Instance.soldierKilledPoints * -1;
        totalPoints += extra;

        if (totalPoints >= 20000)
        { finalResult.text = "S";  }
        else if (totalPoints >= 10000)
        { finalResult.text = "A"; }
        else if(totalPoints >= 5000)
        { finalResult.text = "B"; }
        else if(totalPoints >= 3000)
        { finalResult.text = "C"; }
        else if(totalPoints >= 1000)
        { finalResult.text = "D"; }
        else
        { finalResult.text = "F"; }
    }
}
