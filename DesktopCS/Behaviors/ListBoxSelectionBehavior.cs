using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace DesktopCS.Behaviors
{
    public static class ListBoxSelectionBehavior 
    {
        public static readonly DependencyProperty ClickSelectionProperty = 
            DependencyProperty.RegisterAttached("ClickSelection",  typeof(bool), typeof(ListBoxSelectionBehavior), new UIPropertyMetadata(false, OnClickSelectionChanged));

        public static bool GetClickSelection(DependencyObject obj) 
        {
            return (bool) obj.GetValue(ClickSelectionProperty); 
        }

        public static void SetClickSelection(DependencyObject obj, bool value) 
        {
            obj.SetValue(ClickSelectionProperty, value); 
        }

        private static void OnClickSelectionChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e) 
        {
            var listBox = dpo as ListBox;
            if (listBox != null) 
            { 
                if ((bool)e.NewValue) 
                {
                    listBox.SelectionMode = SelectionMode.Multiple;
                    listBox.SelectionChanged += OnSelectionChanged;
                } 
                else 
                {
                    listBox.SelectionChanged -= OnSelectionChanged;
                } 
            } 
        }

        static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var listBox = sender as ListBox;
                var valid = e.AddedItems[0];
                if (listBox != null)
                    foreach (var item in new ArrayList(listBox.SelectedItems))
                    {
                        if (item != valid)
                        {
                            listBox.SelectedItems.Remove(item);
                        }
                    }
            }
        }
    }
}
