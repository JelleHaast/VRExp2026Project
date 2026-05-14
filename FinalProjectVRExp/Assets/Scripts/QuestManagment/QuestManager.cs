using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/QuestData")]
public class QuestData : ScriptableObject
{
    public string questName;
    public bool isCompleted;
    public void OnEnable() => isCompleted = false;
}