using System;
using Gtk;
using System.Data;
using System.Data.OracleClient;
using static LibraryManagement.Length;
using static LibraryManagement.DatabaseConnection;

namespace LibraryManagement {
  public class AuthorForm {

    private Label nameLabel;
    private Entry nameEntry;
    private Window window;
    private Fixed container;
    private Label genderLabel;
    private RadioButton maleGenderRadioButton;
    private RadioButton femaleGenderRadioButton;
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
    private Button createButton;
    private Button updateButton;
    private DatabaseConnection db;
    private string selectedGender;
    private int selectedId;

    public AuthorForm(Window window, Fixed container, string status = "new"){
      this.window = window;
      this.container = container;
      this.db = new DatabaseConnection();

      this.nameLabel = new Label("Name");
      this.container.Put(this.nameLabel, 20, 100);
      
      this.nameEntry = new Entry();
      this.nameEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.nameEntry, 240, 100);

      this.genderLabel = new Label("Gender");
      this.container.Put(this.genderLabel, 20, 150);

      this.maleGenderRadioButton = new RadioButton("Male");
      this.maleGenderRadioButton.Active = true;
      this.selectedGender = "M";
      this.maleGenderRadioButton.Toggled += new EventHandler(this.OnMaleGenderRadioButtonToggled);
      this.container.Put(this.maleGenderRadioButton, 240, 150);

      this.femaleGenderRadioButton = new RadioButton(this.maleGenderRadioButton, "Female");
      this.container.Put(this.femaleGenderRadioButton, 320, 150);

      this.dobLabel = new Label("Date of birth (dd-mm-yy)");
      this.container.Put(this.dobLabel, 20, 200);

      this.dobEntry = new Entry();
      this.dobEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.dobEntry, 240, 200);

      this.pobLabel = new Label("Place of bith");
      this.container.Put(this.pobLabel, 20, 250);

      this.pobEntry = new Entry();
      this.pobEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.pobEntry, 240, 250);

      this.addressLabel = new Label("Address");
      this.container.Put(this.addressLabel, 20, 300);

      this.addressEntry = new Entry();
      this.addressEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.addressEntry, 240, 300);

      this.phoneLabel = new Label("Phone");
      this.container.Put(this.phoneLabel, 20, 350);

      this.phoneEntry = new Entry();
      this.phoneEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.phoneEntry, 240, 350);

      this.emailLabel = new Label("Email");
      this.container.Put(this.emailLabel, 20, 400);

      this.emailEntry = new Entry();
      this.emailEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.emailEntry, 240, 400);

      if(status == "new"){
        this.createButton = new Button("Create");
        this.createButton.WidthRequest = Length.ButtonLength;
        this.createButton.Clicked += new EventHandler(this.OnCreateButtonClicked);
        this.container.Put(this.createButton, 240, 450);
      }else{
        this.updateButton = new Button("Update");
        this.updateButton.WidthRequest = Length.ButtonLength;
        this.updateButton.Clicked += new EventHandler(this.OnUpdateButtonClicked);
        this.container.Put(this.updateButton, 240, 450);
      }
    }

    private void OnUpdateButtonClicked(object obj, EventArgs args){
      OracleConnection connection = this.db.GetConnection();
      string sql  = "update tblAuthor set authorName = '" + this.nameEntry.Text + "', gender = '" + this.selectedGender + "', dob = TO_DATE('" + this.dobEntry.Text + "', 'dd/mm/yyyy HH:MI:SS AM'), pob = '" + this.pobEntry.Text + "', address = '" + this.addressEntry.Text + "', phone = '" + this.phoneEntry.Text + "', email = '" + this.emailEntry.Text + "' where authorId = " + this.selectedId;
      
      try {
        connection.Open();
        OracleCommand command = new OracleCommand(sql, connection);
        command.ExecuteNonQuery();
        connection.Close();

        MessageDialog md = new MessageDialog(
          this.window,
          DialogFlags.DestroyWithParent,
          MessageType.Info,
          ButtonsType.Ok,
          "Update an author successfully"
        );

        md.Run();
        md.Destroy();
        this.window.Destroy();
        new AuthorShow();
      }catch(Exception ex){
        MessageDialog md = new MessageDialog(
          this.window,
          DialogFlags.DestroyWithParent,
          MessageType.Error,
          ButtonsType.Ok,
          ex.Message
        );

        md.Run();
        md.Destroy();
      }
    }

    public void InsertExistingData(OracleDataReader reader){
      this.selectedId = Int32.Parse((reader["authorId"]).ToString());
      this.nameEntry.Text = (reader["authorName"]).ToString();

      if((reader["gender"]).ToString() == "M"){
        this.maleGenderRadioButton.Active = true;
      }else{
        this.femaleGenderRadioButton.Active = true;
      }

      this.dobEntry.Text = (reader["dob"]).ToString();
      this.pobEntry.Text = (reader["pob"]).ToString();
      this.addressEntry.Text = (reader["address"]).ToString();
      this.phoneEntry.Text = (reader["phone"]).ToString();
      this.emailEntry.Text = (reader["email"]).ToString();
    }

    private void OnMaleGenderRadioButtonToggled(object obj, EventArgs args){
      if(this.maleGenderRadioButton.Active){
        this.selectedGender = "M";
      }else{
        this.selectedGender = "F";
      }
    }

    private void OnCreateButtonClicked(object obj, EventArgs args){
      OracleConnection connection = this.db.GetConnection();
      string sql = "insert into tblAuthor(authorName, gender, dob, pob, address, phone, email) values('" + this.nameEntry.Text + "', '" + this.selectedGender + "', TO_DATE('" + this.dobEntry.Text + "', 'DD-MM-YYYY'), '" + this.pobEntry.Text + "', '" + this.addressEntry.Text + "', '" + this.phoneEntry.Text + "', '" + this.emailEntry.Text + "')";
      
      try {
        connection.Open();
        OracleCommand command = new OracleCommand(sql, connection);
        command.ExecuteNonQuery();

        connection.Close();

        MessageDialog md = new MessageDialog(
          this.window,
          DialogFlags.DestroyWithParent,
          MessageType.Info,
          ButtonsType.Ok,
          "Create a new author successfully"
        );

        md.Run();
        md.Destroy();

        this.window.Destroy();

        new AuthorShow();
      }catch(Exception ex){
        MessageDialog md = new MessageDialog(
          this.window,
          DialogFlags.DestroyWithParent,
          MessageType.Error,
          ButtonsType.Ok,
          ex.Message
        );

        md.Run();
        md.Destroy();
      }
    }
  }
}