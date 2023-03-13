// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Test
{
    public enum InputValidationState
    {
        Unvalidated,
        Error,
        Passed
    }

    public class ValidationEventArgs : EventArgs
    {
        public InputValidationState State;
    }

    [ContentProperty(Name = "InputControl")]
    public sealed class InputValidationHost : Control
    {
        public InputValidationHost()
        {
            this.DefaultStyleKey = typeof(InputValidationHost);
            StateChanged += OnStateChanged;
        }

        public Control InputControl
        {
            get => (Control)GetValue(InputControlProperty);
            set => SetValue(InputControlProperty, value);
        }

        public static readonly DependencyProperty InputControlProperty = DependencyProperty.Register(
            nameof(InputControl),
            typeof(Control),
            typeof(InputValidationHost),
            new(null));

        public InputValidationState State
        {
            get => (InputValidationState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
            nameof(State),
            typeof(InputValidationState),
            typeof(InputValidationHost),
            new(InputValidationState.Unvalidated, StateChangedCallback));

        public IList<Func<object, bool>> Validators
        {
            get => (IList<Func<object, bool>>)GetValue(ValidatorsProperty);
            set => SetValue(ValidatorsProperty, value);
        }

        public static readonly DependencyProperty ValidatorsProperty = DependencyProperty.Register(
            nameof(Validators),
            typeof(IList<Func<object, bool>>),
            typeof(InputValidationHost),
            new(new List<Func<object, bool>>()));

        public object ErrorMessage
        {
            get => GetValue(ErrorMessageProperty);
            set => SetValue(ErrorMessageProperty, value);
        }

        public static readonly DependencyProperty ErrorMessageProperty = DependencyProperty.Register(
            nameof(ErrorMessage),
            typeof(object),
            typeof(InputValidationHost),
            new(null));

        public object PassedMessage
        {
            get => GetValue(PassedMessageProperty);
            set => SetValue(PassedMessageProperty, value);
        }

        public static readonly DependencyProperty PassedMessageProperty = DependencyProperty.Register(
            nameof(PassedMessage),
            typeof(object),
            typeof(InputValidationHost),
            new(null));

        public object UnvalidatedMessage
        {
            get => GetValue(UnvalidatedMessageProperty);
            set => SetValue(UnvalidatedMessageProperty, value);
        }

        public static readonly DependencyProperty UnvalidatedMessageProperty = DependencyProperty.Register(
            nameof(UnvalidatedMessage),
            typeof(object),
            typeof(InputValidationHost),
            new(null));

        public bool HasPassed
        {
            get => (bool)GetValue(HasPassedProperty);
            set => SetValue(HasPassedProperty, value);
        }

        public static readonly DependencyProperty HasPassedProperty = DependencyProperty.Register(
            nameof(HasPassed),
            typeof(bool),
            typeof(InputValidationHost),
            new(false));

        public bool HasError
        {
            get => (bool)GetValue(HasErrorProperty);
            set => SetValue(HasErrorProperty, value);
        }

        public static readonly DependencyProperty HasErrorProperty = DependencyProperty.Register(
            nameof(HasError),
            typeof(bool),
            typeof(InputValidationHost),
            new(false));

        public bool IsUnvalidated
        {
            get => (bool)GetValue(IsUnvalidatedProperty);
            set => SetValue(IsUnvalidatedProperty, value);
        }

        public static readonly DependencyProperty IsUnvalidatedProperty = DependencyProperty.Register(
            nameof(IsUnvalidated),
            typeof(bool),
            typeof(InputValidationHost),
            new(true));

        public event EventHandler<ValidationEventArgs> Validate;
        public event EventHandler<ValidationEventArgs> Validated;
        public event EventHandler<ValidationEventArgs> Passed;
        public event EventHandler<ValidationEventArgs> Error;
        public event EventHandler<ValidationEventArgs> StateChanged;

        public Func<Control, object> GetContent { get; set; }

        private void OnValidate()
        {
            Validate?.Invoke(this, new() { State = State });
        }

        private void OnValidated()
        {
            Validated?.Invoke(this, new() { State = State });
        }

        private void OnPassed()
        {
            Passed?.Invoke(this, new() { State = State });
        }

        private void OnError()
        {
            Error?.Invoke(this, new() { State = State });
        }

        private void OnStateChanged(object sender, ValidationEventArgs args)
        {
            VisualStateManager.GoToState(this, State.ToString(), false);
        }

        private static void StateChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var control = (InputValidationHost)d;
            control.SetValue(HasPassedProperty, control.State == InputValidationState.Passed);
            control.SetValue(HasErrorProperty, control.State == InputValidationState.Error);
            control.SetValue(IsUnvalidatedProperty, control.State == InputValidationState.Unvalidated);
            control.StateChanged?.Invoke(control, new() { State = control.State });
        }

        public void TriggerValidation(object content)
        {
            OnValidate();
            var isPassed = Validators.All(validator => validator(content));
            if (isPassed)
            {
                State = InputValidationState.Passed;
                OnPassed();
            }
            else
            {
                State = InputValidationState.Error;
                OnError();
            }

            OnValidated();
        }

        public void TriggerValidation()
        {
            TriggerValidation(GetContent(InputControl));
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            VisualStateManager.GoToState(this, State.ToString(), false);
        }
    }
}