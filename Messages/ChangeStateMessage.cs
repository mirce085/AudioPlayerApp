using AudioPlayerApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayerApp.Messages;

public class ChangeStateMessage : Message
{
    public ChangeStateMessage(IPlayerState playerState)
    {
        PlayerState = playerState;
    }

    public IPlayerState PlayerState { get; set; }
}
