using AudioPlayerApp.Messages;
using CommunityToolkit.Mvvm.Messaging;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayerApp.Services;

public class StoppedState : IPlayerState
{
    private WaveOutEvent _waveOutEvent;

    public StoppedState(WaveOutEvent waveOutEvent)
    {
        _waveOutEvent = waveOutEvent;
    }

    public void Pause()
    {
        _waveOutEvent.Pause();

        var message = new ChangeStateMessage(new PausedState(_waveOutEvent));

        WeakReferenceMessenger.Default.Send(message);
    }

    public void Play()
    {
        _waveOutEvent.Play();

        var message = new ChangeStateMessage(new PlayingState(_waveOutEvent));
        WeakReferenceMessenger.Default.Send(message);
    }

    public void Stop()
    {
        
    }
}
