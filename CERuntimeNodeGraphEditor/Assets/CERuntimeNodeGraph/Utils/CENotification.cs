/*
 * http://wiki.unity3d.com/index.php?title=Advanced_CSharp_Messenger
 * 
 * Advanced C# messenger by Ilya Suzdalnitski. V1.0
 * 
 * Based on Rod Hyde's "CSharpMessenger" and Magnus Wolffelt's "CSharpMessenger Extended".
 * 
 * Features:
 * Prevents a MissingReferenceException because of a reference to a destroyed message handler.
 * Option to log all messages
 * Extensive error detection, preventing silent bugs
 * 
 * Usage examples:
 *	1. Messenger.AddListener<GameObject>("prop collected", PropCollected);
 *	   Messenger.Broadcast<GameObject>("prop collected", prop);
 *	2. Messenger.AddListener<float>("speed changed", SpeedChanged);
 *	   Messenger.Broadcast<float>("speed changed", 0.5f);
 * 
 * Messenger cleans up its evenTable automatically upon loading of a new oldLevel.
 * 
 * Don't forget that the messages that should survive the cleanup, should be marked with Messenger.MarkAsPermanent(string)
 * 
 * 修改部分:
 *
 *	默认去除 define REQUIRE_LISTENER 及 没有Listener的情况下不会抛异常
 *	修改类名为CEMessenger 和 CEMessengerCallback
 * 
 */

//#define LOG_ALL_MESSAGES
//#define LOG_ADD_LISTENER
//#define LOG_BROADCAST_MESSAGE
//#define REQUIRE_LISTENER

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.CopyEngine.Core.Notification
{
    public class CENotification
    {
        public delegate void CENotificationCallback();

        public delegate void CENotificationCallback<T>(T arg1);

        public delegate void CENotificationCallback<T, U>(T arg1, U arg2);

        public delegate void CENotificationCallback<T, U, V>(T arg1, U arg2, V arg3);

        #region Internal variables

        public Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

        //Message handlers that should never be removed, regardless of calling Cleanup
        public List<string> permanentMessages = new List<string>();

        public MonoBehaviour mb;

        #endregion

        #region Helper methods

        //Marks a certain message as permanent.
        public void MarkAsPermanent(string eventType)
        {
#if LOG_ALL_MESSAGES
		Debug.Log("Messenger MarkAsPermanent \t\"" + eventType + "\"");
#endif

            permanentMessages.Add(eventType);
        }

        public void Cleanup()
        {
#if LOG_ALL_MESSAGES
		Debug.Log("MESSENGER Cleanup. Make sure that none of necessary listeners are removed.");
#endif

            List<string> messagesToRemove = new List<string>();

            foreach (KeyValuePair<string, Delegate> pair in eventTable)
            {
                bool wasFound = false;

                foreach (string message in permanentMessages)
                {
                    if (pair.Key == message)
                    {
                        wasFound = true;
                        break;
                    }
                }

                if (!wasFound)
                    messagesToRemove.Add(pair.Key);
            }

            foreach (string message in messagesToRemove)
            {
                eventTable.Remove(message);
            }
        }

        public void PrintEventTable()
        {
            Debug.Log("\t\t\t=== MESSENGER PrintEventTable ===");

            foreach (KeyValuePair<string, Delegate> pair in eventTable)
            {
                Debug.Log("\t\t\t" + pair.Key + "\t\t" + pair.Value);
            }

            Debug.Log("\n");
        }

        #endregion

        #region Message logging and exception throwing

        public void OnListenerAdding(string eventType, Delegate listenerBeingAdded)
        {
#if LOG_ALL_MESSAGES || LOG_ADD_LISTENER
		Debug.Log("MESSENGER OnListenerAdding \t\"" + eventType + "\"\t{" + listenerBeingAdded.Target + " -> " + listenerBeingAdded.Method + "}");
#endif

            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
            }

            Delegate d = eventTable[eventType];
            if (d != null && d.GetType() != listenerBeingAdded.GetType())
            {
                throw new ListenerException(string.Format(
                    "Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name,
                    listenerBeingAdded.GetType().Name));
            }
        }

        public void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved)
        {
#if LOG_ALL_MESSAGES
		Debug.Log("MESSENGER OnListenerRemoving \t\"" + eventType + "\"\t{" + listenerBeingRemoved.Target + " -> " + listenerBeingRemoved.Method + "}");
#endif

            if (eventTable.ContainsKey(eventType))
            {
                Delegate d = eventTable[eventType];

                if (d == null)
                {
                    throw new ListenerException(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
                }
                else if (d.GetType() != listenerBeingRemoved.GetType())
                {
                    throw new ListenerException(string.Format(
                        "Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType,
                        d.GetType().Name, listenerBeingRemoved.GetType().Name));
                }
            }
            else { }
        }

        public void OnListenerRemoved(string eventType)
        {
            if (eventTable[eventType] == null)
            {
                eventTable.Remove(eventType);
            }
        }

        public void OnBroadcasting(string eventType)
        {
#if REQUIRE_LISTENER
		if (!eventTable.ContainsKey (eventType)) {
			throw new BroadcastException (string.Format ("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));
		}
#endif
        }

        public BroadcastException CreateBroadcastSignatureException(string eventType) { return new BroadcastException(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType)); }

        public class BroadcastException : Exception
        {
            public BroadcastException(string msg)
                : base(msg) { }
        }

        public class ListenerException : Exception
        {
            public ListenerException(string msg)
                : base(msg) { }
        }

        #endregion

        #region AddListener

        //No parameters
        public virtual void AddListener(string eventType, CENotificationCallback handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (CENotificationCallback) eventTable[eventType] + handler;
        }

        //Single parameter
        public virtual void AddListener<T>(string eventType, CENotificationCallback<T> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (CENotificationCallback<T>) eventTable[eventType] + handler;
        }

        //Two parameters
        public virtual void AddListener<T, U>(string eventType, CENotificationCallback<T, U> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (CENotificationCallback<T, U>) eventTable[eventType] + handler;
        }

        //Three parameters
        public virtual void AddListener<T, U, V>(string eventType, CENotificationCallback<T, U, V> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (CENotificationCallback<T, U, V>) eventTable[eventType] + handler;
        }

        #endregion

        #region RemoveListener

        //No parameters
        public virtual void RemoveListener(string eventType, CENotificationCallback handler)
        {
            if (eventTable.ContainsKey(eventType))
            {
                OnListenerRemoving(eventType, handler);
                eventTable[eventType] = (CENotificationCallback) eventTable[eventType] - handler;
                OnListenerRemoved(eventType);
            }
        }

        //Single parameter
        public virtual void RemoveListener<T>(string eventType, CENotificationCallback<T> handler)
        {
            if (eventTable.ContainsKey(eventType))
            {
                OnListenerRemoving(eventType, handler);
                eventTable[eventType] = (CENotificationCallback<T>) eventTable[eventType] - handler;
                OnListenerRemoved(eventType);
            }
        }

        //Two parameters
        public virtual void RemoveListener<T, U>(string eventType, CENotificationCallback<T, U> handler)
        {
            if (eventTable.ContainsKey(eventType))
            {
                OnListenerRemoving(eventType, handler);
                eventTable[eventType] = (CENotificationCallback<T, U>) eventTable[eventType] - handler;
                OnListenerRemoved(eventType);
            }
        }

        //Three parameters
        public virtual void RemoveListener<T, U, V>(string eventType, CENotificationCallback<T, U, V> handler)
        {
            if (eventTable.ContainsKey(eventType))
            {
                OnListenerRemoving(eventType, handler);
                eventTable[eventType] = (CENotificationCallback<T, U, V>) eventTable[eventType] - handler;
                OnListenerRemoved(eventType);
            }
        }

        #endregion

        #region Broadcast

        //No parameters
        public virtual void Broadcast(string eventType)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                CENotificationCallback callback = d as CENotificationCallback;

                if (callback != null)
                {
                    callback();
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        //Single parameter
        public virtual void Broadcast<T>(string eventType, T arg1)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                CENotificationCallback<T> callback = d as CENotificationCallback<T>;

                if (callback != null)
                {
                    callback(arg1);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        //Two parameters
        public virtual void Broadcast<T, U>(string eventType, T arg1, U arg2)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                CENotificationCallback<T, U> callback = d as CENotificationCallback<T, U>;

                if (callback != null)
                {
                    callback(arg1, arg2);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        //Three parameters
        public virtual void Broadcast<T, U, V>(string eventType, T arg1, U arg2, V arg3)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                CENotificationCallback<T, U, V> callback = d as CENotificationCallback<T, U, V>;

                if (callback != null)
                {
                    callback(arg1, arg2, arg3);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        #endregion


        #region LaterNotify(下一帧再Notify,防止一帧内多次发送Notify导致UI多次刷新)

        private List<string> mRemainLaterBroadcastEventList = new List<string>();

        public virtual void LaterBroadcast(string eventType)
        {
            if (!mRemainLaterBroadcastEventList.Contains(eventType))
            {
                mRemainLaterBroadcastEventList.Add(eventType);
                mb.StartCoroutine(DoLaterBroadcast(eventType));
            }
        }

        private IEnumerator DoLaterBroadcast(string eventType)
        {
            yield return new WaitForEndOfFrame();
            Broadcast(eventType);
            mRemainLaterBroadcastEventList.Remove(eventType);
        }


        public virtual void LaterBroadcast<T>(string eventType, T arg1)
        {
            if (!mRemainLaterBroadcastEventList.Contains(eventType))
            {
                mRemainLaterBroadcastEventList.Add(eventType);
                mb.StartCoroutine(DoLaterBroadcast<T>(eventType, arg1));
            }
        }

        private IEnumerator DoLaterBroadcast<T>(string eventType, T arg1)
        {
            yield return new WaitForEndOfFrame();
            Broadcast<T>(eventType, arg1);
            mRemainLaterBroadcastEventList.Remove(eventType);
        }

        public virtual void LaterBroadcast<T, U>(string eventType, T arg1, U arg2)
        {
            if (!mRemainLaterBroadcastEventList.Contains(eventType))
            {
                mRemainLaterBroadcastEventList.Add(eventType);
                mb.StartCoroutine(DoLaterBroadcast<T, U>(eventType, arg1, arg2));
            }
        }

        private IEnumerator DoLaterBroadcast<T, U>(string eventType, T arg1, U arg2)
        {
            yield return new WaitForEndOfFrame();
            Broadcast<T, U>(eventType, arg1, arg2);
            mRemainLaterBroadcastEventList.Remove(eventType);
        }

        public virtual void LaterBroadcast<T, U, V>(string eventType, T arg1, U arg2, V arg3)
        {
            if (!mRemainLaterBroadcastEventList.Contains(eventType))
            {
                mRemainLaterBroadcastEventList.Add(eventType);
                mb.StartCoroutine(DoLaterBroadcast<T, U, V>(eventType, arg1, arg2, arg3));
            }
        }

        private IEnumerator DoLaterBroadcast<T, U, V>(string eventType, T arg1, U arg2, V arg3)
        {
            yield return new WaitForEndOfFrame();
            Broadcast<T, U, V>(eventType, arg1, arg2, arg3);
            mRemainLaterBroadcastEventList.Remove(eventType);
        }

        #endregion
    }
}