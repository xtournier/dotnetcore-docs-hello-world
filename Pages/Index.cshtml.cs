using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.InteropServices;
using Npgsql;
using System.Data;
using System.Collections.Generic;

namespace dotnetcoresample.Pages
{
    public class IndexModel : PageModel
    {
        public string OSVersion { get { return RuntimeInformation.OSDescription; } }
        public List<string> TableData { get; set; } = new List<string>();

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            ReadPostgresTableData();
        }

        private void ReadPostgresTableData()
        {
            var connString = "Host=monsrvbdd.postgres.database.azure.com;Username=pensivegoose1;Password=yicLzWtIZFpCruuVLUSynQ;Database=null";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT * FROM pg_catalog.pg_tables", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var rowData = new List<string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            rowData.Add(reader[i].ToString());
                        }
                        TableData.Add(string.Join(", ", rowData));
                    }
                }
            }
        }
    }
}
