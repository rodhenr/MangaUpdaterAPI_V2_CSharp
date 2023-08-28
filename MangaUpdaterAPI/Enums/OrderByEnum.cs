using System.Runtime.Serialization;

namespace MangaUpdater.API.Enums;

public enum OrderByEnum
{
    [EnumMember(Value = "alphabet")] Alphabetical,
    [EnumMember(Value = "latest")] Latest,
}