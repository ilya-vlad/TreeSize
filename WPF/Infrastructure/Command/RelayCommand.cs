﻿using System;
using System.Threading.Tasks;

namespace WPF.Infrastructure.Command
{
    internal class RelayCommand : BaseCommand
    {
        private readonly Action<object> execute;

        private readonly Func<object, bool> canExecute;

        public RelayCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            canExecute = CanExecute;
        }

        public override bool CanExecute(object p) => canExecute?.Invoke(p) ?? true;

        public override async void Execute(object p) => execute(p);   
    }
}