using System.ComponentModel;

namespace Domain.Enums;

public enum FilmAudioOption
{
    [Description("Legendado")]
    OriginalWithSubtitles = 1,

    [Description("Dublado")]
    Dubbed = 2
}