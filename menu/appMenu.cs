using System;
using Gtk;
using static LibraryManagement.NewLibrarian;
using static LibraryManagement.LibrarianShow;

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
    private Window window;

    public AppMenu(Window window, Fixed container){

      this.window = window;

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
      this.newLibrarian.Activated += OnNewLibrarianActivated;
      this.librarianMenu.Append(this.newLibrarian);

      this.allLibrarians = new MenuItem("All librarians");
      this.allLibrarians.Activated += OnAllLibrariansActivated;
      this.librarianMenu.Append(this.allLibrarians);

      container.Put(menuBar, 0, 0);
    }

    private void OnAllLibrariansActivated(object obj, EventArgs args){
      this.window.Destroy();
      new LibrarianShow();
    }

    private void OnNewLibrarianActivated(object obj, EventArgs args){
      this.window.Destroy();
      new NewLibrarian();
    }

    private void OnExitMenuActivated(object obj, EventArgs args){
      Application.Quit();
    }
  }
}