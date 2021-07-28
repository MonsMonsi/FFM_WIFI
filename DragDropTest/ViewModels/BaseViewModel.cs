using System.ComponentModel;

namespace FFM_WIFI.ViewModels
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(
            // Der Parameter der Methode soll der Name eines Properties sein, das verändert wird
            string propName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propName);
                PropertyChanged(this, args);
            }
        }
    }
}
