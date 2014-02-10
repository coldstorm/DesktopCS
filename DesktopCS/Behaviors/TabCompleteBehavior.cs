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

        private UserItem[] results;
        private TextBox _textBox;
        private int _completeIndex = -1;

        private string replaceWord;
        private int wordStart = -1;
        private int wordEnd = -1;

        protected override void OnAttached()
        {
            base.OnAttached();

            this._textBox = this.AssociatedObject;
            this._textBox.KeyUp += this._textBox_KeyUp;
            this._textBox.KeyDown += this._textBox_KeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this._textBox.KeyUp -= this._textBox_KeyUp;
            this._textBox.KeyDown -= this._textBox_KeyDown;
        }

        void _textBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Tab && this.Items != null)
            {
                // Cancel current loop if any other key is pressed

                this.wordStart = this._textBox.Text.LastIndexOf(" ", this._textBox.CaretIndex) + 1;
                this.wordEnd = this._textBox.Text.IndexOf(" ", wordStart);

                if (wordEnd == -1)
                {
                    wordEnd = this._textBox.Text.Length;
                }

                replaceWord = this._textBox.Text.Substring(wordStart, wordEnd);

                this._completeIndex = 0;

                this.results = this.Items.Where(s => s.NickName.StartsWith(replaceWord, StringComparison.OrdinalIgnoreCase)).ToArray();
            }
        }

        void _textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                if (results.Count() > 1)
                {
                    var replaceEnd = this._textBox.Text.IndexOf(" ", this.wordStart);

                    if (replaceEnd == -1)
                    {
                        replaceEnd = this._textBox.Text.Length;
                    }

                    this._textBox.Text = this._textBox.Text.Substring(0, this.wordStart) +
                        this.results[this._completeIndex].NickName +
                        this._textBox.Text.Substring(replaceEnd);
                    this._textBox.CaretIndex = this._textBox.Text.Length;

                    this._completeIndex++;
                    if (this._completeIndex == results.Count())
                    {
                        this._completeIndex = 0;
                    }
                }
            }
        }
    }
}
