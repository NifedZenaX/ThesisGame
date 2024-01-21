using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int totalCustomers;
    [SerializeField] private TextMeshProUGUI customerText;
    private int currentSatisfiedCustomers = 0;
    public GameObject finishPanel, closePanel;

    public static ScoreManager instance;
    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        customerText.text = currentSatisfiedCustomers + "/" + totalCustomers;
    }

    public void AddSatisfiedCustomer()
    {
        currentSatisfiedCustomers++;
        customerText.text = currentSatisfiedCustomers + "/" + totalCustomers;
        if(currentSatisfiedCustomers == totalCustomers)
        {
            GameFinished(true);
        }
    }

    public void GameFinished(bool targetReached)
    {
        Time.timeScale = 0;
        foreach (GameModule gm in ModuleManager.Instance.gameModules)
        {
            gm.gameObject.SetActive(false);
        }

        if (targetReached)
        {
            finishPanel.SetActive(true);
        }
        else
        {
            closePanel.SetActive(true);
        }
    }
}
