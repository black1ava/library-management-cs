using System;
using Gtk;
using Pango;
using static LibraryManagement.Length;
using static LibraryManagement.Home;

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

    public Login(): base("Login") {
      this.SetDefaultSize(800, 800);

      this.DeleteEvent += new DeleteEventHandler(this.Exit);

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
      this.container.Put(this.loginButton, 300, 450);

      this.emergencyLoginButton = new Button("Emergency login");
      this.emergencyLoginButton.WidthRequest = Length.ButtonLength;
      this.emergencyLoginButton.Clicked += new EventHandler(this.EmergencyButtonClicked);
      this.container.Put(this.emergencyLoginButton, 300, 500);

      this.ShowAll();
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