using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

/* Wir wollen folgendes Ersetzen:
 * //ergebnisTextBox.Text = rechner.Ergebnis.ToString();
 * Grund: da wir zwischen dem ViewModel und der View nur Databinding haben und keinen Aufruf von Methoden
 * => loose Kopplung ist erwünscht, weil wir dann eine bessere Wiederverwendung und Testkeit des Codes haben
 * 
 * Am Beginn des Programmes wird das RechnerViewModel als Objekt instanzsiert
 * Danach wird der Datacontext des Windows mit dem Objekt verbunden
 * this.DataContext = rechner; im Mainwindow.xaml.cs
 * 
 * Das Mainwindow sucht nach einem event-Property PropertyChanged
 * => wenn das Objekt so ein Property hat, dann registriert das Mainwindow eine Methode bei dem Event
 * 
 * 1) In der Oberfläche drückt ein User auf den Button "Add"
 * 2) addButton_Click() wird aufgerufen
 * 3) rechner.Addieren() wird aufgerufen
 * 4) beim Addieren änderns sich der Wert von Ergebnis
 * 5) die set- Methode des Properties Ergebnis wird aufgerufen
 * 6) OnPropertyChanged("Ergebnis")
 * 7) Event PropertyChanged wird aufgerufen
*/

namespace Test.ViewModels
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Der Parameter der Methode soll der Name eines Properties sein, das verändert wird
        protected virtual void OnPropertyChanged(string propName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propName);
                PropertyChanged(this, args);
            }
        }
            
    }
}
