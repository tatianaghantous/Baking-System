using System.ComponentModel.DataAnnotations;

namespace UAS.Persistence.Settings;

public class StorageSettings
{
    [Required] public string DefaultConnection { get; set; }
}