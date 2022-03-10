using System;
using Gtk;
using Pango;
using static LibraryManagement.Length;
using static LibraryManagement.Home;
using static LibraryManagement.DatabaseConnection;
using System.Data;
using System.Data.OracleClient;
using static LibraryManagement.LibrarianAccount;

namespace LibraryManagement {
  public class Login: Window {

    private Fixed container;
    private Label header;
    private Label usernameLabel;
    private Entry usernameEntry;
    private Label passwordLabel;
    private Entry passwordEntry;
    private Button loginButton;
    private Button emergencyLoginButton;
    private DatabaseConnection db;

    public Login(): base("Login") {
      this.SetDefaultSize(800, 800);

      this.DeleteEvent += new DeleteEventHandler(this.Exit);

      this.db = new DatabaseConnection();

      this.container = new Fixed();
      this.Add(this.container);

      this.header = new Label("Login");
      FontDescription fontBold = new FontDescription();
      fontBold.Weight = Weight.Bold;
      fontBold.Size = 20 * 1024;
      this.header.ModifyFont(fontBold);
      this.container.Put(this.header, 350, 300);

      this.usernameLabel = new Label("Username");
      this.container.Put(this.usernameLabel, 200, 350);

      this.usernameEntry = new Entry();
      this.usernameEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.usernameEntry, 300, 350);

      this.passwordLabel = new Label("Password");
      this.container.Put(this.passwordLabel, 200, 400);

      this.passwordEntry = new Entry();
      this.passwordEntry.Visibility = false;
      this.passwordEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.passwordEntry, 300, 400);

      this.loginButton = new Button("Login");
      this.loginButton.WidthRequest = Length.ButtonLength;
      this.loginButton.Clicked += new EventHandler(this.OnLoginButtonClicked);
      this.container.Put(this.loginButton, 300, 450);

      this.emergencyLoginButton = new Button("Emergency login");
      this.emergencyLoginButton.WidthRequest = Length.ButtonLength;
      this.emergencyLoginButton.Clicked += new EventHandler(this.EmergencyButtonClicked);
      this.container.Put(this.emergencyLoginButton, 300, 500);

      this.ShowAll();
    }

    private void OnLoginButtonClicked(object obj, EventArgs args){
      OracleConnection connection = this.db.GetConnection();

      try {
        connection.Open();
        string sql = "select * from tblLibrarian where username = '" + this.usernameEntry.Text + "' and userPassword = '" + this.passwordEntry.Text + "'";
        OracleCommand command = new OracleCommand(sql, connection);
        OracleDataReader reader = command.ExecuteReader();
        bool userExist = reader.Read();
        LibrarianAccount.LibrarianId = Int32.Parse((reader["librarianId"]).ToString());
        
        MessageDialog md;

        if(userExist){
          md = new MessageDialog(
            this,
            DialogFlags.DestroyWithParent,
            MessageType.Info,
            ButtonsType.Ok,
            "Login successfully"
          );

          md.Run();
          md.Destroy();

          this.Destroy();
          new Home();
        }else{
          md = new MessageDialog(
            this,
            DialogFlags.DestroyWithParent,
            MessageType.Error,
            ButtonsType.Ok,
            "Invalid username or password"
          );

          md.Run();
          md.Destroy();
        }

      }catch(Exception ex){
        MessageDialog md = new MessageDialog(
          this,
          DialogFlags.DestroyWithParent,
          MessageType.Error,
          ButtonsType.Ok,
          ex.Message
        );

        md.Run();
        md.Destroy();
      }
    }

    private void EmergencyButtonClicked(object obj, EventArgs args){
      this.Destroy();
      new Home();
    }

    private void Exit(object obj, DeleteEventArgs args){
      Application.Quit();
    }
  }
}