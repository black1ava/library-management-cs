#! bin/bash

mcs -pkg:gtk-sharp-2.0 main.cs account/login.cs length/length.cs home.cs menu/appMenu.cs && mono main.exe