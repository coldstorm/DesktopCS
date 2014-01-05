using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using DesktopCS.Helpers;
using DesktopCS.Helpers.Extensions;
using DesktopCS.Helpers.Parsers;
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
        private int _completeIndex;

        protected override void OnAttached()
        {
            base.OnAttached();

            this._textBox = this.AssociatedObject;
            this._textBox.KeyDown += _textBox_KeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this._textBox.KeyDown -= this._textBox_KeyDown;
        }

        void _textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && this.Items != null)
            {
                // TODO: Edit the current word and not the last one typed in
                // TODO: If there are more than 1 enteries found, loop trough them

                var lastWord = this._textBox.Text.Split(' ').Last();
                UserItem[] results = this.Items.Where(s => s.NickName.ToLower().StartsWith(lastWord.ToLower())).ToArray();

                var count = results.Count();
                if (count >= 1)
                {
                    this._textBox.Text = this._textBox.Text.ReplaceLastOccurrence(lastWord, results[0].NickName);
                    this._textBox.CaretIndex = this._textBox.Text.Length;
                }
            }
            else
            {
                this._completeIndex = -1;
            }
        }
    }
}
