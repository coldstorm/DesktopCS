using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace DesktopCS.Helpers
{
    class CompositeCollection<T> : ObservableCollection<T>
    {
        private readonly ObservableCollection<T>[] _subCollections;

        public CompositeCollection(params ObservableCollection<T>[] subCollections)
        {
            _subCollections = subCollections;

            foreach (var collection in subCollections)
            {
                AddItems(collection);
            }
            AddSubCollections();
            SubscribeToSubCollectionChanges();
        }

        private void AddSubCollections()
        {
            foreach (var collection in _subCollections)
            {
                AddItems(collection);
            }
        }

        private void AddItems(IEnumerable<T> items)
        {
            foreach (T me in items)
                Add(me);
        }

        private void RemoveItems(IEnumerable<T> items)
        {
            foreach (T me in items)
                Remove(me);
        }

        private void SubscribeToSubCollectionChanges()
        {
            foreach (var collection in _subCollections)
            {
                collection.CollectionChanged += OnSubCollectionChanged;
            }
        }

        private void OnSubCollectionChanged(object source, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add: AddItems(args.NewItems.Cast<T>());
                    break;

                case NotifyCollectionChangedAction.Remove: RemoveItems(args.OldItems.Cast<T>());
                    break;

                case NotifyCollectionChangedAction.Reset: Clear();
                    AddSubCollections();
                    break;
            }
        }
    }
}
