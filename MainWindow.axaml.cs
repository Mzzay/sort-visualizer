using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using sort_visualizer.Utils;

namespace sort_visualizer
{

    public partial class MainWindow : Window
    {
        public DockPanel dock;
        public Choice userChoice = Choice.Random;
        public ESortType sortType = ESortType.Bubble;
        public static int numberGeneration = 100;
        public int[] arrayValue;
        public char separator = ';';
        public IFile? file;
        public int[] previousArray;
        
        public IControl homeComponent;
        public bool isSorting = false;
        
        public enum ESortType
        {
            Bubble = 0,
            Insertion,
        }
        public enum Choice
        {
            Random,
            Custom
        }
        
        public MainWindow()
        {
            InitializeComponent();
            this.homeComponent = this.FindControl<Border>("GraphContent").Child;
            // home button
            Button homeButton = this.FindControl<Button>("homeButton");
            homeButton.Click += (s, a) =>
            {
                this.isSorting = false;
                this.FindControl<Border>("GraphContent").Child = this.homeComponent; 
            };
            
            // choice button action
            Button RandomChoice = this.FindControl<Button>("RandomChoice");
            Button CustomChoice = this.FindControl<Button>("CustomChoice");
            setBox();
            
            RandomChoice.Click += delegate
            {
                this.userChoice = Choice.Random;
                setBox();
            };
            
            CustomChoice.Click += delegate
            {
                this.userChoice = Choice.Custom;
                setBox();
            };
            
            // sort type
            Button sortTypeBtn = this.FindControl<Button>("SortType");
            sortTypeBtn.Click += (s, a) =>
            {
                this.sortType = (ESortType) (((int) this.sortType + 1) % Enum.GetNames(typeof(ESortType)).Length);
                setSortType();
            };
            
            setSortType();
            
            // button openfile
            Button OpenFile = this.FindControl<Button>("OpenFile");
            OpenFile.Click += openFile;
            
            // button statr
            Button startButton = this.FindControl<Button>("StartButton");
            startButton.Click += startSort;
        }

        private void setBox()
        {
            Button RandomChoice = this.FindControl<Button>("RandomChoice");
            Button CustomChoice = this.FindControl<Button>("CustomChoice");
            Border RandomBox = this.FindControl<Border>("RandomBox");
            Border CustomBox = this.FindControl<Border>("CustomBox");
            
            switch (this.userChoice)
            {
                case Choice.Random:
                    // border
                    RandomBox.BorderBrush = Brushes.White;
                    CustomBox.BorderBrush = null;
                
                    // foreground
                    RandomChoice.Foreground = Brushes.White;
                    CustomChoice.Foreground = Brushes.Gray;
                    
                    // buttonContent
                    RandomChoice.Classes.Add("isActive");
                    CustomChoice.Classes.Remove("isActive");
                    break;
                case Choice.Custom:
                    // border
                    CustomBox.BorderBrush = Brushes.White;
                    RandomBox.BorderBrush = null;
                
                    // foreground
                    CustomChoice.Foreground = Brushes.White;
                    RandomChoice.Foreground = Brushes.Gray;
                    
                    // buttonContent
                    CustomChoice.Classes.Add("isActive");
                    RandomChoice.Classes.Remove("isActive");
                    break;
            }
        }

        private void setSortType()
        {
            TextBlock textSortType = this.FindControl<TextBlock>("TextSortButton");
            textSortType.Text = this.sortType.ToString();
        }
        
        private void startSort(object s, RoutedEventArgs e)
        {
            this.isSorting = true;
            
            switch (sortType)
            {
                case ESortType.Bubble:
                    BubbleSort();
                    break;
                case ESortType.Insertion:
                    InsertionSort();
                    break;
            }
        }
        
        // open file
        private async void openFile(object s, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filters.Add(new FileDialogFilter() { Extensions = { "txt" } });
            string[]? result = await openFileDialog.ShowAsync(new Window());
            if (result != null)
            {
                string path = result.FirstOrDefault();
                IFile importedFile = new IFile(path, this.separator);
                importedFile.ParseContent();
                this.file = importedFile;
                this.previousArray = importedFile.arrayValue;
                this.FindControl<TextBlock>("ImportFileText").Text = importedFile.fileName;
            }
        }
        
        private void GenerateChart()
        {
            if (!isSorting) return; 
            
            this.FindControl<Border>("GraphContent").Child = null;
            DockPanel customDock = new DockPanel
            {
                Height = 600,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            
            foreach (int value in this.arrayValue)
            {
                customDock.Children.Add(GenerateBar(value));
            }
            this.FindControl<Border>("GraphContent").Child = customDock;
        }
        
        private Grid GenerateBar(int value)
        {
            return new Grid
            {
                Height = value,
                Width = 5,
                Background = Brushes.White,
                Margin = new Thickness(2),
                VerticalAlignment = VerticalAlignment.Bottom
            };
        }
        
        // generate random ArrayValue
        private static int[] InitializeArray(int n, int max)
        {
            List<int> totalValue = new List<int>();
            for (int i = 0; i < n; i++)
                totalValue.Add(new Random().Next(max));
            
            return totalValue.ToArray();
        }
        
        // setup arrayValue according user's choice before sorting
        private bool SetArrayValue()
        {
            switch (this.userChoice)
            {
                case Choice.Random:
                    this.arrayValue = InitializeArray(numberGeneration, 700);
                    return true;
                case Choice.Custom:
                    if (file == null)
                        return false;
                    
                    this.arrayValue = new int[this.previousArray.Length];
                    this.previousArray.CopyTo(this.arrayValue,0);
                    return true;
            }

            return false;
        }
        
        // BUBBLE SORT
        private async void BubbleSort()
        {
            if (!SetArrayValue()) return; 
            this.FindControl<Border>("GraphContent").Child = dock;
            
            int delay = this.arrayValue.Length > 20 ? 10 : 100;
            GenerateChart();
            for (int i = 0; i < this.arrayValue.Length; i++)
            {
                for (int j = 0; j < this.arrayValue.Length - 1; j++)
                {
                    if (this.arrayValue[j] > this.arrayValue[j + 1])
                    {
                        if (!isSorting) return;
                        (this.arrayValue[j], this.arrayValue[j + 1]) = (this.arrayValue[j + 1], this.arrayValue[j]);
                        await Task.Delay(delay);
                        GenerateChart();
                    }
                }
            }
        }
        
        // INSERTION SORT
        private async void InsertionSort()
        {
            if (!SetArrayValue()) return; 
            this.FindControl<Border>("GraphContent").Child = dock;
            
            int delay = this.arrayValue.Length > 20 ? 10 : 100;
            GenerateChart();
            for (int i = 1; i < this.arrayValue.Length; ++i)
            {
                for (int j = i; j > 0 && this.arrayValue[j] < this.arrayValue[j - 1]; --j)
                {
                    if (!isSorting) return;
                    (this.arrayValue[j-1], this.arrayValue[j]) = (this.arrayValue[j], this.arrayValue[j-1]);
                    await Task.Delay(delay);
                    GenerateChart();
                }
            }
        }
    }
}