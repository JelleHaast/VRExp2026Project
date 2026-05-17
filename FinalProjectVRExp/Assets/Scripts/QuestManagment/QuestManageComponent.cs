using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Allows us to use the .All check
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public List<QuestData> allQuests;

    public string nextSceneName;

    public void CheckAllQuests()
    {
        Debug.Log("QuestCompleted");
        bool gameFinished = allQuests.All(q => q.isCompleted);

        if (gameFinished)
        {
            Debug.Log("All quests done! Game Complete.");
            SceneManager.LoadScene(nextSceneName);
        }
    }

    public void ResetAllQuests()
    {
        foreach (QuestData quest in allQuests)
        {
            quest.isCompleted = false;
        }
    }
}