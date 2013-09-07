using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace DesktopCS.MVVM
{
    public class CompositeCollection<T> : IEnumerable<T>, INotifyCollectionChanged
    {
        private readonly ObservableCollection<T>[] _subCollections;

        public CompositeCollection(params ObservableCollection<T>[] subCollections)
        {
            _subCollections = subCollections;
            SubscribeToSubCollectionChanges();
        }

        private void SubscribeToSubCollectionChanges()
        {
            foreach (var collection in _subCollections)
            {
                collection.CollectionChanged += OnSubCollectionChanged;
            }
        }

        private void OnSubCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = (ObservableCollection<T>)sender;
            int startingIndex = 0;

            foreach (var c in _subCollections)
            {
                if (c != collection)
                    startingIndex += c.Count();
                else
                    break;
            }

            NotifyCollectionChangedEventArgs args;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    args = new NotifyCollectionChangedEventArgs(e.Action, e.NewItems[0], e.NewStartingIndex + startingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    args = new NotifyCollectionChangedEventArgs(e.Action, e.OldItems[0], e.OldStartingIndex + startingIndex);
                    break;
                case NotifyCollectionChangedAction.Move:
                    args = new NotifyCollectionChangedEventArgs(e.Action, e.NewItems, e.NewStartingIndex + startingIndex, e.OldStartingIndex + startingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace: 
                    args = new NotifyCollectionChangedEventArgs(e.Action, e.NewItems, e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    args = new NotifyCollectionChangedEventArgs(e.Action);
                    break;
                default:
                    args = null;
                    break;
            }

           this.OnCollectionChanged(args);
        }

        #region IEnumerable Memebers

        public IEnumerator<T> GetEnumerator()
        {
            return this._subCollections.SelectMany(collection => collection).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region INotifyCollectionChanged Memebers

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = this.CollectionChanged;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}
