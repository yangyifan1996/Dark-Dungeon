using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public interface IEventInfo
{

}

public class MyEventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;

    public MyEventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}

public class MyEventInfo : IEventInfo
{
    public UnityAction actions;

    public MyEventInfo(UnityAction action)
    {
        actions += action;
    }
}


/// <summary>
/// 事件中心 单例模式对象
/// 1.Dictionary
/// 2.委托
/// 3.观察者设计模式
/// 4.泛型
/// </summary>
public class EventCenter
{
    private static EventCenter instance;

    public static EventCenter GetInstance()
    {
        if (instance == null)
            instance = new EventCenter();
        return instance;
    }
    //key —— 事件的名字（比如：怪物死亡，玩家死亡，通关 等等）
    //value —— 对应的是 监听这个事件 对应的委托函数们
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="name">事件的名字</param>
    /// <param name="action">准备用来处理事件 的委托函数</param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        //有没有对应的事件监听
        //有的情况
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as MyEventInfo<T>).actions += action;
        }
        //没有的情况
        else
        {
            eventDic.Add(name, new MyEventInfo<T>(action));
        }
    }

    /// <summary>
    /// 监听不需要参数传递的事件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventListener(string name, UnityAction action)
    {
        //有没有对应的事件监听
        //有的情况
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as MyEventInfo).actions += action;
        }
        //没有的情况
        else
        {
            eventDic.Add(name, new MyEventInfo(action));
        }
    }


    /// <summary>
    /// 移除对应的事件监听
    /// </summary>
    /// <param name="name">事件的名字</param>
    /// <param name="action">对应之前添加的委托函数</param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as MyEventInfo<T>).actions -= action;
    }

    /// <summary>
    /// 移除不需要参数的事件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as MyEventInfo).actions -= action;
    }

    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="name">哪一个名字的事件触发了</param>
    public void EventTrigger<T>(string name, T info)
    {
        //有没有对应的事件监听
        //有的情况
        if (eventDic.ContainsKey(name))
        {
            //eventDic[name]();
            if ((eventDic[name] as MyEventInfo<T>).actions != null)
                (eventDic[name] as MyEventInfo<T>).actions.Invoke(info);
            //eventDic[name].Invoke(info);
        }
    }

    /// <summary>
    /// 事件触发（不需要参数的）
    /// </summary>
    /// <param name="name"></param>
    public void EventTrigger(string name)
    {
        //有没有对应的事件监听
        //有的情况
        if (eventDic.ContainsKey(name))
        {
            //eventDic[name]();
            if ((eventDic[name] as MyEventInfo).actions != null)
                (eventDic[name] as MyEventInfo).actions.Invoke();
            //eventDic[name].Invoke(info);
        }
    }




    public void AddEventComponent(GameObject gameObject, int eventID, UnityAction<object> callback)
    {
        EventTrigger eventTrigger;
        if (gameObject.GetComponent<EventTrigger>())
        {
            eventTrigger = gameObject.GetComponent<EventTrigger>();
        }
        else
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = (EventTriggerType)eventID;
        entry.callback.AddListener((target) =>
        {
            callback.Invoke(target);
        });
        // entry.callback.AddListener(callback);
        eventTrigger.triggers.Add(entry);
    }
    public void RemoveEventComponent(GameObject gameObject, int eventID, UnityAction<object> callback)
    {
        if (gameObject == null || callback == null)
        {
            return;
        }

        EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>();
        if (eventTrigger != null)
        {
            for (int i = 0; i < eventTrigger.triggers.Count; i++)
            {
                if (eventTrigger.triggers[i].eventID == (EventTriggerType)eventID)
                {
                    EventTrigger.Entry entry = eventTrigger.triggers[i];
                    entry.callback = null;
                    entry = null;
                    eventTrigger.triggers.RemoveAt(i);
                    return;
                }
            }
        }
    }


    /// <summary>
    /// 清空事件中心
    /// 主要用在 场景切换时
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
