using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set; }

    public event EventHandler OnStateChanged;
    public enum State
    {
        WaitingToStart,
        Countdown,
        GamePlaying,
        GameOver,
    }

    private State state;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 120f;

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;   
    }

    private void Update()
    {
        switch (state) { 
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0f)
                {
                    gamePlayingTimer = gamePlayingTimerMax;
                    state = State.Countdown;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Countdown:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);

                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);

                }
                break;
            case State.GameOver:
                break;

        }
        Debug.Log(state);
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }
    public bool IsCountdownToStartActive()
    {
        return state == State.Countdown;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }
    public bool IsGameOver()
    {
        return state == State.GameOver;
    }
    public float GetPlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }
}
