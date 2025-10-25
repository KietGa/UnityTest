using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void NormalMode()
    {
        PlayerPrefs.SetInt("mode", 0);
        SceneManager.LoadScene("Intern");
    }

    public void TimeAttack()
    {
        PlayerPrefs.SetInt("mode", 1);
        SceneManager.LoadScene("Intern");
    }
}
