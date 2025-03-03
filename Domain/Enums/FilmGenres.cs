using System.ComponentModel;

namespace Domain.Enums;

public enum FilmGenres
{
    [Description("Ação")]
    Action = 1,
    
    [Description("Aventura")]
    Adventure = 2,
    
    [Description("Animação")]
    Animation = 3,
    
    [Description("Biografia")]
    Biography = 4,
    
    [Description("Comédia")]
    Comedy = 5,
    
    [Description("Crime")]
    Crime = 6,
    
    [Description("Documentário")]
    Documentary = 7,
    
    [Description("Drama")]
    Drama = 8,
    
    [Description("Família")]
    Family = 9,
    
    [Description("Fantasia")]
    Fantasy = 10,
    
    [Description("Histórico")]
    History = 11,
    
    [Description("Terror")]
    Horror = 12,
    
    [Description("Musical")]
    Musical = 13,
    
    [Description("Mistério")]
    Mystery = 14,
    
    [Description("Romance")]
    Romance = 15,
    
    [Description("Ficção Científica")]
    ScienceFiction = 16,
    
    [Description("Esporte")]
    Sports = 17,
    
    [Description("Suspense")]
    Thriller = 18,
    
    [Description("Guerra")]
    War = 19,
    
    [Description("Faroeste")]
    Western = 20
}