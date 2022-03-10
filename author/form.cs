using System;
using Gtk;
using static LibraryManagement.Length;

namespace LibraryManagement {
  public class AuthorForm {

    private Label nameLabel;
    private Entry nameEntry;
    private Window window;
    private Fixed container;
    private Label genderLabel;
    private RadioButton maleGenderRadioButton;
    private RadioButton femaleGenderRadioButton;
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
    private Button createButton;
    private Button updateButton;

    public AuthorForm(Window window, Fixed container, string status = "new"){
      this.window = window;
      this.container = container;

      this.nameLabel = new Label("Name");
      this.container.Put(this.nameLabel, 20, 100);
      
      this.nameEntry = new Entry();
      this.nameEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.nameEntry, 240, 100);

      this.genderLabel = new Label("Gender");
      this.container.Put(this.genderLabel, 20, 150);

      this.maleGenderRadioButton = new RadioButton("Male");
      this.maleGenderRadioButton.Active = true;
      this.container.Put(this.maleGenderRadioButton, 240, 150);

      this.femaleGenderRadioButton = new RadioButton(this.maleGenderRadioButton, "Female");
      this.container.Put(this.femaleGenderRadioButton, 320, 150);

      this.dobLabel = new Label("Date of birth (dd-mm-yy)");
      this.container.Put(this.dobLabel, 20, 200);

      this.dobEntry = new Entry();
      this.dobEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.dobEntry, 240, 200);

      this.pobLabel = new Label("Place of bith");
      this.container.Put(this.pobLabel, 20, 250);

      this.pobEntry = new Entry();
      this.pobEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.pobEntry, 240, 250);

      this.addressLabel = new Label("Address");
      this.container.Put(this.addressLabel, 20, 300);

      this.addressEntry = new Entry();
      this.addressEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.addressEntry, 240, 300);

      this.phoneLabel = new Label("Phone");
      this.container.Put(this.phoneLabel, 20, 350);

      this.phoneEntry = new Entry();
      this.phoneEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.phoneEntry, 240, 350);

      this.emailLabel = new Label("Email");
      this.container.Put(this.emailLabel, 20, 400);

      this.emailEntry = new Entry();
      this.emailEntry.WidthRequest = Length.EntryLength;
      this.container.Put(this.emailEntry, 240, 400);

      if(status == "new"){
        this.createButton = new Button("Create");
        this.createButton.WidthRequest = Length.ButtonLength;
        this.container.Put(this.createButton, 240, 450);
      }else{
        this.updateButton = new Button("Update");
        this.updateButton.WidthRequest = Length.ButtonLength;
        this.container.Put(this.updateButton, 240, 450);
      }
    }
  }
}