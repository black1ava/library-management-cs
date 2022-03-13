using System;
using Gtk;
using System.Data;
using System.Data.OracleClient;
using static LibraryManagement.Length;
using static LibraryManagement.DatabaseConnection;
using static LibraryManagement.BookTypeShow;

namespace LibraryManagement {
  public class BookTypeForm {

    private Label nameLabel;
    private Entry nameEntry;
    private Label descriptionLabel;
    private Entry descriptionEntry;
    private Window window;
    private Fixed container;
    private Button createButton;
    private Button updateButton;
    private DatabaseConnection db;
    private int selectedId;

    public BookTypeForm(Window window, Fixed container, string status = "new"){
      this.window = window;
      this.container = container;
      this.db = new DatabaseConnection();

      this.nameLabel = new Label("Name");
      this.container.Put(this.nameLabel, 20, 100);

      this.nameEntry = new Entry();
      this.nameEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.nameEntry, 240, 100);

      this.descriptionLabel = new Label("Description");
      this.container.Put(this.descriptionLabel, 20, 150);

      this.descriptionEntry = new Entry();
      this.descriptionEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.descriptionEntry, 240, 150);

      if(status == "new"){
        this.createButton = new Button("Create");
        this.createButton.WidthRequest = Length.ButtonLength;
        this.createButton.Clicked += new EventHandler(this.OnCreateButtonClicked);
        this.container.Put(this.createButton, 240, 200);
      }else{
        this.updateButton = new Button("Update");
        this.updateButton.WidthRequest = Length.ButtonLength;
        this.updateButton.Clicked += new EventHandler(this.OnUpdateButtonClicked);
        this.container.Put(this.updateButton, 240, 200);
      }
    }

    private void OnUpdateButtonClicked(object obj, EventArgs args){
      OracleConnection connection = this.db.GetConnection();
      string sql = "update tblBookType set typeName = '" + this.nameEntry.Text + "', description = '" + this.descriptionEntry.Text + "' where bookTypeId = " + this.selectedId;

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
          "Update a book type successfully"
        );

        md.Run();
        md.Destroy();
        this.window.Destroy();

        new BookTypeShow();
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
      this.selectedId = Int32.Parse((reader["bookTypeId"]).ToString());
      this.nameEntry.Text = (reader["typeName"]).ToString();
      this.descriptionEntry.Text = (reader["description"]).ToString();
    }

    private void OnCreateButtonClicked(object obj, EventArgs args){
      OracleConnection connection = this.db.GetConnection();
      string sql = "insert into tblBookType(typeName, description) values('" + this.nameEntry.Text + "', '" + this.descriptionEntry.Text + "')";
      
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
          "Create a new book type successfully"
        );

        md.Run();
        md.Destroy();

        new BookTypeShow();
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