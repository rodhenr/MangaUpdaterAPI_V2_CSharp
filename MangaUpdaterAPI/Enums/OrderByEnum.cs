using System.Runtime.Serialization;

namespace MangaUpdater.Domain.Enums;

public enum OrderByEnum{
    [EnumMember(Value = "alphabet")]
    Alphabetical,
    [EnumMember(Value = "latest")]
    Latest,
}
