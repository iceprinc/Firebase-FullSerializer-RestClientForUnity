public class Task
{
    public string taskName, userName, date, hard;
    public Task(string _taskName, string _userName, string _date, string _hard)
    {
        taskName = _taskName;
        date = _date;
        userName = _userName;
        hard = _hard;
    }
    public Task()
    {
        taskName = "task";
        date = "01-01-0001";
        userName = "name";
        hard = "0";
    }
}