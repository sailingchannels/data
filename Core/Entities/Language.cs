using System.Globalization;

namespace Core.Entities
{
    public record Language(string Code)
    {
        public string Name => new CultureInfo(Code).EnglishName;
    }
}
