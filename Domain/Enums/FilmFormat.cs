using System.ComponentModel;

namespace Domain.Enums;

public enum FilmFormat
{
    [Description("2D")]
    D2 = 1,
    
    [Description("3D")]
    D3 = 2,
    
    [Description("VIP")]
    Vip = 3
}
