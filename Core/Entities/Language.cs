using System.Globalization;

namespace Core.Entities
{
    public class Language
    {
        public string Code { get; set; }
        public string Name
        {
            get
            {
                return new CultureInfo(Code).EnglishName;
            }
        }

        public Language(string code)
        {
            Code = code;
        }
    }
}
