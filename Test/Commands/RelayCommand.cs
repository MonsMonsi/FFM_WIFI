using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Test.Commands
{
    // Command hat 2 Eigenschaften:
    // 1) delegate für eine Methode: hier wird eine Methode aus dem ViewModel registriert, die beim Aufruf des Commands aufgerufen werden soll
    // 2) boolsche Eigenschaft, ob ein Button aktiviert oder nicht aktiviert ist
    class RelayCommand : ICommand
    {
        // Attribute
        private Action m_execute; // s. 1)
        private Func<bool> m_canExecute; // s. 2)

        public event EventHandler CanExecuteChanged;

        // Konstruktoren:
        // Konstruktor für Buttons, die immer aktiv sind
        public RelayCommand(Action execute)
        {
            // Abspeichern einer Methode im delegate
            m_execute = execute;
            m_canExecute = null;
        }

        // Konstruktor für Buttons, die eine aktiv/nicht aktiv Logik haben
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            // Abspeichern einer Methode im delegate
            m_execute = execute;
            m_canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (m_canExecute == null)
                return true;
            else
                return m_canExecute();
        }

        public void Execute(object parameter)
        {
            // Aufruf des, mittels Konstruktor registrierten, delegates
            m_execute();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
