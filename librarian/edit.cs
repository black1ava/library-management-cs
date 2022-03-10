using System;
using Gtk;
using System.Data;
using System.Data.OracleClient;
using static LibraryManagement.AppMenu;
using static LibraryManagement.LibrarianForm;
using static LibraryManagement.DatabaseConnection;
using static LibraryManagement.Length;

namespace LibraryManagement {
  public class LibrarianEdit: Window {

    private Fixed container;
    private int librarainId;
    private DatabaseConnection db;
    private LibrarianForm form;
    private Button updateButton;

    public LibrarianEdit(int librarainId): base("Edit Librarian"){

      this.librarainId = librarainId;
      this.db = new DatabaseConnection();

      this.SetDefaultSize(800, 800);
      this.DeleteEvent += new DeleteEventHandler(this.Exit);

      this.container = new Fixed();
      this.Add(this.container);

      new AppMenu(this, this.container);
      this.form = new LibrarianForm(this, this.container);
      this.form.RemovePasswordSection();
      this.FetchData();

      this.updateButton = new Button("Update");
      this.updateButton.WidthRequest = Length.ButtonLength;
      this.form.setUpButton(this.updateButton, "update");

      this.ShowAll();
    }

    private void FetchData(){
      OracleConnection connection = this.db.GetConnection();
      string sql = "select librarianId, librarianName, gender, dob, pob, address, phone, email, username from tblLibrarian where librarianId = " + this.librarainId;

      try{
        connection.Open();
        OracleCommand command = new OracleCommand(sql, connection);
        OracleDataReader reader = command.ExecuteReader();
        reader.Read();
        this.form.InsertExistingData(reader);

        connection.Close();
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

    private void Exit(object obj, DeleteEventArgs args){
      Application.Quit();
    }
  }
}