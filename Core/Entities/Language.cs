using System.Globalization;

namespace Core.Entities
{
    public record Language(string Code)
    {
        public string Name
        {
            get
            {
                var cultureInfo = new CultureInfo(Code);
                return cultureInfo.EnglishName;
            }
        }
    }
}
