using System;
using Gtk;
using Pango;
using System.Data;
using System.Data.OracleClient;
using static LibraryManagement.AppMenu;
using static LibraryManagement.DatabaseConnection;
using static LibraryManagement.BookTypeEdit;

namespace LibraryManagement {
  public class BookTypeShow: Window {

    private Fixed container;
    private Label header;
    private TreeStore treeStore;
    private TreeView treeView;
    private Button editButton;
    private Button deleteButton;
    private Button refreshButton;
    private DatabaseConnection db;
    private int selectedId;

    public BookTypeShow(): base("Book Type"){
      this.SetDefaultSize(1000, 800);
      this.DeleteEvent += new DeleteEventHandler(this.Exit);
      this.db = new DatabaseConnection();

      this.container = new Fixed();
      this.Add(this.container);
      new AppMenu(this, this.container);

      this.header = new Label("All book types");
      FontDescription fontDescription = new FontDescription();
      fontDescription.Weight = Weight.Bold;
      fontDescription.Size = 1024 * 20;
      this.header.ModifyFont(fontDescription);
      this.container.Put(this.header, 20, 30);

      this.treeStore = new TreeStore(typeof(string), typeof(string), typeof(string));
      this.FetchData();

      this.treeView = new TreeView();
      this.treeView.Model = this.treeStore;
      this.treeView.HeadersVisible = true;
      this.treeView.RowActivated += OnRowActivated;
      this.treeView.EnableGridLines = TreeViewGridLines.Both;

      this.treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
      this.treeView.AppendColumn("Name", new CellRendererText(), "text", 1);
      this.treeView.AppendColumn("Description", new CellRendererText(), "text", 2);

      ScrolledWindow sw = new ScrolledWindow();
      sw.Add(this.treeView);
      sw.WidthRequest = 800;
      sw.HeightRequest = 800;
      this.container.Put(sw, 0, 100);

      this.editButton = new Button("Edit");
      this.editButton.WidthRequest = 200;
      this.editButton.Sensitive = false;
      this.editButton.Clicked += new EventHandler(OnEditButtonClicked);
      this.container.Put(this.editButton, 850, 100);

      this.deleteButton = new Button("Delete");
      this.deleteButton.WidthRequest = 200;
      this.deleteButton.Sensitive = false;
      this.deleteButton.Clicked += new EventHandler(OnDeleteButtonClicked);
      this.container.Put(this.deleteButton, 850, 150);

      this.refreshButton = new Button("Refresh");
      this.refreshButton.WidthRequest = 200;
      this.refreshButton.Clicked += new EventHandler(OnRefreshButtonClicked);
      this.container.Put(this.refreshButton, 850, 200);

      this.ShowAll();
    }

    private void OnDeleteButtonClicked(object obj, EventArgs args){
      MessageDialog md = new MessageDialog(
        this,
        DialogFlags.DestroyWithParent,
        MessageType.Warning,
        ButtonsType.YesNo,
        "Are you sure yout want to delete this book type? After perfoming this action, this action will be deleted permanently"
      );

      ResponseType result = (ResponseType)md.Run();

      if(result == ResponseType.Yes){
        OracleConnection connection = this.db.GetConnection();
        string sql = "delete from tblBookType where bookTypeId = " + this.selectedId;

        try {
          connection.Open();
          OracleCommand command = new OracleCommand(sql, connection);
          command.ExecuteNonQuery();
          connection.Close();

          MessageDialog message = new MessageDialog(
            this,
            DialogFlags.DestroyWithParent,
            MessageType.Info,
            ButtonsType.Ok,
            "Delete a book type sucessfully"
          );

          message.Run();
          message.Destroy();
          this.Destroy();
          new BookTypeShow();
        }catch(Exception ex){
          MessageDialog error = new MessageDialog(
            this,
            DialogFlags.DestroyWithParent,
            MessageType.Error,
            ButtonsType.Ok,
            ex.Message
          );

          error.Run();
          error.Destroy();
        }
      }else{
        md.Destroy();
      }
    }

    private void OnEditButtonClicked(object obj, EventArgs args){
      this.Destroy();
      new BookTypeEdit(this.selectedId);
    }

    private void OnRowActivated(object obj, RowActivatedArgs args){
      TreeIter iter;
      TreeModel model = this.treeView.Model;
      model.GetIter(out iter, args.Path);
      this.selectedId = Int32.Parse((model.GetValue(iter, 0)).ToString());
      this.editButton.Sensitive = true;
      this.deleteButton.Sensitive = true;
    }

    private void FetchData(){
      OracleConnection connection = this.db.GetConnection();
      string sql = "select * from tblBookType";

      try {
        connection.Open();
        OracleCommand command = new OracleCommand(sql, connection);
        OracleDataReader reader = command.ExecuteReader();

        while(reader.Read()){
          this.treeStore.AppendValues(
            (reader["bookTypeId"]).ToString(),
            (reader["typeName"]).ToString(),
            (reader["description"]).ToString()
          );
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

    private void OnRefreshButtonClicked(object obj, EventArgs args){
      this.Destroy();
      new BookTypeShow();
    }

    private void Exit(object obj, DeleteEventArgs args){
      Application.Quit();
    }
  }
}