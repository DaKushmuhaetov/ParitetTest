using Paritet.Model;
using Paritet.Service;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Paritet.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        private readonly IMessageBox messageBoxService;

        private string firstName;
        public string FirstName
        {
            get => firstName;
            set
            {
                if (value == firstName)
                    return;

                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        private string middleName;
        public string MiddleName
        {
            get => middleName;
            set
            {
                if (value == middleName)
                    return;

                middleName = value;
                OnPropertyChanged("MiddleName");
            }
        }

        private string lastName;
        public string LastName
        {
            get => lastName;
            set
            {
                if (value == lastName)
                    return;

                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        private DateTime dateOfBirth;
        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set
            {
                if (value == dateOfBirth)
                    return;

                dateOfBirth = value;
                OnPropertyChanged("DateOfBirth");
            }
        }

        private int height;
        public int Height
        {
            get => height;
            set
            {
                if (value == height)
                    return;

                height = value;
                OnPropertyChanged("Height");
            }
        }

        private double weight;
        public double Weight
        {
            get => weight;
            set
            {
                if (value == weight)
                    return;

                weight = value;
                OnPropertyChanged("Weight");
            }
        }

        private Person selectedPerson;
        public Person SelectedPerson
        {
            get => selectedPerson;
            set
            {
                if (value == selectedPerson)
                    return;

                selectedPerson = value;
                OnPropertyChanged("SelectedPerson");
            }
        }

        public RelayCommand RemoveCommand
        {
            get
            {
                return new RelayCommand(delegate (object parameter)
                {
                    Person person = parameter as Person;
                    
                    using (Context db = new Context()) {
                        db.Entry<Person>(person).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                    }

                    People.Remove(person);

                    messageBoxService.Show("Запись успешно удалена!");
                }, (obj) => SelectedPerson != null);
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return new RelayCommand(delegate (object parameter)
                {
                    Person person = GetPersonFromParameter(parameter);
                    person.ID = SelectedPerson.ID;

                    People.Remove(People.Where(o => o.ID == person.ID).First());

                    using (Context db = new Context())
                    {
                        db.Entry<Person>(person).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }

                    People.Add(person);

                    messageBoxService.Show("Запись успешно изменена!");
                }, (obj) =>
                {
                    if (SelectedPerson != null)
                    {
                        return IsCorrect(obj);
                    }
                    else
                    {
                        return false;
                    }
                });
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return new RelayCommand(delegate (object parameter)
                {
                    Person person = GetPersonFromParameter(parameter);
                    person.ID = Guid.NewGuid();
                    
                    using (Context db = new Context())
                    {
                        db.People.Add(person);
                        db.SaveChanges();
                    }

                    People.Add(person);

                    messageBoxService.Show("Запись успешно добавлена!");
                }, (obj) => {
                    return IsCorrect(obj);
                });
            }
        }

        private Person GetPersonFromParameter(object parameter)
        {
            var values = (object[])parameter;
            var firstName = (string)values[0];
            var middleName = (string)values[1];
            var lastName = (string)values[2];
            var dateOfBirth = ((DateTime?)values[3]) == null ? DateTime.Now : ((DateTime?)values[3]).Value;
            int? height = null;
            double? weight = null;

            if(!String.IsNullOrWhiteSpace((string)values[4]))
                height = Convert.ToInt32(values[4]);

            if (!String.IsNullOrWhiteSpace((string)values[5]))
                weight = Convert.ToDouble(values[5]);

            Person person = new Person()
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Height = height ?? 0,
                Weight = weight ?? 0
            };

            return person;
        }

        private bool IsCorrect(object obj)
        {
            Person person = GetPersonFromParameter(obj);

            if (person == null)
                return false;
            if (string.IsNullOrWhiteSpace(person.FirstName))
                return false;
            if (string.IsNullOrWhiteSpace(person.MiddleName))
                return false;
            if (string.IsNullOrWhiteSpace(person.LastName))
                return false;
            if (person.Height <= 0)
                return false;
            if (person.Weight <= 0)
                return false;

            return true;
        }

        public ObservableCollection<Person> People { get; set; }

        public PersonViewModel(IMessageBox messageBox)
        {
            messageBoxService = messageBox;
            People = new ObservableCollection<Person>();
            UpdateCollection();
        }

        private void UpdateCollection()
        {
            People.Clear();

            using (Context db = new Context()) {
                db.People.OrderBy(o => new { o.FirstName, o.MiddleName, o.LastName }).ToList().ForEach(o => People.Add(o));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}