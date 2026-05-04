using System.Data;
using Microsoft.Data.SqlClient;

namespace Car_Rental_System
{
    internal static class SqlCommandExtensions
    {
        public static SqlParameter AddInt(this SqlCommand command, string name, int value)
        {
            SqlParameter parameter = command.Parameters.Add(name, SqlDbType.Int);
            parameter.Value = value;
            return parameter;
        }

        public static SqlParameter AddDate(this SqlCommand command, string name, DateTime value)
        {
            SqlParameter parameter = command.Parameters.Add(name, SqlDbType.Date);
            parameter.Value = value.Date;
            return parameter;
        }

        public static SqlParameter AddMoney(this SqlCommand command, string name, decimal value)
        {
            SqlParameter parameter = command.Parameters.Add(name, SqlDbType.Decimal);
            parameter.Precision = AppConstants.MoneyPrecision;
            parameter.Scale = AppConstants.MoneyScale;
            parameter.Value = value;
            return parameter;
        }

        public static SqlParameter AddString(this SqlCommand command, string name, string value, int size)
        {
            SqlParameter parameter = command.Parameters.Add(name, SqlDbType.NVarChar, size);
            parameter.Value = value;
            return parameter;
        }

        public static SqlParameter AddNullableString(this SqlCommand command, string name, string? value, int size)
        {
            SqlParameter parameter = command.Parameters.Add(name, SqlDbType.NVarChar, size);
            parameter.Value = string.IsNullOrWhiteSpace(value) ? DBNull.Value : value.Trim();
            return parameter;
        }

        public static void AddSearchParameters(this SqlCommand command, string search)
        {
            string trimmedSearch = search.Trim();

            command.AddString("@Search", trimmedSearch, 200);
            command.AddString("@SearchText", $"%{trimmedSearch}%", 202);
        }
    }
}
