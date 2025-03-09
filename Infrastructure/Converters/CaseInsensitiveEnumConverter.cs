using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System;

namespace AuchanTest.Infrastructure.Converters
{
    public class CaseInsensitiveEnumConverter<T> : DefaultTypeConverter where T : struct, Enum
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (Enum.TryParse<T>(text.Replace("-", ""), true, out var result))
            {
                return result;
            }

            throw new TypeConverterException(this, memberMapData, text, row.Context, $"The conversion cannot be performed. Text: '{text}'");
        }
    }
}
