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
            GameFinished();
        }
    }

    private void GameFinished()
    {
        Debug.Log("All customers has been satisfied");
        SceneManager.LoadScene(ButtonManager.MAIN_MENU_SCENE);
    }
}
