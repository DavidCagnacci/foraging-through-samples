using System;
using System.Collections.Generic;

public class TimerManager
{
    private static List<Timer> pendingTimers = new List<Timer>();
    private static List<Timer> activeTimers = new List<Timer>();
    private static List<Timer> expiredTimers = new List<Timer>();

    public static void Update(float deltaTime)
    {
        foreach (Timer pendingTimer in pendingTimers)
        {
            activeTimers.Add(pendingTimer);
        }

        pendingTimers.Clear();

        foreach (Timer activeTimer in activeTimers)
        {
            activeTimer.Update(deltaTime);

            if (activeTimer.IsExpired)
            {
                expiredTimers.Add(activeTimer);
            }
        }

        foreach (Timer expiredTimer in expiredTimers)
        {
            activeTimers.Remove(expiredTimer);
        }

        expiredTimers.Clear();
    }

    public static ITimer CreateTimer(float duration, bool repeating, Action onComplete)
    {
        Timer newTimer = new Timer(duration, repeating, onComplete);

        pendingTimers.Add(newTimer);

        return newTimer;
    }

    public static void ExpireAllTimers()
    {
        foreach (Timer pendingTimer in pendingTimers)
        {
            pendingTimer.Stop();
        }

        pendingTimers.Clear();

        foreach (Timer activeTimer in activeTimers)
        {
            activeTimer.Stop();
        }

        activeTimers.Clear();
    }

    private class Timer : ITimer
    {
        private float duration;
        private bool isRepeating;
        private Action onComplete;

        private float elapsedTime = 0f;
        private bool isExpired = false;
        private bool isPaused = false;

        public Timer(float duration, bool isRepeating, Action onComplete)
        {
            this.duration = duration;
            this.isRepeating = isRepeating;
            this.onComplete = onComplete;
        }

        public float Duration
        {
            get { return duration; }
        }

        public float ElapsedTime
        {
            get { return elapsedTime; }
        }

        public bool IsExpired
        {
            get { return isExpired; }
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Resume()
        {
            isPaused = false;
        }

        public void Stop()
        {
            isExpired = true;
        }

        public void Update(float deltaTime)
        {
            if (isExpired || isPaused)
            {
                return;
            }

            elapsedTime += deltaTime;

            if (elapsedTime >= duration)
            {
                onComplete();

                if (isRepeating)
                {
                    elapsedTime = 0;
                }
                else
                {
                    isExpired = true;
                }
            }
        }
    }
}

public interface ITimer
{
    float Duration { get; }
    float ElapsedTime { get; }
    bool IsExpired { get; }
    void Pause();
    void Resume();
    void Stop();
}
