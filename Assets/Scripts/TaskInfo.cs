using UnityEngine;
using UnityEngine.UI;

public class TaskInfo : MonoBehaviour
{
    public Text Name;

    public void ShowInfo(string name)
    {
        Name.text = name;
    }
}
