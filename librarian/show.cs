using System;
using Gtk;
using static LibraryManagement.AppMenu;
using System.Data;
using System.Data.OracleClient;
using static LibraryManagement.DatabaseConnection;
using Pango;
using static LibraryManagement.Length;

namespace LibraryManagement {
  public class LibrarianShow: Window {

    private Fixed container;
    private TreeView treeview;
    private TreeStore treestore;
    private ScrolledWindow sw;
    private DatabaseConnection db;
    private Label header;
    private int selectedId;
    private Button editButton;
    private Button refreshButton;
    private Button deleteButton;

    public LibrarianShow(): base("Librarians") {
      this.SetDefaultSize(1000, 800);
      this.DeleteEvent += new DeleteEventHandler(this.Exit);

      this.db = new DatabaseConnection();

      this.container = new Fixed();
      this.Add(this.container);
      new AppMenu(this, this.container);

      this.header = new Label("Librarians");
      FontDescription fontDescription = new FontDescription();
      fontDescription.Weight = Weight.Bold;
      fontDescription.Size = 20 * 1024;
      this.header.ModifyFont(fontDescription);
      this.container.Put(this.header, 20, 30);

      this.treestore = new TreeStore(
        typeof(string),
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

      this.treeview = new TreeView();
      this.treeview.HeadersVisible = true;
      this.treeview.Model = this.treestore;
      this.treeview.RowActivated += OnRowActivated;

      this.treeview.AppendColumn("ID", new CellRendererText(), "text", 0);
      this.treeview.AppendColumn("Name", new CellRendererText(), "text", 1);
      this.treeview.AppendColumn("Gender", new CellRendererText(), "text", 2);
      this.treeview.AppendColumn("Date of birth", new CellRendererText(), "text", 3);
      this.treeview.AppendColumn("Place of birth", new CellRendererText(), "text", 4);
      this.treeview.AppendColumn("Address", new CellRendererText(), "text", 5);
      this.treeview.AppendColumn("Phone", new CellRendererText(), "text", 6);
      this.treeview.AppendColumn("Email", new CellRendererText(), "text", 7);
      this.treeview.AppendColumn("Username", new CellRendererText(), "text", 8);

      this.sw = new ScrolledWindow();
      this.sw.WidthRequest = 800;
      this.sw.HeightRequest = 800;
      this.sw.Add(this.treeview);

      this.container.Put(this.sw, 0, 100);


      this.editButton = new Button("Edit");
      this.editButton.WidthRequest = 200;
      this.editButton.Sensitive = false;
      this.container.Put(this.editButton, 850, 100);

      this.deleteButton = new Button("Delete");
      this.deleteButton.WidthRequest= 200;
      this.deleteButton.Sensitive = false;
      this.container.Put(this.deleteButton, 850, 150);

      this.refreshButton = new Button("Refresh");
      this.refreshButton.WidthRequest = 200;
      this.refreshButton.Clicked += new EventHandler(this.OnRefreshButtonClicked);
      this.container.Put(this.refreshButton, 850, 200);

      this.ShowAll();
    }

    private void OnRefreshButtonClicked(object obj, EventArgs args){
      this.Destroy();
      new LibrarianShow();
    }

    private void OnRowActivated(object obj, RowActivatedArgs args){
      TreeIter iter;
      var model = treeview.Model;
      model.GetIter(out iter, args.Path);
      int id = Int32.Parse((model.GetValue(iter, 0)).ToString());
      this.selectedId = id;
      this.editButton.Sensitive = true;
      this.deleteButton.Sensitive = true;
    }

    private void FetchData(){
      OracleConnection connection = this.db.GetConnection();

      try {
        connection.Open();
        string sql = "select * from tblLibrarian";
        OracleCommand command = new OracleCommand(sql, connection);
        OracleDataReader reader = command.ExecuteReader();
        
        while(reader.Read()){
          this.treestore.AppendValues(
            (reader["librarianId"]).ToString(),
            (reader["librarianName"]).ToString(),
            (reader["gender"]).ToString(),
            (reader["dob"]).ToString(),
            (reader["pob"]).ToString(),
            (reader["address"]).ToString(),
            (reader["phone"]).ToString(),
            (reader["email"]).ToString(),
            (reader["username"]).ToString()
          );
        }

        connection.Close();
      }catch(Exception ex){
        Console.WriteLine(ex.Message);
      }
    }

    private void Exit(object obj, DeleteEventArgs args){
      Application.Quit();
    }
  }
}