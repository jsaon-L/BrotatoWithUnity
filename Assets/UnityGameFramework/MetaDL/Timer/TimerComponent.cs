using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using Skyunion;


public class TimerComponent : GameFrameworkComponent
{

    /// <summary>
    /// Register a new timer that should fire an event after a certain amount of time
    /// has elapsed.
    ///
    /// Registered timers are destroyed when the scene changes.
    /// </summary>
    /// <param name="duration">The time to wait before the timer should fire, in seconds.</param>
    /// <param name="onComplete">An action to fire when the timer completes.</param>
    /// <param name="onUpdate">An action that should fire each time the timer is updated. Takes the amount
    /// of time passed in seconds since the start of the timer's current loop.</param>
    /// <param name="isLooped">Whether the timer should repeat after executing.</param>
    /// <param name="useRealTime">Whether the timer uses real-time(i.e. not affected by pauses,
    /// slow/fast motion) or game-time(will be affected by pauses and slow/fast-motion).</param>
    /// <param name="autoDestroyOwner">An object to attach this timer to. After the object is destroyed,
    /// the timer will expire and not execute. This allows you to avoid annoying <see cref="NullReferenceException"/>s
    /// by preventing the timer from running and accessessing its parents' components
    /// after the parent has been destroyed.</param>
    /// <returns>A timer object that allows you to examine stats and stop/resume progress.</returns>

    public Timer Register(float duration, Action onComplete, Action<float> onUpdate = null,
        bool isLooped = false, bool useRealTime = false, MonoBehaviour autoDestroyOwner = null)
    {
        return Timer.Register(duration, onComplete, onUpdate, isLooped, useRealTime, autoDestroyOwner);
    }
    public void Cancel(Timer timer)
    {
        Timer.Cancel(timer);
    }
    public void CancelAllRegisteredTimers()
    {
        Timer.CancelAllRegisteredTimers();
    }

    private void OnDestroy()
    {
        CancelAllRegisteredTimers();
    }
}
