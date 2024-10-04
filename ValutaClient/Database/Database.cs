using Microsoft.Data.SqlClient;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValutaClient.Database
{
    public class Database
    {
        //static readonly string connectionString = @"Server=localhost,1433;User=sa;Password=S3cr3tP4sSw0rd;Persist Security Info=False;Trusted_Connection=False;Encrypt=False;TrustServerCertificate=True;";
        static readonly string connectionString = "Server=localhost,1433;User Id=sa;Password=S3cr3tP4sSw0rd#123;Encrypt=false;TrustServerCertificate=True;";

        public Database()
        {
            SetupDatabase();
        }

        private void SetupDatabase()
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

        public IList<ExchangeRate> LoadData(string currencyCode)
        {
            List<ExchangeRate> result = new();

            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                string cs = $"SELECT * FROM ExchangeRate where CurrencyCode = '{currencyCode}' ORDER BY ID DESC";
                SqlCommand command = new SqlCommand(cs, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string currency = reader["CurrencyCode"].ToString();
                    decimal value = Convert.ToDecimal(reader["Value"]);
                    DateTime timestamp = Convert.ToDateTime(reader["Timestamp"]);

                    result.Add(new ExchangeRate() { CurrencyCode = currency, Value = value, Timestamp = timestamp });
                }

            }
            catch (Exception ex) 
            {
                Log.Error($"LoadData: currencyCode={currencyCode}: {ex.Message}");
            }

            return result;
        }

        public void SaveData(IList<ExchangeRate> data)
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
                command.ExecuteNonQuery();

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Log.Error($"SaveData: {ex.Message}");
            }

        }
    }
}
