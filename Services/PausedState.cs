using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AudioPlayerApp.Messages;
using CommunityToolkit.Mvvm.Messaging;
using NAudio.Wave;

namespace AudioPlayerApp.Services;

public class PausedState : IPlayerState
{
    private WaveOutEvent _waveOutEvent;

    public PausedState(WaveOutEvent waveOutEvent)
    {
        _waveOutEvent = waveOutEvent;
    }

    public void Pause()
    {
        
    }

    public void Play()
    {
        _waveOutEvent.Play();

        var message = new ChangeStateMessage(new PlayingState(_waveOutEvent));
        WeakReferenceMessenger.Default.Send(message);
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
