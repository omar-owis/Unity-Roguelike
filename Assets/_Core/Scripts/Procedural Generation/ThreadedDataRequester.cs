using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace DungeonMan.Terrain
{
    public class ThreadedDataRequester : MonoBehaviour
    {
        static ThreadedDataRequester instance;
        Queue<ThreadInfo> dataQueue = new Queue<ThreadInfo>();

        private void Awake()
        {
            instance = FindObjectOfType<ThreadedDataRequester>();
        }

        public static void RequestData(Action<object> callback, Func<object> generateData)
        {
            ThreadStart threadStart = delegate { instance.DataThread(callback, generateData); };
            new Thread(threadStart).Start();
        }

        void DataThread(Action<object> callback, Func<object> generateData)
        {
            object data = generateData();
            lock (dataQueue)
            {
                dataQueue.Enqueue(new ThreadInfo(callback, data));
            }

        }

        private void Update()
        {
            if (dataQueue.Count > 0)
            {
                for (int i = 0; i < dataQueue.Count; i++)
                {
                    ThreadInfo threadInfo = dataQueue.Dequeue();
                    threadInfo.callback(threadInfo.parameter);
                }
            }
        }
        struct ThreadInfo
        {
            public readonly Action<object> callback;
            public readonly object parameter;

            public ThreadInfo(Action<object> _callback, object _parameter)
            {
                callback = _callback;
                parameter = _parameter;
            }
        }
    }
}