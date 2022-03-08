using System;
using System.Data;
using System.Data.OracleClient;

namespace LibraryManagement {
  public class DatabaseConnection{

    private OracleConnection connection;

    public DatabaseConnection(){
      string connectionString = "Data source=koompios:1521/xepdb1; User ID=group1; Password=123456";

      try {
        this.connection = new OracleConnection(connectionString);
        Console.WriteLine("Connect to database successfully");
      }catch (Exception ex){
        Console.WriteLine("error: {0}", ex.Message);
      }
    }
  }
}