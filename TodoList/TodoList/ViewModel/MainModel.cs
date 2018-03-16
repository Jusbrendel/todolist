using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoList.Helper;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace TodoList.ViewModel
{
    class MainModel : Helper.MyPropertyChanged
    {
        /**
        * Déclaration de la liste
        **/
   
        private ObservableCollection<Model.Tasks> _ListTasks { get; set; }
        public ObservableCollection<Model.Tasks> ListTasks { get { return _ListTasks; } set { _ListTasks = value; NotifyPropertyChanged(); } }

        /**
        * Properties qui permettent de récupérer les informations lors de la validation du formulaire
        **/

        public DateTimeOffset NewTaskDate { get { return NewTask.date; } set { NewTask.date = value.DateTime; NotifyPropertyChanged(); } }
        public String NewTaskTitle { get { return NewTask.title; } set { NewTask.title = value; NotifyPropertyChanged(); } }
        public String NewTaskContent { get { return NewTask.content; } set { NewTask.content = value; NotifyPropertyChanged(); } }
        public Boolean NewTaskStatut { get { return NewTask.statut; } set { NewTask.statut = value; NotifyPropertyChanged(); } }
        private Model.Tasks NewTask { get; set; }
        public IValueConverter Converter { get; set; }

        /**
        * Permet de sélectionner un objet de la liste 
        **/

        private Model.Tasks _SeletedModel { get; set; }

        public Model.Tasks SelectedModel
        {
            get { return _SeletedModel; }
            set {
                if (value != null)
                {
                    _SeletedModel = value;
                }
                if (SelectedModel != null)
                {
                    SetTextBoxValue();
                }
            }
        }

        /**
        * Commande qui permet d'ajouter une tâche
        **/

        private Helper.Command _addCommand;

        public ICommand addCommand
        {
            get
            {
                if (_addCommand == null)
                    _addCommand = new Helper.Command((x) => addSomething(x), true);
                return _addCommand;
            }
        }

        /**
        * Fonction qui permet de créer une tâche et de l'ajouter à la liste 
        **/

        public void addSomething(object parameter)
        {
            if (NewTask.title == "" || NewTask.title == null || NewTask.content == "" || NewTask.content == null)
                dialogErrorAdd();
            else
            {
                ListTasks.Add(NewTask);
                NotifyPropertyChanged("ListTasks");
                NewTask = new Model.Tasks();
                NewTaskTitle = null;
                NewTaskContent = null;
                NewTaskDate = DateTime.Today;
            }
        }

        /**
        * Commande qui permet de supprimer une tâche
        **/

        public ICommand _delCommand { get; set; }

        public ICommand DeleteCommand
        {
            get
            {
                if (_delCommand == null)
                    _delCommand = new Helper.Command((x) => delSomething(x), true);
                return _delCommand;
            }
        }

        /**
        * Fonction qui permet de supprimer une tâche de la liste 
        **/

        private async void delSomething(object parameter)
        {
            MessageDialog showDialog = new MessageDialog("Etês-vous certain de vouloir supprimer cette tâche : " + SelectedModel.title + " ?");
            showDialog.Commands.Add(new UICommand("Oui") { Id = 0 });
            showDialog.Commands.Add(new UICommand("Non") { Id = 1 });
            showDialog.DefaultCommandIndex = 0;
            showDialog.CancelCommandIndex = 1;
            var result = await showDialog.ShowAsync();
            if ((int)result.Id == 0)
            {
                if (SelectedModel != null)
                {
                    ListTasks.Remove(SelectedModel);
                    NotifyPropertyChanged("ListTasks");
                }
            }
        }

        /**
        * Commande qui permet d'éditer une tâche
        **/

        public ICommand _editCommand { get; set; }

        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new Helper.Command((x) => EditTask(x), true);
                return _editCommand;
            }
        }

        /**
        * Fonction qui permet d'éditer une tâche de la liste 
        **/

        public void EditTask(object parameter)
        {
            if (NewTask.title == "" || NewTask.title == null || NewTask.content == "" || NewTask.content == null)
                dialogErrorAdd();
            else
            {
                ListTasks.Add(NewTask);
                ListTasks.Remove(SelectedModel);
                NotifyPropertyChanged("ListTasks");
                NewTask = new Model.Tasks();
                NewTaskTitle = null;
                NewTaskContent = null;
                NewTaskDate = DateTime.Today;
            }
        }

        /**
        * Fonction qui permet l'affichage d'une boîte de dialogue en cas de tâche incomplète
        **/

        private async void dialogErrorAdd()
        {
            MessageDialog showDialog = new MessageDialog("Impossible d'ajouter une tâche incomplète, veuillez renseigner un titre et une description.");
            var result = await showDialog.ShowAsync();
        }

        /**
        * Fonction qui initialise les valeurs d'une tâche dans les champs textes. 
        **/

        private void SetTextBoxValue()
        {
            NewTaskTitle = SelectedModel.title;
            NewTaskContent = SelectedModel.content;
            NewTaskDate = SelectedModel.date;
            NewTaskStatut = SelectedModel.statut;
        }

        /**
         * Fonction principale du programme qui permet la création d'une liste de tâches
         **/

        public MainModel()
        {
            ListTasks = new ObservableCollection<Model.Tasks>();
            NewTask = new Model.Tasks() { date = DateTime.Today };
        }
    }
}
