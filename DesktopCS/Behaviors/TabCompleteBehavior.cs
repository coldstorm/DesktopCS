using System;
using System.Collections.ObjectModel;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using DesktopCS.Helpers.Extensions;                      
using DesktopCS.Models;

namespace DesktopCS.Behaviors
{
    public class TabCompleteBehavior : Behavior<TextBox>
    {
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items",
            typeof(ObservableCollection<UserItem>),
            typeof (TabCompleteBehavior),
            new FrameworkPropertyMetadata(null));

        public ObservableCollection<UserItem> Items
        {
            get { return (ObservableCollection<UserItem>)base.GetValue(ItemsProperty); }
            set { base.SetValue(ItemsProperty, value); }
        }

        private TextBox _textBox;
        private int _completeIndex = -1;
        private string _lastWord = "";

        protected override void OnAttached()
        {
            base.OnAttached();

            this._textBox = this.AssociatedObject;
            this._textBox.PreviewKeyDown += _textBox_PreviewKeyDown;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();

            this._textBox.PreviewKeyDown -= this._textBox_PreviewKeyDown;
        }

        private void _textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Tab)
            {
                // Cancel current loop if any other key is pressed
                this._completeIndex = -1;
            }
            else if (this.Items != null && this._textBox.CaretIndex == this._textBox.Text.Length)
            {
                var lastWord = this._textBox.Text.Split(' ').Last();
                var replaceWord = this._completeIndex == -1 ? lastWord : this._lastWord;
                
                UserItem[] results = this.Items.Where(s => s.NickName.StartsWith(replaceWord, StringComparison.OrdinalIgnoreCase)).ToArray();
                var count = results.Count();

                if (count > 0)
                {
                    if (count > 1)
                    {
                        if (_completeIndex == -1)
                        {
                            this._completeIndex = count - 1;
                            this._lastWord = lastWord;
                        }
                        _completeIndex--;
                    }

                    this._textBox.Text = this._textBox.Text.ReplaceLastOccurrence(lastWord,
                        results[_completeIndex + 1].NickName);
                    this._textBox.CaretIndex = this._textBox.Text.Length;
                }
            }
        }
    }
}
