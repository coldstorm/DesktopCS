using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesktopCS.MVVM;

namespace DesktopCS.Models
{
    public class Topic : ObservableObject
    {
        private UserItem _author;

        public UserItem Author
        {
            get { return this._author; }
            set
            {
                this._author = value;
                this.OnPropertyChanged("Author");
            }
        }

        private string _content;

        public string Content
        {
            get { return this._content; }
            set
            {
                this._content = value;
                this.OnPropertyChanged("Content");
            }
        }

        private DateTime _authorDate;

        public DateTime AuthorDate
        {
            get { return this._authorDate; }
            set
            {
                this._authorDate = value;
                this.OnPropertyChanged("AuthorDate");
            }
        }
    }
}
