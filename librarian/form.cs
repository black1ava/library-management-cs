using System;
using Gtk;
using static LibraryManagement.Length;
using static LibraryManagement.DatabaseConnection;
using System.Data;
using System.Data.OracleClient;
using static LibraryManagement.LibrarianShow;

namespace LibraryManagement {
  public class LibrarianForm {

    private Label nameLabel;
    private Entry nameEntry;
    private Label genderLabel;
    private RadioButton genderMaleRadioButton;
    private RadioButton genderFemaleRadioButton;
    private Label dobLabel;
    private Entry dobEntry;
    private Label pobLabel;
    private Entry pobEntry;
    private Label addressLabel;
    private Entry addressEntry;
    private Label phoneLabel;
    private Entry phoneEntry;
    private Label emailLabel;
    private Entry emailEntry;
    private Label usernameLabel;
    private Entry usernameEntry;
    private Label passwordLabel;
    private Entry passwordEntry;
    private Label confirmPasswordLabel;
    private Entry confirmPassowrdEntry;
    private Fixed container;
    private DatabaseConnection db;
    private string selectedGender;
    private Window window;

    public LibrarianForm(Window window, Fixed container) {

      this.container = container;
      this.window = window;

      this.db = new DatabaseConnection();

      this.nameLabel = new Label("Name");
      this.container.Put(this.nameLabel, 20, 100);

      this.nameEntry = new Entry();
      this.nameEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.nameEntry, 240, 100);

      this.genderLabel = new Label("Gender");
      this.container.Put(this.genderLabel, 20, 150);

      this.genderMaleRadioButton = new RadioButton("Male");
      this.genderMaleRadioButton.Toggled += new EventHandler(this.OnGenderToggle);
      this.genderMaleRadioButton.Active = true;
      this.selectedGender = "M";
      this.container.Put(this.genderMaleRadioButton, 240, 150);

      this.genderFemaleRadioButton = new RadioButton(this.genderMaleRadioButton, "Female");
      this.container.Put(this.genderFemaleRadioButton, 320, 150);

      this.dobLabel = new Label("Date of birth (dd-mm-yyyy)");
      this.container.Put(this.dobLabel, 20, 200);

      this.dobEntry = new Entry();
      this.dobEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.dobEntry, 240, 200);

      this.pobLabel = new Label("Place of birth");
      this.container.Put(this.pobLabel, 20, 250);

      this.pobEntry = new Entry();
      this.pobEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.pobEntry, 240, 250);

      this.addressLabel = new Label("Address");
      this.container.Put(this.addressLabel, 20, 300);

      this.addressEntry = new Entry();
      this.addressEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.addressEntry, 240, 300);

      this.phoneLabel = new Label("Phone number");
      this.container.Put(this.phoneLabel, 20, 350);

      this.phoneEntry = new Entry();
      this.phoneEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.phoneEntry, 240, 350);

      this.emailLabel = new Label("Email");
      this.container.Put(this.emailLabel, 20, 400);

      this.emailEntry = new Entry();
      this.emailEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.emailEntry, 240, 400);

      this.usernameLabel = new Label("Username");
      this.container.Put(this.usernameLabel, 20, 450);

      this.usernameEntry = new Entry();
      this.usernameEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.usernameEntry, 240, 450);

      this.passwordLabel = new Label("Password");
      this.container.Put(this.passwordLabel, 20, 500);

      this.passwordEntry = new Entry();
      this.passwordEntry.WidthRequest = Length.EntryLength;
      this.passwordEntry.Visibility = false;
      this.container.Put(this.passwordEntry, 240, 500);

      this.confirmPasswordLabel = new Label("Confirm password");
      this.container.Put(this.confirmPasswordLabel, 20, 550);

      this.confirmPassowrdEntry = new Entry();
      this.confirmPassowrdEntry.WidthRequest = Length.EntryLength;
      this.confirmPassowrdEntry.Visibility = false;
      this.container.Put(this.confirmPassowrdEntry, 240, 550);
    }

    private void OnGenderToggle(object obj, EventArgs arg){
      if(this.genderMaleRadioButton.Active){
        this.selectedGender = "M";
      }else{
        this.selectedGender = "F";
      }
    }

    public void setUpButton(Button button, string status = "new"){
      this.container.Put(button, 240, 600);

      switch(status){
        case "new":
          button.Clicked += new EventHandler(onNewButtonClicked);
          break;
        case "update":
          button.Clicked += new EventHandler(onUpdateButtonClicked);
          break;
        default:
          break;
      }
    }

    private void onNewButtonClicked(object obj, EventArgs args){
      OracleConnection connection = this.db.GetConnection();

      try {
        if(this.passwordEntry.Text == this.confirmPassowrdEntry.Text){
          connection.Open();
          string sql = "insert into tblLibrarian(librarianName, gender, dob, pob, address, phone, email, username, userPassword) values('" + this.nameEntry.Text + "', '" + this.selectedGender + "', TO_DATE('" + this.dobEntry.Text + "', 'dd-mm-yyyy'), '" + this.pobEntry.Text + "', '" + this.addressEntry.Text + "', '" + this.phoneEntry.Text + "', '" + this.emailEntry.Text + "', '" + this.usernameEntry.Text + "', '" + this.passwordEntry.Text + "')";
          Console.WriteLine(sql);
          OracleCommand command = new OracleCommand(sql, connection);
          command.ExecuteNonQuery();
          connection.Close();

          MessageDialog md = new MessageDialog(this.window, 
            DialogFlags.DestroyWithParent,
            MessageType.Info,
            ButtonsType.Ok,
            "Create a new librarian successfully"
          );

          md.Run();
          md.Destroy();

          this.window.Destroy();
          new LibrarianShow();
        }else{
          MessageDialog md = new MessageDialog(
            this.window,
            DialogFlags.DestroyWithParent,
            MessageType.Error,
            ButtonsType.Ok,
            "Password is not matched"
          );

          md.Run();
          md.Destroy();
        }
      }catch(Exception ex){
        MessageDialog md = new MessageDialog(
          this.window,
          DialogFlags.DestroyWithParent,
          MessageType.Error,
          ButtonsType.Ok,
          "Create new librarian unsuccessfully. Please enter all of the fields"
        );

        md.Run();
        md.Destroy();
      }
    }

    private void onUpdateButtonClicked(object obj, EventArgs args){
      Console.WriteLine("update");
    }
  }
}