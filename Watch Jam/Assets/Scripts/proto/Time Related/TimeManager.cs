using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void ReversingTimeActionDelegate();
public delegate void ReversingTimeActionDelegate2(Object ob = null);
class ReversingActionWithTime
{
    public ReversingActionWithTime(float time, ReversingTimeActionDelegate2 action)
    {
        this.time = time;
        this.actionToCarry = action;
    }
    public float time;
    public ReversingTimeActionDelegate2 actionToCarry;
}
public interface IRewindAble
{

    void startRewind();
    void stopRewind();
}
public class TimeManager : MonoBehaviour {
    public static TimeManager _instance;
    Stack<ReversingActionWithTime> actions = new Stack<ReversingActionWithTime>();
    // Use this for initialization

    public float curentTime;
    public bool isRewinding = false;
    private List<IRewindAble> rewindables;
    public Event test;
    
    void Awake () {
        _instance = this;
    }
	

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            startRewind();

        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            stopRewind();

        }
    }
    private void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }
    public void Rewind()
    {
        curentTime -= Time.fixedDeltaTime;
        while(actions.Count >0 && (actions.Peek().time >= curentTime))
        {
            var action = actions.Pop();
            action.actionToCarry(); 
        }
      
    }
    public void Record()
    {
        curentTime += Time.fixedDeltaTime;
    }
    public void startRewind()
    {
        isRewinding = true;

    }
    public void stopRewind()
    {

        isRewinding = false;
    }
    public void RememberAction(ReversingTimeActionDelegate2 action)
    {
        actions.Push(new ReversingActionWithTime(curentTime, action));
    }
}
