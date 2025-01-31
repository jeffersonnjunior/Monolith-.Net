using System.ComponentModel;

namespace Domain.Enums;

public enum FilmAudioOption
{
    [Description("Legendado")]
    OriginalWithSubtitles,

    [Description("Dublado")]
    Dubbed
}