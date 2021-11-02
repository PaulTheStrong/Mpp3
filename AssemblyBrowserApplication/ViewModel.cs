using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AssemblyBrowserApplication.Annotations;
using Library;
using Microsoft.Win32;

namespace AssemblyBrowserApplication
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly AssemblyLoader AssemblyLoader = new AssemblyLoader();
        public List<Namespace> Namespaces { get; private set; }

        private string _assemblyFilePath;

        public string AssemblyFilePath
        {
            get => _assemblyFilePath;
            set
            {
                _assemblyFilePath = value;
                try
                {
                    Namespaces = AssemblyLoader.GetAssemblyInfo(_assemblyFilePath);
                }
                catch (FileNotFoundException e)
                {
                    Console.Error.WriteLine($"File {_assemblyFilePath} not found");
                    _assemblyFilePath = null;
                }
                OnPropertyChanged(nameof(Namespaces));
            }
        }

        public ICommand OpenFileCommand => new OpenFileCommand(OpenAssembly);

        private void OpenAssembly()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Assemblies|*.dll",
                Title = "Select assembly",
                Multiselect = false
            };
            bool opened = openFileDialog.ShowDialog() ?? false;
            if (opened)
            {
                AssemblyFilePath = openFileDialog.FileName;
                OnPropertyChanged(nameof(AssemblyFilePath));
            }
        }
    }
}