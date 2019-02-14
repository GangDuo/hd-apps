using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JFront.ViewModels
{
    class MainWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Models.FilterType _CategoryType;
        //private DateTime? _DeliveryDateFrom;

        public Models.FilterType CategoryType
        {
            get { return this._CategoryType; }
            set
            {
                this._CategoryType = value;
                this.NotifyChanged("CategoryType");
            }
        }

        //public DateTime? DeliveryDateFrom
        //{
        //    get { return this._DeliveryDateFrom; }
        //    set
        //    {
        //        this._DeliveryDateFrom = value;
        //        this.NotifyChanged("DeliveryDateFrom");
        //    }
        //}

        void NotifyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
