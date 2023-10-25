using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BodyBuddy.Events
{
    public class CheckedChangedBehavior : Behavior<RadioButton>
    {
        public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CheckedChangedBehavior), null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        protected override void OnAttachedTo(RadioButton bindable)
        {
            bindable.CheckedChanged += OnCheckedChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(RadioButton bindable)
        {
            bindable.CheckedChanged -= OnCheckedChanged;
            base.OnDetachingFrom(bindable);
        }

        private void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (Command != null && Command.CanExecute(e) && e.Value)
            {
                var radioButton = sender as RadioButton;
                Command.Execute(radioButton.Content);
            }
        }
    }
}
