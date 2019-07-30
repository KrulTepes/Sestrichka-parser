using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using Сестричка_парсер.Core;
using Сестричка_парсер.Core.Vuz;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Xml.Serialization;

namespace Сестричка_парсер
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        ParserWorker<string[]> parser;
        bool isRunning = false;
        const int startPage = 1;
        const int endPage = 11;
        int currentPage;
        List<Abiturient> listCollection;
        List<Abiturient> listCollectionPrevious;
        List<Abiturient> listab;
        List<Abiturient> listTarget;
        List<Abiturient> listSpecial;

        private void LoadInternet(object sender, RoutedEventArgs e)
        {
            if (isRunning || listCollection != null)
            {
                if (parser.IsActive) parser.Abort();
                listAbiturient.ItemsSource = null;
                listCollection.Clear();
                listab.Clear();
                listTarget.Clear();
                listSpecial.Clear();
            }
            else
            {
                listCollection = new List<Abiturient>();
                listab = new List<Abiturient>();
                listTarget = new List<Abiturient>();
                listSpecial = new List<Abiturient>();
            }
            currentPage = 1;
            parser = new ParserWorker<string[]>(new VuzParser());
            parser.OnNewData += Parser_OnNewData;
            parser.Settings = new VuzSettings(startPage, int.Parse(endPageTB.Text));
            parser.Settings.BaseUrl = linkTB.Text;
            parser.Start();
            isRunning = true;
        }
        private void Parser_OnNewData(object arg1, string[] data)
        {
            processingTB.Content = $"Page processing: {currentPage}/{endPageTB.Text}";

            int j = 0;
            while (j < data.Length)
            {
                if (data[j + 3] == "о")
                {
                    Abiturient ab = new Abiturient();
                    ab.obj = data[j + 1] == fioTB.Text.Replace(" ", "\u00A0") ? "*" : "";
                    ab.New = "";
                    ab.Num = 0;
                    ab.FIO = data[j + 1];
                    ab.AllBalls = int.Parse(data[j + 6]);
                    ab.Doc = "Оригинал";
                    ab.SpecialRights = data[j + 10] != "нет" ? "Есть" : "нет";
                    ab.TargetDirection = data[j + 11] != "нет" ? "Есть" : "нет";
                    if (ab.SpecialRights == "Есть") listSpecial.Add(ab);
                    else if (ab.TargetDirection == "Есть") listTarget.Add(ab);
                    else listab.Add(ab);
                }
                j += 13;
            }

            if (currentPage == int.Parse(endPageTB.Text))
            {
                int countPublicPlaces = int.Parse(countPublicPlacesTB.Text);
                int countSpecialPlaces = int.Parse(countSpecialPlacesTB.Text);
                int cointTargetPlaces = int.Parse(countTargetPlacesTB.Text);

                listab.OrderByDescending(ab => ab.AllBalls);
                if (listab.Count > countPublicPlaces) listab.RemoveRange(countPublicPlaces, listab.Count - countPublicPlaces);

                listSpecial.OrderByDescending(ab => ab.AllBalls);
                if (listSpecial.Count > countSpecialPlaces) listSpecial.RemoveRange(countSpecialPlaces, listSpecial.Count - countSpecialPlaces);
                listab.AddRange(listSpecial);

                listTarget.OrderByDescending(ab => ab.AllBalls);
                if (listTarget.Count > cointTargetPlaces) listTarget.RemoveRange(cointTargetPlaces, listTarget.Count - cointTargetPlaces);
                listab.AddRange(listTarget);

                listab.OrderByDescending(ab => ab.AllBalls);

                j = 1;
                foreach (var item in listab)
                {
                    item.Num = j;
                    listCollection.Add(item);
                    j++;
                }

                listAbiturient.ItemsSource = null;
                listAbiturient.ItemsSource = listCollection;
                isRunning = false;
            }

            currentPage++;
        }
        private void SaveResults(object sender, RoutedEventArgs e)
        {
            if (listCollection != null)
            {
                CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string directory = dialog.FileName;
                    XmlSerializer formatter = new XmlSerializer(typeof(List<Abiturient>));
                    string path = $@"{directory}\{DateTime.Now.ToString().Replace('/', '.').Replace(':', '-')}.xml";
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        formatter.Serialize(fs, listCollection);
                    }

                    path = path.Replace("xml", "txt");
                    using (StreamWriter fs = new StreamWriter(path, false, System.Text.Encoding.Default))
                    {
                        string str = "";
                        foreach (var item in listCollection)
                        {
                            str += $"{item.obj}\t{item.Num}\t{item.FIO}\t{item.AllBalls}\t{item.Doc}\t{item.SpecialRights}\t{item.TargetDirection}\n";
                        }
                        fs.Write(str);
                    }
                }
            }
            else MessageBox.Show("Загрузите список");
        }
        private void CompareResults(object sender, RoutedEventArgs e)
        {
            if (listCollection != null && !isRunning)
            {
                CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                dialog.Filters.Add(new CommonFileDialogFilter("Files", "*.xml"));
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string filePath = dialog.FileName;
                    XmlSerializer formatter = new XmlSerializer(typeof(List<Abiturient>));

                    using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                    {
                        listCollectionPrevious = (List<Abiturient>)formatter.Deserialize(fs);
                    }
                }

                for (int i = 0; i < listCollection.Count; i++)
                {
                    bool isFind = false;
                    for (int j = 0; j < listCollectionPrevious.Count; j++)
                    {
                        if (listCollection[i].FIO == listCollectionPrevious[j].FIO)
                        {
                            isFind = true;
                        }
                    }
                    if (!isFind)
                    {
                        listCollection[i].New = "NEW";
                    }
                }
                listAbiturient.ItemsSource = null;
                listAbiturient.ItemsSource = listCollection;
            }
            else MessageBox.Show("Загрузите список");
        }
        private void LoadResults(object sender, RoutedEventArgs e)
        {
            if (isRunning)
            {
                parser.Abort();
                listCollection.Clear();
                listab.Clear();
                listTarget.Clear();
                listSpecial.Clear();
            }
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("Files", "*.xml"));
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string filePath = dialog.FileName;
                XmlSerializer formatter = new XmlSerializer(typeof(List<Abiturient>));

                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    listCollection = (List<Abiturient>)formatter.Deserialize(fs);

                    listAbiturient.ItemsSource = null;
                    listAbiturient.ItemsSource = listCollection;
                }
            }
        }
        private void ShowHelp(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Чтобы получить помощь нажмите 'Cправка'\n\n\t\tКак работать с программой:\n1)Заполните все поля(поля с особыми и целевыми местами можно не трогать если их нет). Адрес должен выглядеть также как в примере. ФИО заполняется также как оно будет в списке(заполняется чтобы 'подсветить' тебя в списке, необязательно к заполнению)\n2)Нажмите 'Загрузить из интернета'\n3) Дождитесь завершения загрузки данных(прогресс можно увидеть сверху 'Page processing: n/m' где 'n' текущая страница, а 'm' все страницы)\n4) После завершения загрузки появится таблица с результатом\n\n\t\tКолонки:\n'*' - Показывает тебя\n'New' - показывает новоприбывших(работает только при сравнении)\n'№' - Показывает номер абитуриента\n'ФИО' - ФИО абитуриента\n'Общие баллы' - Сумма баллов по всем профильным предметам\n 'Оригинал/Копия' - документ который принёс абитуриент(априори оригинал)\n'Особые квоты' - Сироты и инвалиды\n'Целевое направление' - абитуриенты с целевым направлением\n\n\t\tНижняя панель:\n'Загрузить' - загружает таблицу из xml файла\n'Сохранить' - сохраняет два файла, txt и xml в выбранную директорию\n'Сравнить' - считывает выбранный предыдущий сейв и ищет новых людей, помечает их в поле 'New'(использовать только когда загруженна свежая версия, а сравнивается с старой)\n\nНа этом справка законченна, удачного пользования))\nP.S. Выводятся только люди принёсшие оригиналы");
        }
        private void SaveSearchTemplate(object sender, RoutedEventArgs e)
        {
            List<string> source = new List<string>();
            source.Add(fioTB.Text);
            source.Add(linkTB.Text);
            source.Add(endPageTB.Text);
            source.Add(countPublicPlacesTB.Text);
            source.Add(countSpecialPlacesTB.Text);
            source.Add(countTargetPlacesTB.Text);

            string directory = @"C:\Windows\Temp\Сестричка-парсер\";
            string fileName = "settings.xml";
            string path = directory + fileName;

            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            XmlSerializer formatter = new XmlSerializer(typeof(List<string>));
            using (FileStream fs = new FileStream(path, FileMode.CreateNew))
            {
                formatter.Serialize(fs, source);
            }
        }
        private void LoadSettings()
        {
            string directory = @"C:\Windows\Temp\Сестричка-парсер\";
            string fileName = "settings.xml";
            string path = directory + fileName;
            if (Directory.Exists(directory) && File.Exists(path)) 
            {
                XmlSerializer formatter = new XmlSerializer(typeof(List<string>));
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    List<string> source = (List<string>)formatter.Deserialize(fs);
                    fioTB.Text = source[0];
                    linkTB.Text = source[1];
                    endPageTB.Text = source[2];
                    countPublicPlacesTB.Text = source[3];
                    countSpecialPlacesTB.Text = source[4];
                    countTargetPlacesTB.Text = source[5];
                }
            }

        }
        private void ResetSearchTemplate(object sender, RoutedEventArgs e)
        {
            string directory = @"C:\Windows\Temp\Сестричка-парсер\";
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            if (Directory.Exists(directory)) dirInfo.Delete(true);
        }
    }
}
