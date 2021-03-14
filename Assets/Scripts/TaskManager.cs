using FullSerializer;
using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static Task staticTask = new Task();
    public Dictionary<string, Task> tasks;

    [SerializeField]
    private GameObject CurrentTaskPanel, MenuPanel, AddTaskPanel;
    [SerializeField]
    private Text NameText, NameUserText, DateText, HardText;

    [SerializeField]
    private InputField NameTaskInputField, NameUserInputField, DateInputField, HardInputField;

    [SerializeField]
    private Transform PlaceForTasks;
    [SerializeField]
    private GameObject PrefTask;

    public string FireBaseURL = "https://project-firebase-60823-default-rtdb.firebaseio.com/";

    private void Start()
    {
        LoadAllFromFirebase();
    }
    public void LoadAllFromFirebase()
    {
        foreach (Transform child in PlaceForTasks)
        {
            Destroy(child.gameObject);
        }
        RestClient.Get($"{FireBaseURL}tasks.json").Then(response =>
        {
            tasks = new Dictionary<string, Task>();
            var responseJson = response.Text;
            var data = fsJsonParser.Parse(responseJson);
            object deserialized = null;
            fsSerializer serializer = new fsSerializer();
            serializer.TryDeserialize(data, typeof(Dictionary<string, Task>), ref deserialized);
            tasks = deserialized as Dictionary<string, Task>;
            foreach (var task in tasks)
            {
                Debug.Log($"{task.Value.taskName} {task.Value.userName} {task.Value.date} {task.Value.hard}");
                GameObject taskGO = Instantiate(PrefTask, PlaceForTasks, false);
                taskGO.GetComponent<TaskInfo>().ShowInfo(task.Value.taskName);
            }
            Debug.Log(tasks.Count);
        });
    }

    public void CallCurrentTask(string taskName)
    {
        foreach (var task in tasks)
        {
            if (task.Value.taskName == taskName)
            {
                CurrentTaskPanel.SetActive(true);
                MenuPanel.SetActive(false);
                NameText.text = task.Value.taskName;
                NameUserText.text = task.Value.userName;
                DateText.text = task.Value.date;
                HardText.text = task.Value.hard;
            }
        }
    }

    public void ClickDeleteTask()
    {
        RestClient.Delete($"{FireBaseURL}tasks/{NameText.text}.json", (err, res) => {
            if (err != null)
            {
                Debug.LogWarning("Error: " + err.Message);
            }
            else
            {
                Debug.Log("Success delete");
                ClickGoBackToMenu();
                StartCoroutine(WaitSecondAndRefreshTasks());
            }
        });
    }


    IEnumerator WaitSecondAndRefreshTasks()
    {
        foreach (Transform child in PlaceForTasks)
        {
            Destroy(child.gameObject);
        }
        yield return new WaitForSeconds(0.5f);
        LoadAllFromFirebase();
    }
    public void ClickAddToFirebase()
    {
        if(!string.IsNullOrEmpty(NameTaskInputField.text) && !string.IsNullOrEmpty(NameUserInputField.text) && !string.IsNullOrEmpty(DateInputField.text) && !string.IsNullOrEmpty(HardInputField.text))
        {
            Task myTask = new Task(NameTaskInputField.text, NameUserInputField.text, DateInputField.text, HardInputField.text);
            RestClient.Put($"{FireBaseURL}tasks/" + myTask.taskName + ".json", myTask);
            ClickGoBackToMenu();
            StartCoroutine(WaitSecondAndRefreshTasks());
        }
        else
        {
            Debug.LogWarning("Заполните все поля!");
        }
    }
    public void ClickShowAddTaskPanel()
    {
        AddTaskPanel.SetActive(true);
        CurrentTaskPanel.SetActive(false);
        MenuPanel.SetActive(false);
    }
    public void ClickGoBackToMenu()
    {
        AddTaskPanel.SetActive(false);
        CurrentTaskPanel.SetActive(false);
        MenuPanel.SetActive(true);
    }
}
