using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DictionaryApp
{
    class Program
    {
        static List<Dictionary> dictionaries = new List<Dictionary>();
        static string dataFolder = "Dictionaries";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Створюємо папку для словників
            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);

            LoadDictionaries();
            MainMenu();
        }

        static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════╗");
                Console.WriteLine("║        ДОДАТОК \"СЛОВНИКИ\"          ║");
                Console.WriteLine("╚════════════════════════════════════╝");
                Console.WriteLine("\n1. Створити новий словник");
                Console.WriteLine("2. Вибрати словник для роботи");
                Console.WriteLine("3. Показати всі словники");
                Console.WriteLine("4. Видалити словник");
                Console.WriteLine("0. Вихід");
                Console.Write("\nВиберіть пункт меню: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateDictionary();
                        break;
                    case "2":
                        SelectDictionary();
                        break;
                    case "3":
                        ShowAllDictionaries();
                        break;
                    case "4":
                        DeleteDictionary();
                        break;
                    case "0":
                        SaveDictionaries();
                        return;
                    default:
                        Console.WriteLine("\nНевірний вибір!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void CreateDictionary()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║      СТВОРЕННЯ НОВОГО СЛОВНИКА     ║");
            Console.WriteLine("╚════════════════════════════════════╝\n");

            Console.Write("Введіть назву словника: ");
            string name = Console.ReadLine();

            Console.Write("Введіть тип словника (напр. англо-український): ");
            string type = Console.ReadLine();

            var dict = new Dictionary { Name = name, Type = type };
            dictionaries.Add(dict);

            SaveDictionaries();
            Console.WriteLine("\n✓ Словник успішно створено!");
            Console.ReadKey();
        }

        static void SelectDictionary()
        {
            if (dictionaries.Count == 0)
            {
                Console.WriteLine("\nНемає жодного словника. Створіть новий!");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║        ВИБІР СЛОВНИКА              ║");
            Console.WriteLine("╚════════════════════════════════════╝\n");

            for (int i = 0; i < dictionaries.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {dictionaries[i].Name} ({dictionaries[i].Type})");
            }

            Console.Write("\nВиберіть номер словника: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= dictionaries.Count)
            {
                DictionaryMenu(dictionaries[index - 1]);
            }
            else
            {
                Console.WriteLine("\nНевірний номер!");
                Console.ReadKey();
            }
        }

        static void DictionaryMenu(Dictionary dict)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"╔════════════════════════════════════╗");
                Console.WriteLine($"║ СЛОВНИК: {dict.Name}                      ║");
                Console.WriteLine($"║ ТИП: {dict.Type}              ║");
                Console.WriteLine($"╚════════════════════════════════════╝");
                Console.WriteLine($"\nКількість слів: {dict.Entries.Count}\n");
                Console.WriteLine("1. Додати слово");
                Console.WriteLine("2. Знайти переклад");
                Console.WriteLine("3. Редагувати слово");
                Console.WriteLine("4. Видалити слово");
                Console.WriteLine("5. Показати всі слова");
                Console.WriteLine("6. Експортувати слово");
                Console.WriteLine("0. Повернутися до головного меню");
                Console.Write("\nВиберіть пункт меню: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddWord(dict);
                        break;
                    case "2":
                        SearchWord(dict);
                        break;
                    case "3":
                        EditWord(dict);
                        break;
                    case "4":
                        DeleteWord(dict);
                        break;
                    case "5":
                        ShowAllWords(dict);
                        break;
                    case "6":
                        ExportWord(dict);
                        break;
                    case "0":
                        SaveDictionaries();
                        return;
                    default:
                        Console.WriteLine("\nНевірний вибір!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddWord(Dictionary dict)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║         ДОДАВАННЯ СЛОВА            ║");
            Console.WriteLine("╚════════════════════════════════════╝\n");

            Console.Write("Введіть слово: ");
            string word = Console.ReadLine();

            var entry = dict.Entries.FirstOrDefault(e => e.Word.Equals(word, StringComparison.OrdinalIgnoreCase));

            if (entry == null)
            {
                entry = new WordEntry { Word = word };
                dict.Entries.Add(entry);
            }

            Console.WriteLine("\nВведіть переклади (порожній рядок для завершення):");
            int count = 1;
            while (true)
            {
                Console.Write($"Переклад {count}: ");
                string translation = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(translation))
                    break;

                if (!entry.Translations.Contains(translation, StringComparer.OrdinalIgnoreCase))
                {
                    entry.Translations.Add(translation);
                    count++;
                }
                else
                {
                    Console.WriteLine("Такий переклад вже існує!");
                }
            }

            SaveDictionaries();
            Console.WriteLine("\n✓ Слово успішно додано!");
            Console.ReadKey();
        }

        static void SearchWord(Dictionary dict)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║         ПОШУК ПЕРЕКЛАДУ            ║");
            Console.WriteLine("╚════════════════════════════════════╝\n");

            Console.Write("Введіть слово для пошуку: ");
            string word = Console.ReadLine();

            var entry = dict.Entries.FirstOrDefault(e => e.Word.Equals(word, StringComparison.OrdinalIgnoreCase));

            if (entry != null)
            {
                Console.WriteLine($"\n📖 Слово: {entry.Word}");
                Console.WriteLine("Переклади:");
                for (int i = 0; i < entry.Translations.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}. {entry.Translations[i]}");
                }
            }
            else
            {
                Console.WriteLine("\n❌ Слово не знайдено!");
            }

            Console.ReadKey();
        }

        static void EditWord(Dictionary dict)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║       РЕДАГУВАННЯ СЛОВА            ║");
            Console.WriteLine("╚════════════════════════════════════╝\n");

            Console.Write("Введіть слово для редагування: ");
            string word = Console.ReadLine();

            var entry = dict.Entries.FirstOrDefault(e => e.Word.Equals(word, StringComparison.OrdinalIgnoreCase));

            if (entry == null)
            {
                Console.WriteLine("\n❌ Слово не знайдено!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nПоточне слово: {entry.Word}");
            Console.WriteLine("Переклади:");
            for (int i = 0; i < entry.Translations.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {entry.Translations[i]}");
            }

            Console.WriteLine("\n1. Замінити слово");
            Console.WriteLine("2. Додати переклад");
            Console.WriteLine("3. Замінити переклад");
            Console.WriteLine("4. Видалити переклад");
            Console.WriteLine("0. Назад");
            Console.Write("\nВиберіть дію: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("\nВведіть нове слово: ");
                    string newWord = Console.ReadLine();
                    entry.Word = newWord;
                    SaveDictionaries();
                    Console.WriteLine("✓ Слово замінено!");
                    break;

                case "2":
                    Console.Write("\nВведіть новий переклад: ");
                    string newTranslation = Console.ReadLine();
                    if (!entry.Translations.Contains(newTranslation, StringComparer.OrdinalIgnoreCase))
                    {
                        entry.Translations.Add(newTranslation);
                        SaveDictionaries();
                        Console.WriteLine("✓ Переклад додано!");
                    }
                    else
                    {
                        Console.WriteLine("❌ Такий переклад вже існує!");
                    }
                    break;

                case "3":
                    Console.Write("\nВведіть номер перекладу для заміни: ");
                    if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= entry.Translations.Count)
                    {
                        Console.Write("Введіть новий переклад: ");
                        string replacement = Console.ReadLine();
                        entry.Translations[idx - 1] = replacement;
                        SaveDictionaries();
                        Console.WriteLine("✓ Переклад замінено!");
                    }
                    break;

                case "4":
                    if (entry.Translations.Count == 1)
                    {
                        Console.WriteLine("\n❌ Не можна видалити останній переклад!");
                    }
                    else
                    {
                        Console.Write("\nВведіть номер перекладу для видалення: ");
                        if (int.TryParse(Console.ReadLine(), out int delIdx) && delIdx > 0 && delIdx <= entry.Translations.Count)
                        {
                            entry.Translations.RemoveAt(delIdx - 1);
                            SaveDictionaries();
                            Console.WriteLine("✓ Переклад видалено!");
                        }
                    }
                    break;
            }

            Console.ReadKey();
        }

        static void DeleteWord(Dictionary dict)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║         ВИДАЛЕННЯ СЛОВА            ║");
            Console.WriteLine("╚════════════════════════════════════╝\n");

            Console.Write("Введіть слово для видалення: ");
            string word = Console.ReadLine();

            var entry = dict.Entries.FirstOrDefault(e => e.Word.Equals(word, StringComparison.OrdinalIgnoreCase));

            if (entry != null)
            {
                Console.Write($"\nВи впевнені, що хочете видалити '{entry.Word}' з усіма перекладами? (Так/Ні): ");
                string confirm = Console.ReadLine();

                if (confirm.Equals("Так", StringComparison.OrdinalIgnoreCase))
                {
                    dict.Entries.Remove(entry);
                    SaveDictionaries();
                    Console.WriteLine("\n✓ Слово видалено!");
                }
            }
            else
            {
                Console.WriteLine("\n❌ Слово не знайдено!");
            }

            Console.ReadKey();
        }

        static void ShowAllWords(Dictionary dict)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║         ВСІ СЛОВА СЛОВНИКА         ║");
            Console.WriteLine("╚════════════════════════════════════╝\n");

            if (dict.Entries.Count == 0)
            {
                Console.WriteLine("Словник порожній!");
            }
            else
            {
                foreach (var entry in dict.Entries.OrderBy(e => e.Word))
                {
                    Console.WriteLine($"\n📖 {entry.Word}");
                    foreach (var translation in entry.Translations)
                    {
                        Console.WriteLine($"   → {translation}");
                    }
                }
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void ExportWord(Dictionary dict)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║         ЕКСПОРТ СЛОВА              ║");
            Console.WriteLine("╚════════════════════════════════════╝\n");

            Console.Write("Введіть слово для експорту: ");
            string word = Console.ReadLine();

            var entry = dict.Entries.FirstOrDefault(e => e.Word.Equals(word, StringComparison.OrdinalIgnoreCase));

            if (entry != null)
            {
                string fileName = $"Export_{entry.Word}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string filePath = Path.Combine(dataFolder, fileName);

                using (StreamWriter writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine($"Слово: {entry.Word}");
                    writer.WriteLine($"Словник: {dict.Name} ({dict.Type})");
                    writer.WriteLine($"Дата експорту: {DateTime.Now}");
                    writer.WriteLine("\nПереклади:");
                    for (int i = 0; i < entry.Translations.Count; i++)
                    {
                        writer.WriteLine($"{i + 1}. {entry.Translations[i]}");
                    }
                }

                Console.WriteLine($"\n✓ Слово експортовано до файлу: {fileName}");
            }
            else
            {
                Console.WriteLine("\n❌ Слово не знайдено!");
            }

            Console.ReadKey();
        }

        static void ShowAllDictionaries()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║         ВСІ СЛОВНИКИ               ║");
            Console.WriteLine("╚════════════════════════════════════╝\n");

            if (dictionaries.Count == 0)
            {
                Console.WriteLine("Немає жодного словника!");
            }
            else
            {
                for (int i = 0; i < dictionaries.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {dictionaries[i].Name}");
                    Console.WriteLine($"   Тип: {dictionaries[i].Type}");
                    Console.WriteLine($"   Кількість слів: {dictionaries[i].Entries.Count}\n");
                }
            }

            Console.WriteLine("Натисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void DeleteDictionary()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║       ВИДАЛЕННЯ СЛОВНИКА           ║");
            Console.WriteLine("╚════════════════════════════════════╝\n");

            if (dictionaries.Count == 0)
            {
                Console.WriteLine("Немає жодного словника!");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < dictionaries.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {dictionaries[i].Name} ({dictionaries[i].Type})");
            }

            Console.Write("\nВиберіть номер словника для видалення: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= dictionaries.Count)
            {
                var dict = dictionaries[index - 1];
                Console.Write($"\nВи впевнені, що хочете видалити '{dict.Name}'? (Так/Ні): ");
                string confirm = Console.ReadLine();

                if (confirm.Equals("Так", StringComparison.OrdinalIgnoreCase))
                {
                    dictionaries.RemoveAt(index - 1);
                    SaveDictionaries();
                    Console.WriteLine("\n✓ Словник видалено!");
                }
            }
            else
            {
                Console.WriteLine("\nНевірний номер!");
            }

            Console.ReadKey();
        }

        static void SaveDictionaries()
        {
            string json = JsonSerializer.Serialize(dictionaries, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path.Combine(dataFolder, "dictionaries.json"), json, System.Text.Encoding.UTF8);
        }

        static void LoadDictionaries()
        {
            string filePath = Path.Combine(dataFolder, "dictionaries.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
                dictionaries = JsonSerializer.Deserialize<List<Dictionary>>(json) ?? new List<Dictionary>();
            }
        }
    }
}
