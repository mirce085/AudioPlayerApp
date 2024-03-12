using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioPlayerApp.Messages;
using CommunityToolkit.Mvvm.Messaging;
using NAudio.Wave;

namespace AudioPlayerApp.Services;

public class PlayingState : IPlayerState
{
    private WaveOutEvent _waveOutEvent;

    public PlayingState(WaveOutEvent waveOutEvent)
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
        
    }

    public void Stop()
    {
        _waveOutEvent.Stop();

        var message = new ChangeStateMessage(new StoppedState(_waveOutEvent));
        WeakReferenceMessenger.Default.Send(message);

        var message2 = new StoppedStateLabelsMessage();
        WeakReferenceMessenger.Default.Send(message2);
    }
}
