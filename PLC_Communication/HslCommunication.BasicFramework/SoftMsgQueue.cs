using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace HslCommunication.BasicFramework;

public class SoftMsgQueue<T> : SoftFileSaveBase
{
	private Queue<T> all_items = new Queue<T>();

	private int m_Max_Cache = 200;

	private object lock_queue = new object();

	public int MaxCache
	{
		get
		{
			return m_Max_Cache;
		}
		set
		{
			if (value > 10)
			{
				m_Max_Cache = value;
			}
		}
	}

	public T CurrentItem
	{
		get
		{
			if (all_items.Count > 0)
			{
				return all_items.Peek();
			}
			return default(T);
		}
	}

	public SoftMsgQueue()
	{
		base.LogHeaderText = "SoftMsgQueue<" + typeof(T).ToString() + ">";
	}

	public void AddNewItem(T item)
	{
		lock (lock_queue)
		{
			while (all_items.Count >= m_Max_Cache)
			{
				all_items.Dequeue();
			}
			all_items.Enqueue(item);
		}
	}

	public override string ToSaveString()
	{
		return JArray.FromObject(all_items).ToString();
	}

	public override void LoadByString(string content)
	{
		JArray jArray = JArray.Parse(content);
		all_items = (Queue<T>)jArray.ToObject(typeof(Queue<T>));
	}
}
