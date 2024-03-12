﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayerApp.Services;

public interface IPlayerState
{
    void Play();
    void Pause();
    void Stop();
}
