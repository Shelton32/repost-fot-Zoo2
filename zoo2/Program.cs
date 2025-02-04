using System;
using System.Threading.Channels;

namespace zoo2

{
    internal class Program
    {

        static void Main(string[] args)
        {
           Zoo zoo = new Zoo();// Создание самого зоопарка 
           while (true)
           {
               Console.WriteLine("Добро пожаловать в систему управления зоопарка!");
               Console.WriteLine("1. Создать новый вольер");
               Console.WriteLine("2. Добавить животное");
               Console.WriteLine("3. Посмотреть все вольеры и животных");
               Console.WriteLine("4. Покормить животное");
               Console.WriteLine("5. Покормить всех животных в зоопарке");
               Console.WriteLine("6. Заставить животное издать звук");
               Console.WriteLine("7. Уложить животное спать");
               Console.WriteLine("8. Разбудить животное");
               Console.WriteLine("9. Поиграть с животным");
               Console.WriteLine("0. Выйти из программы");
               
               string choice = Console.ReadLine();
               switch (choice)
               {
                   case "1":
                       Console.WriteLine("Введите название вольера: ");
                       string encName = Console.ReadLine();
                       if (string.IsNullOrEmpty(encName))
                       {
                           Console.WriteLine("Название вольера не может быть пустым.");
                           break;
                       }
                       zoo.AddEnclosure(encName);// это добавление нового вольера: 
                       break;
                   case "2": // Добавление животных 
                       Console.Write("Введите название вольера: ");
                       string encToAdd = Console.ReadLine();
                       var enclosure = zoo.GetEnclosure(encToAdd);
                       if (enclosure != null) //Если аольер найден
                       {
                           Console.WriteLine("Введите имя животного: ");
                           string name = Console.ReadLine();
                           if (string.IsNullOrWhiteSpace(name))
                           {
                               Console.WriteLine("Имя не может быть пустым.");
                               return;
                           }
                           Console.Write("Введите возраст: ");
                           if (!int.TryParse(Console.ReadLine(), out int age) || age <= 0)
                           {
                               Console.WriteLine("Возраст не может быть отрицательным.");
                               return;
                           }

                           Console.WriteLine("Введите вес: ");
                           if (!double.TryParse(Console.ReadLine(), out double weight) || weight <= 0)
                           {
                               Console.WriteLine("Вес должен быть положительным числом.");
                               return;
                           }

                           Console.Write("Выберите тип животного (Лев/Попугай/Пингвин): ");
                           string type = Console.ReadLine().ToLower();//Преображение только в нижний регистр

                           switch (type)
                           {
                               case "лев": 
                                   enclosure.AddAnimal(new Lion(name, age, weight));//Добавили льва
                                   break;
                               case "попугай":
                                   enclosure.AddAnimal(new Parrot(name, age, weight));//Добавление попугая
                                   break;
                               case "пингвин":
                                   enclosure.AddAnimal(new Penguin(name, age, weight));// Добавление пингвина 
                                   break;
                               default:
                                   Console.WriteLine("Неизвестный тип животного. ");
                                   break;
                           }
                       }
                       else
                       {
                           Console.WriteLine("Такого вольера нет.");//Это если вольер не найден
                       }
                       break;
                   case "3": // ДЛя просмотра всех вольеров и животных в них
                       zoo.ListEnclosures();// Нужно для вывода всех вольеров 
                       Console.WriteLine("Введите номер вольера для просмотра животных:");
                       if (!int.TryParse(Console.ReadLine(), out int encChoice))
                       {
                           Console.WriteLine("Неверный формат номера вольера");
                           return;
                       }

                       var selectedEnclosure = zoo.GetEnclosureByIndex(encChoice - 1);//нужно чтобы найти вольер по индексу 
                       if (selectedEnclosure != null)
                       {
                           selectedEnclosure.ListAnimals();//Вывод всех животных в вольере 
                       }
                       else
                       {
                           Console.WriteLine("Такого вольера нет: это печально");//Если вдруг вольер не найден
                       }
                       break;
                   case "4":// Далее кормление животного
                       Console.WriteLine("Введите название вольера для кормления животного");
                       string feedEnclosureName = Console.ReadLine();
                       var feedEnclousre = zoo.GetEnclosure(feedEnclosureName);// Поиск Вольера 
                       if (feedEnclousre != null)
                       {
                           Console.Write("Введите номер животного для кормления ");
                           int animalIndex = int.Parse(Console.ReadLine()) - 1;
                           Animal animalToFeed = feedEnclousre.GetAnimal(animalIndex);//Для получения животного по индексу
                           if (animalToFeed != null)
                           {
                               animalToFeed.Feed();//кормление животного
                           }
                           else
                           {
                               Console.WriteLine("Животное не найдено");
                           }
                       }
                       else
                       {
                           Console.WriteLine("Вольер не найден.");
                       }
                       break;
                   case "5"://Для кормление всех животных во всех аольерах
                       zoo.FeedAllEnclosures();
                       Console.WriteLine("Во всех вольерах все животные поели ");
                       break;
                   case "6": //Звуки животное 
                       Console.Write("Введите название вольера ");
                       string soundEnclosreName = Console.ReadLine();
                       var soundEnclosre = zoo.GetEnclosure(soundEnclosreName);// Для поиска вольера 
                       if (soundEnclosre != null)
                       {
                           Console.Write("Введите номер живтного для издания звука: ");
                           int animalIndexSound = int.Parse(Console.ReadLine()) - 1;
                           Animal animalToSound = soundEnclosre.GetAnimal(animalIndexSound);//Получение животного по индексу
                           if (animalToSound != null)
                           {
                               animalToSound.MakeSound();//Воспроизведение звука
                           }
                           else
                           {
                               Console.WriteLine("Животное не найдено");
                           }
                       }
                       else
                       {
                           Console.WriteLine("Вольер не найден");
                       }
                       break;
                   case "7": // Укладывание животного спать 
                       Console.WriteLine("Введите название вольера для укладывания в сон: ");
                       string sleepEnclosureName = Console.ReadLine();
                       var sleepEnclosure = zoo.GetEnclosure(sleepEnclosureName); // Поиск вольера
                       if (sleepEnclosure != null)
                       {
                           Console.Write("Введите название вольера для укладывания в сон: ");
                           int animalIndexSleep = int.Parse(Console.ReadLine()) - 1;
                           Animal animalToSleep = sleepEnclosure.GetAnimal(animalIndexSleep);//Для получения животного по индексу
                           if (animalToSleep != null)
                           {
                               animalToSleep.Sleep();
                           }
                           else
                           {
                               Console.WriteLine("Вольер не найден");
                           }
                       }
                       else
                       {
                           Console.WriteLine("Волер не найден");
                       }
                       break;
                   case "8":
                       Console.Write("Введите название вольера для пробуждения: ");
                       string wakeUpEnclosureName = Console.ReadLine();
                       var wakeUpEnclosure = zoo.GetEnclosure(wakeUpEnclosureName);//Поиск вольера 
                       if (wakeUpEnclosure != null)
                       {
                           Console.WriteLine("Введите номер животного для пробужения: ");
                           int animalIndexWakeUp = int.Parse(Console.ReadLine()) - 1;
                           Animal animalToWakeUp = wakeUpEnclosure.GetAnimal(animalIndexWakeUp);//Полцчение животного по индексу 
                           if (animalToWakeUp != null)
                           {
                               animalToWakeUp.WakeUp();// Разбуждение живтного
                           }
                           else
                           {
                               Console.WriteLine("Животное не найдено");
                           }
                       }
                       else
                       {
                           Console.WriteLine("Вольер не найден.");
                       }
                       break;
                   case "9": // Игра с животным
                       Console.Write("Введите название вольера для игры: ");
                       string playEnclosureName = Console.ReadLine();
                       var playEnclosure = zoo.GetEnclosure(playEnclosureName); // Поиск вольера
                       if (playEnclosure != null) // Если вольер найден
                       {
                           Console.Write("Введите номер животного для игры: ");
                           int animalIndexPlay = int.Parse(Console.ReadLine()) - 1;
                           Animal animalToPlay = playEnclosure.GetAnimal(animalIndexPlay); // Получение животного по индексу
                           if (animalToPlay != null)
                           {
                               animalToPlay.Play(); // Игра с животным
                               Console.WriteLine($"Вы поиграли с {animalToPlay.GetName()}, оно счастливо!"); // Сообщение о счастье животного
                           }
                           else
                           {
                               Console.WriteLine("Животное не найдено."); // Если животное не найдено
                           }
                       }
                       else
                       {
                           Console.WriteLine("Вольер не найден."); // Если вольер не найден
                       }
                       break;
                   case "0":
                       Console.WriteLine("Вы вышли из программы");
                       return;
                       
               }
           }
           
        }

    }
    //Интерфейс для животных

    public interface IAnimal
    {
     
    }
    
    //абстрактный класс для животных реализующи  интерфейс Animal

    public abstract class Animal 
    {
        public string Name { get; private set; }//Свойство для имени животного
        public int Age { get; private set; }//Свойстов для возраста животного
        public double Weight { get; private set; }// Свойство для веса животного
        public bool Hungry { get; private set; } = true;// Свойство для состояния сна животного 
        private bool _sleeping = false; //Приватное поле для состояния сна животного
    
        
        //Конструкция класса Animal, который инициализирует имя, возраст и вес
        public Animal(string name, int age, double weight)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Поле с именем не может быть пустым.");
                }
                if (age < 0)
                {
                    throw new ArgumentException("Возраст не может быть отрицательным числом.");
                }

                if (weight <= 0)
                {
                    throw new ArgumentException("Вес не может быть отрицательным числом.");
                }
                
                Name = name;
                Age = age; 
                Weight = weight;
            }
        
        
        
        //Метод для кормление животного
        public void Feed()
        {
            if (_sleeping)
            {
                Console.WriteLine($"{Name} спит и не может есть. ");
            }
            else
            {
                Hungry = false;
                Console.WriteLine($"{Name} покормлен. ");
            }
        }
        
        //Метод для укладывания спать животного
        public void Sleep()
        {
            //Здесь мы устанавливаем что животное спит, и соответственно оно сразу становится голодным
            _sleeping = true;
            Hungry = true;
            Console.WriteLine($"{Name} заснул. ");
        }
        
        //Метод лдя пробуждения животного
        public void WakeUp()
        {
            _sleeping = false;
            Console.WriteLine($"{Name} проснулось и голодное");
        }
        
        //Метод для игры с животным 

        public void Play()
        {
            // Хдесь мы проверяем на голод животное
            if (Hungry)
            {
                Hungry = true;
                Console.WriteLine($"{Name} животное голодное и не хочет играть. ");
            }
            else
            {
                Console.WriteLine($"{Name} животное играет! ");
            }
        }
        
        // Метод дял проверки, спит ли животное 
        
        public bool IsSleeping() => _sleeping;// Возвращения сна животного
        public string GetName() => Name;// Возвращение имени животного
        public int GetAge() => Age; // Возвращение возраста животного
        public double GetWeight() => Weight; // Возвращение веса животного
        public abstract string GetType(); // Абстрактный метод для получения типа животного
        public abstract void MakeSound(); // Абстрактный метод для издания звука животным
        
    }
    
    
    // Классы для животных, которые наследует класс Animal и реализуются его методы 

    // Для льва
    public class Lion : Animal
    {
        public Lion(string name, int age, double weight) : base(name, age, weight){} // Конструктор для Льва 
        
        public override string GetType() => "Лев";//Метод для определения типа животного

        public override void MakeSound()
        {
        Console.WriteLine($"{GetName()} рычит: Ррррр! ");
        }
        
    }
    
    // Для попугая
    public class Parrot : Animal
    {
        // Конструктор для Попугая 
        public Parrot(string name, int age, double weight) : base(name, age, weight) {} // Конструктор для попугая 

        public override string GetType() => "Попугай"; // Метод для определения типа животного

        public override void MakeSound() => Console.WriteLine($"{GetName()} говорит: Привет! ");

    }
    
    //Для Пингвина

    public class Penguin : Animal
    {
        public Penguin(string name, int age, double weight) : base(name, age, weight) {} // Конструктор для пингвина

        public override string GetType() => "Пингвин"; // Переопределения метода для получения типа животного

        public override void MakeSound() => Console.WriteLine($"{GetName()} говорит: Кря-кря! "); //Переопределения метода для издания звука
    }
    
    
    // Класс Вольер для хранения животных

    public class Enclosure
    {
        public string Name { get; private set; } // Свойства для названия вольера 
        private List<Animal> _animals = new List<Animal>(); //Коллекция для хранения животных в вольере

        // Конструктор для создания вольера
        public Enclosure(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Поле с названием вольера не может быть пустым");
            }
            Name = name;
        }
        
        // Метод для добавление животного в вольере 
        public void AddAnimal(Animal animal)
        {
            _animals.Add(animal);// Добавление животного в коллекцию 
            Console.WriteLine($"{animal.GetName()} добавлен в {Name}.");
        }
        
        // Метод для вывода списка всех животных в вольере 
        public void ListAnimals()
        {
            Console.WriteLine($"В вольере {Name} находятся следующие животные: ");
            for (int i = 0; i < _animals.Count; i++) // Для перебора всех животных в вольере 
            {
                var animal = _animals[i];
                Console.WriteLine($"{i + 1}. Имя: {animal.GetName()}, Тип: {animal.GetType()}, Возраст: {animal.GetAge()}, Вес: {animal.GetWeight()} кг.");
            }
        }
        
        // Методы получения животного по индексу 
        // ПОДСКАЗАЛ ЧАТ ГПТ НЕ ОЧ ПОНЯЛ КАК ЭТО РАБОТАЕТ 
        public Animal GetAnimal(int index)
        {
            if (index >= 0 && index < _animals.Count) // Если индекс в пределах допустимого диапазона
            {
                return _animals[index];//Возвращаем животное по индексу
            }
            return null;
        }
        
        //метод для кормления всех животных в вольере 
        public void FeedAllAnimals()
        {
            foreach (var animal in _animals)
            {
                animal.Feed();
            }
        }
        public void PlayWithAnimal(int index)
        {
            if (index >= 0 && index < _animals.Count)
            {
                Animal animal = _animals[index];
                animal.Play();
            }
            else
            {
                Console.WriteLine("Неверный номер животного.");
            }
        }

        
        
    }
    
    
    //Далее класс зоопрака для управления всем

    public class Zoo
    {
        
        private List<Enclosure> _enclosures = new List<Enclosure>();// Хранение всех вольеров 

        public void AddEnclosure(string name)
        {
            _enclosures.Add(new Enclosure(name));//Добавление нового
            Console.WriteLine($"Вольер {name} добавлен.");
        }
        
        //Метод для добавление нового вольера 
        public Enclosure GetEnclosure(string name)
        {
            return _enclosures.Find(e => e.Name == name);
        }
        
        //Метод вывода списков всех вольеров 
        public void ListEnclosures()
        {
            Console.WriteLine("Список всех вольеров");
            for (int i = 0; i < _enclosures.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_enclosures[i].Name}"); // Вывод названия вольера 
            }
        }
        // Метод получения получения вольера по индексу 

        public Enclosure GetEnclosureByIndex(int index)
        {
            if (index >= 0 && index < _enclosures.Count) //
            {
                return _enclosures[index];//Возвращение вольера по индексу 
            }
            return null; // Ничего не возвращаем если индекс неверный 
        }

        // Метод для кормления животных во всех вольерах 
        public void FeedAllEnclosures()
        {
            foreach (var enclosure in _enclosures)
            {
                enclosure.FeedAllAnimals();
            }
        }
        
        
    }
     


    
    
    
}