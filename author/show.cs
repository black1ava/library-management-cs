using System;
using Gtk;
using Pango;
using System.Data;
using System.Data.OracleClient;
using static LibraryManagement.AppMenu;
using static LibraryManagement.DatabaseConnection;
using static LibraryManagement.AuthorEdit;
using static LibraryManagement.LibrarianAccount;

namespace LibraryManagement {
  public class AuthorShow: Window {

    private Fixed container;
    private Label header;
    private TreeView treeView;
    private TreeStore treeStore;
    private Button updateButton;
    private Button deleteButton;
    private Button refreshButton;
    private DatabaseConnection db;
    private int selectedId;

    public AuthorShow(): base("Librarians") {
      this.SetDefaultSize(1000, 800);
      this.DeleteEvent += new DeleteEventHandler(this.Exit);
      this.db = new DatabaseConnection();
      
      this.container = new Fixed();
      this.Add(this.container);

      new AppMenu(this, this.container);

      this.header = new Label("All authors");
      FontDescription fontDescription = new FontDescription();
      fontDescription.Weight = Weight.Bold;
      fontDescription.Size = 1024 * 20;
      this.header.ModifyFont(fontDescription);
      this.container.Put(this.header, 20, 30);

      this.treeStore = new TreeStore(
        typeof(string),
        typeof(string),
        typeof(string),
        typeof(string),
        typeof(string),
        typeof(string),
        typeof(string),
        typeof(string)
      );
      this.FetchData();

      this.treeView = new TreeView();
      this.treeView.Model = this.treeStore;
      this.treeView.HeadersVisible = true;
      this.treeView.RowActivated += OnTreeViewRowActivated;
      this.treeView.EnableGridLines = TreeViewGridLines.Both;

      this.treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
      this.treeView.AppendColumn("Gender", new CellRendererText(), "text", 1);
      this.treeView.AppendColumn("Name", new CellRendererText(), "text", 2);
      this.treeView.AppendColumn("Date of birth", new CellRendererText(), "text", 3);
      this.treeView.AppendColumn("Place of birth", new CellRendererText(), "text", 4);
      this.treeView.AppendColumn("Address", new CellRendererText(), "text", 5);
      this.treeView.AppendColumn("Phone", new CellRendererText(), "text", 6);
      this.treeView.AppendColumn("Email", new CellRendererText(), "text", 7);

      ScrolledWindow sw = new ScrolledWindow();
      sw.Add(this.treeView);
      sw.WidthRequest = 800;
      sw.HeightRequest = 800;
      this.container.Put(sw, 0, 100);

      this.updateButton = new Button("Edit");
      this.updateButton.WidthRequest = 200;
      this.updateButton.Sensitive = false;
      this.updateButton.Clicked += new EventHandler(this.OnUpdateButtonClicked);
      this.container.Put(this.updateButton, 850, 100);

      this.deleteButton = new Button("Delete");
      this.deleteButton.WidthRequest = 200;
      this.deleteButton.Sensitive = false;
      this.deleteButton.Clicked += new EventHandler(this.OnDeleteButtonClicked);
      this.container.Put(this.deleteButton, 850, 150);

      this.refreshButton = new Button("Refresh");
      this.refreshButton.WidthRequest = 200;
      this.refreshButton.Clicked += new EventHandler(this.OnRefreshButtonClicked);
      this.container.Put(this.refreshButton, 850, 200);

      this.ShowAll();
    }

    private void OnDeleteButtonClicked(object obj, EventArgs args){
      MessageDialog clarify = new MessageDialog(
        this,
        DialogFlags.DestroyWithParent,
        MessageType.Warning,
        ButtonsType.YesNo,
        "Are you sure you want to delete this author? After performing this action, this author will be deleted permanently."
      );

      ResponseType result = (ResponseType)clarify.Run();

      if(result == ResponseType.Yes){
        OracleConnection connection = this.db.GetConnection();
        string sql = "delete from tblAuthor where authorId = " + this.selectedId;

        try {
          connection.Open();
          OracleCommand command = new OracleCommand(sql, connection);
          command.ExecuteNonQuery();
          connection.Close();

          MessageDialog md = new MessageDialog(
            this,
            DialogFlags.DestroyWithParent,
            MessageType.Info,
            ButtonsType.Ok,
            "Delete an author successsfully"
          );

          md.Run();
          md.Destroy();

          this.Destroy();
          new AuthorShow();
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
      }else{
        clarify.Destroy();
      }
    }

    private void OnTreeViewRowActivated(object obj, RowActivatedArgs args){
      TreeIter iter;
      TreeModel model = this.treeView.Model;
      model.GetIter(out iter, args.Path);
      this.selectedId = Int32.Parse((model.GetValue(iter, 0)).ToString());
      this.updateButton.Sensitive = true;
      this.deleteButton.Sensitive = true;
    }

    private void FetchData(){
      OracleConnection connection = this.db.GetConnection();
      string sql = "select * from tblAuthor";

      try{
        connection.Open();
        OracleCommand command = new OracleCommand(sql, connection);
        OracleDataReader reader = command.ExecuteReader();

        while(reader.Read()){
          this.treeStore.AppendValues(
            (reader["authorId"]).ToString(),
            (reader["authorName"]).ToString(),
            (reader["gender"]).ToString(),
            (reader["dob"]).ToString(),
            (reader["pob"]).ToString(),
            (reader["address"]).ToString(),
            (reader["phone"]).ToString(),
            (reader["email"]).ToString()
          );
        }

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

    private void OnUpdateButtonClicked(object obj, EventArgs args){
      this.Destroy();
      new AuthorEdit(this.selectedId);
    }

    private void OnRefreshButtonClicked(object obj, EventArgs args){
      this.Destroy();
      new AuthorShow();
    }

    private void Exit(object obj, DeleteEventArgs args){
      Application.Quit();
    }
  }
}