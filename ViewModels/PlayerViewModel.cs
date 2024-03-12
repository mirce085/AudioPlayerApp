using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NAudio.Wave;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using AudioPlayerApp.Models;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using AudioPlayerApp.Services;
using CommunityToolkit.Mvvm.Messaging;
using AudioPlayerApp.Messages;

namespace AudioPlayerApp.ViewModels;


[INotifyPropertyChanged]
public partial class PlayerViewModel : BaseViewModel
{
    [ObservableProperty]
    private Uri? _source;

    [ObservableProperty]
    private double _sliderValue;

    [ObservableProperty]
    private string _mediaDuration;

    [ObservableProperty]
    private string _currentDuration;

    [ObservableProperty]
    private double _sliderMaxValue;

    private WaveOutEvent _waveOutEvent = null!;
    private AudioFileReader? _audioFileReader;

    private IPlayerState _playerState;

    public ObservableCollection<Media> MediaFiles { get; private set; }

    public PlayerViewModel()
    {
        _waveOutEvent = new WaveOutEvent();
        _playerState = new StoppedState(_waveOutEvent);

        WeakReferenceMessenger.Default.Register<ChangeStateMessage>(this, (sender, message) =>
        {
            _playerState = message.PlayerState;
        });

        WeakReferenceMessenger.Default.Register<StoppedStateLabelsMessage>(this, (sender, message) =>
        {
            SliderValue = 0;
            CurrentDuration = "0:00";
            MediaDuration = "0:00";
        });


        SliderMaxValue = 1;
        CurrentDuration = "0:00";
        MediaDuration = "0:00";
        SliderValue = 0;
        MediaFiles = new ObservableCollection<Media>();
    }



    private void TimerValueChanged(object? sender, EventArgs e)
    {
        if (_waveOutEvent != null && _audioFileReader != null && _waveOutEvent.PlaybackState == PlaybackState.Playing)
        {
            SliderValue = _audioFileReader.CurrentTime.TotalSeconds;
            CurrentDuration = $"{_audioFileReader.CurrentTime.Minutes}:{_audioFileReader.CurrentTime.Seconds % 60:D2}";
        }
    }

    [RelayCommand]
    void PlayPause(Media? file)
    {
        try
        {
            if(file == null)
            {
                MessageBox.Show("Select media file!");
                return;
            }

            if (_waveOutEvent.PlaybackState == PlaybackState.Playing)
            {
                _playerState.Pause();

                return;
            }
            else if (_waveOutEvent.PlaybackState == PlaybackState.Paused)
            {
                _playerState.Play();

                return;
            }

            _audioFileReader = new AudioFileReader(file.Path);
            _waveOutEvent.Init(_audioFileReader);
            _waveOutEvent.Play();
            _playerState = new PlayingState(_waveOutEvent);
            SliderMaxValue = _audioFileReader.TotalTime.TotalSeconds;
            MediaDuration = $"{_audioFileReader.TotalTime.Minutes}:{_audioFileReader.TotalTime.Seconds % 60:D2}";
            double intervalMilliseconds = Math.Max(100, _audioFileReader.TotalTime.TotalMilliseconds / 100);
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(intervalMilliseconds);
            timer.Tick += TimerValueChanged;
            timer.Start();
        }
        catch 
        {
            MessageBox.Show("Something went wrong!");
        }
    }

    [RelayCommand]
    public void Stop()
    {
        if(_waveOutEvent.PlaybackState == PlaybackState.Stopped)
        {
            return;
        }

        _playerState.Stop();
    }


    [RelayCommand]
    void RewindBack()
    {
        if (_waveOutEvent != null && _audioFileReader != null && _waveOutEvent.PlaybackState == PlaybackState.Playing)
        {
            TimeSpan rewindDuration = TimeSpan.FromSeconds(5);

            TimeSpan newPosition = _audioFileReader.CurrentTime - rewindDuration;

            if (newPosition.TotalSeconds < 0)
            {
                newPosition = TimeSpan.Zero;
            }

            _audioFileReader.CurrentTime = newPosition;
        }
    }

    [RelayCommand]
    void RewindFront()
    {
        if (_waveOutEvent != null && _audioFileReader != null && _waveOutEvent.PlaybackState == PlaybackState.Playing)
        {
            TimeSpan rewindDuration = TimeSpan.FromSeconds(5);

            TimeSpan newPosition = _audioFileReader.CurrentTime + rewindDuration;

            if (newPosition.TotalSeconds > _audioFileReader.TotalTime.Seconds)
            {
                newPosition = TimeSpan.FromSeconds(_audioFileReader.TotalTime.Seconds);
            }

            _audioFileReader.CurrentTime = newPosition;
        }
    }

    private void GetAllFiles(string path)
    {
        if (!Directory.Exists(path))
        {
            return;
        }

        var files = Directory.GetFiles(path);
        foreach (var file in files)
        {
            if(Path.GetExtension(file) == ".mp3" || Path.GetExtension(file) == ".mp4")
            {
                var media = new Media();
                media.Path = file;
                var arr = file.Split("\\");
                media.Title = arr[arr.Length - 1];
                MediaFiles.Add(media);
            }
        }
    }

    [RelayCommand]
    void SelectAlbum()
    {
        var fileDialog = new FolderBrowserDialog();


        if (fileDialog.ShowDialog() == DialogResult.OK)
        {
            GetAllFiles(fileDialog.SelectedPath);
        }
    }
}