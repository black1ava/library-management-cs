using System;
using Gtk;
using static LibraryManagement.Login;

namespace LibraryManagement {
  public class MainClass {
    public static void Main(string[] args){
      Application.Init();
      new Login();
      Application.Run();
    }
  }
}