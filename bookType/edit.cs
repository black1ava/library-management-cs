using System;
using Gtk;
using Pango;
using System.Data;
using System.Data.OracleClient;
using static LibraryManagement.AppMenu;
using static LibraryManagement.BookTypeForm;
using static LibraryManagement.DatabaseConnection;

namespace LibraryManagement {
  public class BookTypeEdit: Window {

    private Fixed container;
    private Label header;
    private BookTypeForm form;
    private int selectedId;
    private DatabaseConnection db;

    public BookTypeEdit(int selectedId): base("Book type edit") {
      this.SetDefaultSize(800, 800);
      this.DeleteEvent += new DeleteEventHandler(this.Exit);
      this.selectedId = selectedId;
      this.db = new DatabaseConnection();

      this.container = new Fixed();
      this.Add(this.container);

      this.header = new Label("Edit book type");
      FontDescription fontDescription = new FontDescription();
      fontDescription.Weight = Weight.Bold;
      fontDescription.Size = 20 * 1024;
      this.header.ModifyFont(fontDescription);
      this.container.Put(this.header, 20, 30);

      new AppMenu(this, this.container);

      this.form = new BookTypeForm(this, this.container, "update");
      this.FetchData();

      this.ShowAll();
    }

    private void FetchData(){
      OracleConnection connection = this.db.GetConnection();
      string sql = "select * from tblBookType where bookTypeId = " + this.selectedId;

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