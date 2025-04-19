using System.ComponentModel.DataAnnotations;

namespace TS.Persistence.Settings;

public class StorageSettings
{
    [Required] public string DefaultConnection { get; set; }
}