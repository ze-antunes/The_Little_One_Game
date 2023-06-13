using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.N))
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 != SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    public void LoadNextLevelWithItem(int sceneId)
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 != SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadLevel(sceneId));
        }
    }

    public void LoadNextLevelMirror(int sceneId)
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 != SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadLevel(sceneId));
        }
    }

    public void LoadNextLevelCoin(int sceneId)
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 != SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadLevel(sceneId));
        }
    }

    public void LoadNextLevelBow(int sceneId)
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 != SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadLevel(sceneId));
        }
    }

    public void LoadRandomLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 != SceneManager.sceneCountInBuildSettings)
        {
            List<int> list1 = StateController.LevelPlayerPassed;
            List<int> list2 = new List<int> { 2, 3, 4 };

            int differentValue = GetDifferentValue(list1, list2);

            if (differentValue != -1)
            {
                StartCoroutine(LoadLevel(differentValue));
            }
            else
            {
                Debug.Log("No different value found.");
            }
        }
    }

    public int GetDifferentValue(List<int> list1, List<int> list2)
    {
        List<int> allValues = list1.Concat(list2).ToList();

        foreach (int value in allValues)
        {
            if (list1.Contains(value) != list2.Contains(value))
            {
                return value;
            }
        }

        return -1; // Indicates no different value found
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
