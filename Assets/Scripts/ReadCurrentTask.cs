using FullSerializer;
using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadCurrentTask : MonoBehaviour
{
    public void ClickGetInfo()
    {
        GameObject.Find("TaskManager").GetComponent<TaskManager>().CallCurrentTask(GetComponentInChildren<Text>().text);
    }
}
