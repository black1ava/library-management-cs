using System;
using Gtk;
using static LibraryManagement.Length;
using static LibraryManagement.DatabaseConnection;

namespace LibraryManagement {
  public class LibrarianForm {

    private Label nameLabel;
    private Entry nameEntry;
    private Label genderLabel;
    private RadioButton genderMaleRadioButton;
    private RadioButton genderFemaleRadioButton;
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
    private Fixed container;

    public LibrarianForm(Fixed container) {

      this.container = container;

      new DatabaseConnection();

      this.nameLabel = new Label("Name");
      this.container.Put(this.nameLabel, 20, 100);

      this.nameEntry = new Entry();
      this.nameEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.nameEntry, 240, 100);

      this.genderLabel = new Label("Gender");
      this.container.Put(this.genderLabel, 20, 150);

      this.genderMaleRadioButton = new RadioButton("Male");
      this.container.Put(this.genderMaleRadioButton, 240, 150);

      this.genderFemaleRadioButton = new RadioButton(this.genderMaleRadioButton, "Female");
      this.container.Put(this.genderFemaleRadioButton, 320, 150);

      this.dobLabel = new Label("Date of birth (dd-mm-yyyy)");
      this.container.Put(this.dobLabel, 20, 200);

      this.dobEntry = new Entry();
      this.dobEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.dobEntry, 240, 200);

      this.pobLabel = new Label("Place of birth");
      this.container.Put(this.pobLabel, 20, 250);

      this.pobEntry = new Entry();
      this.pobEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.pobEntry, 240, 250);

      this.addressLabel = new Label("Address");
      this.container.Put(this.addressLabel, 20, 300);

      this.addressEntry = new Entry();
      this.addressEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.addressEntry, 240, 300);

      this.phoneLabel = new Label("Phone number");
      this.container.Put(this.phoneLabel, 20, 350);

      this.phoneEntry = new Entry();
      this.phoneEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.phoneEntry, 240, 350);

      this.emailLabel = new Label("Email");
      this.container.Put(this.emailLabel, 20, 400);

      this.emailEntry = new Entry();
      this.emailEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.emailEntry, 240, 400);
    }

    public void setUpButton(Button button, string status = "new"){
      this.container.Put(button, 240, 450);

      switch(status){
        case "new":
          button.Clicked += new EventHandler(onNewButtonClicked);
          break;
        case "update":
          button.Clicked += new EventHandler(onUpdateButtonClicked);
          break;
        default:
          break;
      }
    }

    private void onNewButtonClicked(object obj, EventArgs args){
      Console.WriteLine("new");
    }

    private void onUpdateButtonClicked(object obj, EventArgs args){
      Console.WriteLine("update");
    }
  }
}