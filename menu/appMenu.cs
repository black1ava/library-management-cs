using System;
using Gtk;

namespace LibraryManagement {
  public class AppMenu {

    private MenuBar menuBar;
    private Menu fileMenu;
    private MenuItem file;
    private MenuItem exit;
    private Menu librarianMenu;
    private MenuItem librarian;
    private MenuItem newLibrarian;
    private MenuItem allLibrarians;

    public AppMenu(Window window, Fixed container){
      this.menuBar = new MenuBar();

      this.fileMenu = new Menu();
      this.file = new MenuItem("File");
      this.file.Submenu = this.fileMenu;
      this.menuBar.Append(this.file);

      this.exit = new MenuItem("Exit");
      this.exit.Activated += OnExitMenuActivated;
      this.fileMenu.Append(this.exit);

      this.librarianMenu = new Menu();
      this.librarian = new MenuItem("Librarian");
      this.librarian.Submenu = this.librarianMenu;
      this.menuBar.Append(this.librarian);

      this.newLibrarian = new MenuItem("New librarian");
      this.librarianMenu.Append(this.newLibrarian);

      this.allLibrarians = new MenuItem("All librarians");
      this.librarianMenu.Append(this.allLibrarians);

      container.Put(menuBar, 0, 0);
    }

    private void OnExitMenuActivated(object obj, EventArgs args){
      Application.Quit();
    }
  }
}