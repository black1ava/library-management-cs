using System;
using Gtk;
using System.Data;
using System.Data.OracleClient;
using static LibraryManagement.AppMenu;
using static LibraryManagement.AuthorForm;
using static LibraryManagement.DatabaseConnection;

namespace LibraryManagement {
  public class AuthorEdit: Window {

    private Fixed container;
    private AuthorForm form;
    private DatabaseConnection db;
    private int selectedId;

    public AuthorEdit(int selectedId): base("Edit author"){
      this.selectedId = selectedId;
      this.SetDefaultSize(800, 800);
      this.DeleteEvent += new DeleteEventHandler(this.Exit);

      this.db = new DatabaseConnection();

      this.container = new Fixed();
      this.Add(this.container);

      new AppMenu(this, this.container);
      this.form = new AuthorForm(this, this.container, "update");
      this.FetchData();

      this.ShowAll();
    }

    private void FetchData(){
      OracleConnection connection = this.db.GetConnection();
      string sql = "select * from tblAuthor where authorId = " + this.selectedId;

      try {
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