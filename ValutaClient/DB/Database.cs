using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValutaClient.DB
{
    public sealed class Database : DbContext
    {
        //static readonly string connectionString = @"Server=localhost,1433;User=sa;Password=S3cr3tP4sSw0rd;Persist Security Info=False;Trusted_Connection=False;Encrypt=False;TrustServerCertificate=True;";
        private static readonly string connectionString = "Server=localhost,1433;User Id=sa;Password=S3cr3tP4sSw0rd#123;Encrypt=false;TrustServerCertificate=True;";

        static Database()
        {
            SetupDatabase();
        }

        private static void SetupDatabase()
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                //string cs = "DROP TABLE ExchangeRate";
                string cs = "IF OBJECT_ID(N'dbo.ExchangeRate', N'U') IS NULL BEGIN   CREATE TABLE dbo.ExchangeRate (Id INT IDENTITY(1,1), CurrencyCode varchar(3) not null, Value decimal(18,5) not null, Timestamp DateTime not null); END;";
                SqlCommand command = new SqlCommand(cs, sqlConnection);
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Log.Error($"SetupDatabase: {ex.Message}");
            }
        }

        public static async Task<ExchangeRate?> LoadData(string currencyCode)
        {
            ExchangeRate? result = null;

            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                string cs = $"SELECT TOP 1 1 FROM ExchangeRate where CurrencyCode = '{currencyCode}' ORDER BY ID DESC";
                SqlCommand command = new SqlCommand(cs, sqlConnection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    string currency = reader["CurrencyCode"].ToString();
                    decimal value = Convert.ToDecimal(reader["Value"]);
                    DateTime timestamp = Convert.ToDateTime(reader["Timestamp"]);

                    result = new ExchangeRate() { CurrencyCode = currency, Value = value, Timestamp = timestamp };
                }
                sqlConnection.Close();
            }
            catch (Exception ex) 
            {
                Log.Error($"LoadData: currencyCode={currencyCode}: {ex.Message}");
            }

            return result;
        }

        public static async Task<List<ExchangeRate>> GetSupportedCurrencies()
        {
            List<ExchangeRate?> result = new();
            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                string cs = $"SELECT DISTINCT CurrencyCode FROM ExchangeRate";
                SqlCommand command = new SqlCommand(cs, sqlConnection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    string currency = reader["CurrencyCode"].ToString();
                    decimal value = Convert.ToDecimal(reader["Value"]);
                    DateTime timestamp = Convert.ToDateTime(reader["Timestamp"]);

                    if (!String.IsNullOrEmpty(currency))
                    {
                        result.Add(new ExchangeRate() { CurrencyCode = currency, Value = value, Timestamp = timestamp });
                    }
                }
                sqlConnection.Close();

            }
            catch (Exception ex)
            {
                Log.Error($"GetSupportedCurrencies: {ex.Message}");
            }

            return result;
        }

            public static async Task SaveData(IList<ExchangeRate> data)
        {
            if (data.Count == 0)
            {
                return;
            }

            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                StringBuilder sb = new StringBuilder("INSERT INTO ExchangeRate (CurrencyCode, Value, Timestamp) VALUES ");
                foreach (var item in data)
                {
                    sb.Append($"('{item.CurrencyCode}', {item.Value}, GETDATE()),");
                }
                sb.Length--;
                sb.Append(";");

                SqlCommand command = new SqlCommand(sb.ToString(), sqlConnection);
                await command.ExecuteNonQueryAsync();

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Log.Error($"SaveData: {ex.Message}");
            }
        }
    }
}
