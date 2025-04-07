
///Encryption Codes
//// Required namespaces
///
//using System;
//using System.IO;
//using System.IO.Compression;
//using System.Text;
//using Microsoft.Data.SqlClient;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.Hosting;

//string connectionString = @"ConnectionString";
//string jsonFilePath = @"D:\\ConsoleAppForJSON\\json\\CFR.json";

//if (args.Contains("--api"))
//{
//    var builder = WebApplication.CreateBuilder(args);
//    var app = builder.Build();



//    app.MapGet("/getdata", async () =>
//    {
//        var decompressedJson = DecompressDataFromDatabase(connectionString);
//        return Results.Ok(decompressedJson);
//    });

//    app.Run("http://localhost:5000");
//}
//else
//{
//    try
//    {


//        Console.WriteLine("🔃 Reading and compressing JSON file...");
//        string jsonContent = File.ReadAllText(jsonFilePath);
//        byte[] compressedData = CompressString(jsonContent);

//        Console.WriteLine("📦 Inserting compressed data into SQL Server...");
//        using (SqlConnection connection = new SqlConnection(connectionString))
//        {
//            connection.Open();
//            string sql = "INSERT INTO tblCFRjsondata (CompressedCaseDetails) VALUES (@CompressedJson)";
//            using (SqlCommand command = new SqlCommand(sql, connection))
//            {
//                command.Parameters.AddWithValue("@CompressedJson", compressedData);
//                command.ExecuteNonQuery();
//                Console.WriteLine("✅ Compressed JSON inserted successfully.");
//            }
//        }
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine("❌ Error while compressing or inserting: " + ex.Message);
//    }

//    Console.WriteLine("🧠 Fetching and decompressing latest row...");
//    string jsonPreview = DecompressDataFromDatabase(connectionString);
//    Console.WriteLine("\n🔍 Preview: " + jsonPreview.Substring(0, Math.Min(1000, jsonPreview.Length)));
//}

//// ------------------------ Utility Methods ------------------------
//static byte[] CompressString(string input)
//{
//    byte[] inputBytes = Encoding.UTF8.GetBytes(input);
//    using (MemoryStream output = new MemoryStream())
//    {
//        using (GZipStream gzip = new GZipStream(output, CompressionLevel.SmallestSize))
//        {
//            gzip.Write(inputBytes, 0, inputBytes.Length);
//        }
//        return output.ToArray();
//    }
//}

//static string DecompressGZip(byte[] compressedData)
//{
//    using (var inputStream = new MemoryStream(compressedData))
//    using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
//    using (var outputStream = new MemoryStream())
//    {
//        gzipStream.CopyTo(outputStream);
//        return Encoding.UTF8.GetString(outputStream.ToArray());
//    }
//}

//static string DecompressDataFromDatabase(string connectionString)
//{
//    try
//    {
//        using SqlConnection conn = new SqlConnection(connectionString);
//        conn.Open();

//        string query = "SELECT TOP 1 CompressedCaseDetails FROM tblCFRjsondata ORDER BY Id DESC";
//        using SqlCommand cmd = new SqlCommand(query, conn);
//        var result = cmd.ExecuteScalar();

//        if (result != DBNull.Value && result is byte[] compressedBytes)
//        {
//            return DecompressGZip(compressedBytes);
//        }
//        else
//        {
//            return "⚠️ No compressed data found in the table.";
//        }
//    }
//    catch (Exception ex)
//    {
//        return "❌ Error during decompression: " + ex.Message;
//    }
//}



// Decompression Code 

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting();

var app = builder.Build();

// Serve static HTML/JS/CSS from wwwroot folder (like React UI)
app.UseDefaultFiles();  // Serves index.html by default
app.UseStaticFiles();   // Serves JS, CSS, images, etc.

// ✅ API Endpoint - Decompress JSON from DB and return
app.MapGet("/getdata", async () =>
{
    string connectionString = @"ConnectionString";
    try
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            await conn.OpenAsync();

            string query = "SELECT TOP 1 CompressedCaseDetails FROM tblCFRjsondata ORDER BY Id DESC";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                var result = await cmd.ExecuteScalarAsync();

                if (result != DBNull.Value && result is byte[] compressedBytes)
                {
                    string json = DecompressGZip(compressedBytes);
                    return Results.Content(json, "application/json");
                }
            }
        }
        return Results.NotFound("⚠️ No compressed data found.");
    }
    catch (Exception ex)
    {
        return Results.Problem("❌ Error: " + ex.Message);
    }
});

// ✅ Start the app
app.Run("http://localhost:5000");


// 🧠 Reusable helper to decompress
static string DecompressGZip(byte[] compressedData)
{
    using (var inputStream = new MemoryStream(compressedData))
    using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
    using (var outputStream = new MemoryStream())
    {
        gzipStream.CopyTo(outputStream);
        return Encoding.UTF8.GetString(outputStream.ToArray());
    }
}
