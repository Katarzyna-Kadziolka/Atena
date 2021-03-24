﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Athena.Data;
using Athena.Data.Books;
using Athena.Data.Series;
using Athena.EnumLocalizations;
using Athena.Windows;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;


namespace Athena {
    public partial class BookFormControl : UserControl {
        public BookView BookView { get; set; }
        public string Title { get; set; }
        public string ButtonContent { get; set; }
        public ICommand ButtonCommand { get; set; }
        private ApplicationDbContext ApplicationDbContext { get; set; }
        public ObservableCollection<Author> Authors { get; set; }
        public ObservableCollection<StoragePlace> StoragePlaces { get; set; }
        public ObservableCollection<PublishingHouse> PublishingHouses { get; set; }
        public ObservableCollection<Series> SeriesList { get; set; }

        public BookFormControl(string title, string buttonContent, Book book) {
            InitializeComponent();
            Title = title;
            ButtonContent = buttonContent;
            this.DataContext = this;
            BookView = Mapper.Instance.Map<BookView>(book);
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            ApplicationDbContext = new ApplicationDbContext();
            ApplicationDbContext.Authors.Load();
            Authors = new ObservableCollection<Author>(ApplicationDbContext.Authors.Local
                .ToList()
                .OrderBy(a => a.LastName));
            
            ApplicationDbContext.StoragePlaces.Load();
            StoragePlaces = new ObservableCollection<StoragePlace>(ApplicationDbContext.StoragePlaces.Local
                .ToList()
                .OrderBy(a => a.StoragePlaceName));
           
            ApplicationDbContext.PublishingHouses.Load();
            PublishingHouses = new ObservableCollection<PublishingHouse>(ApplicationDbContext.PublishingHouses.Local
                .ToList()
                .OrderBy(a => a.PublisherName));

            ApplicationDbContext.Series.Load();
            SeriesList = new ObservableCollection<Series>(ApplicationDbContext.Series.Local
                .ToList()
                .OrderBy(a => a.SeriesName));
            
            if (!BookView.Authors.IsNullOrEmpty()) {
                AuthorCombobox.SelectedIndex = Authors.IndexOf(BookView.Authors.ToList()[0]);
                if (BookView.Authors.Count > 1) {
                    for (int i = 1; i < BookView.Authors.Count; i++) {
                        AddingAuthorCombobox(this, new RoutedEventArgs());
                        var authorAdding = (AuthorAdding) AuthorsStackPanel.Children[i - 1];
                        var combobox = authorAdding.AuthorComboBox;
                        combobox.SelectedIndex = Authors.IndexOf(BookView.Authors.ToList()[i]);
                    }
                }
            }

            if (!BookView.Categories.IsNullOrEmpty()) {
                CategoriesCombobox.SelectedItem = BookView.Categories.First().Name;
                for (int i = 1; i < BookView.Categories.Count; i++) {
                    AddingCategoryCombobox_Click(this, new RoutedEventArgs());
                    var categoryAdding = (CategoryAdding) CategoriesStackPanel.Children[i - 1];
                    var combobox = categoryAdding.CategoryComboBox;
                    combobox.SelectedItem = BookView.Categories.ToList()[i].Name;
                }
            }

            CategoriesCombobox.ItemsSource = EnumSorter.GetSortedByDescriptions<CategoryName>();
            LanguageComboBox.ItemsSource = EnumSorter.GetSortedByDescriptions<Language>();
        }

        private void AddingAuthorCombobox(object sender, RoutedEventArgs e) {
            var authorAddingUserControl = new AuthorAdding();
            AuthorsStackPanel.Children.Add(authorAddingUserControl);
        }

        private void AddSeries_Click(object sender, RoutedEventArgs e) {
            new AddSeriesWindow().Show();
        }

        private void AddPublisher_Click(object sender, RoutedEventArgs e) {
            new AddPublisherWindow().Show();
        }

        private void AddStoragePlace_Click(object sender, RoutedEventArgs e) {
            new AddStoragePlaceWindow().Show();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e) {
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void AllowOnlyNumbers(object sender, TextCompositionEventArgs e) {
            e.Handled = e.Text.Any(a => !char.IsDigit(a));
        }

        private void AllowPastOnlyNumbers(object sender, DataObjectPastingEventArgs e) {
            if (e.DataObject.GetDataPresent(typeof(string))) {
                string text = (string) e.DataObject.GetData(typeof(string));
                if (text.Any(a => !char.IsDigit(a))) {
                    e.CancelCommand();
                }
            }
            else {
                e.CancelCommand();
            }
        }

        private void AddingCategoryCombobox_Click(object sender, RoutedEventArgs e) {
            var categoryAddingUserControl = new CategoryAdding();
            CategoriesStackPanel.Children.Add(categoryAddingUserControl);
        }


        private void AuthorCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.RemovedItems.Count > 0) {
                BookView.Authors.Remove((Author) e.RemovedItems[0]);
            }
            
            BookView.Authors.Add((Author) e.AddedItems[0]);
        }

        private void CategoriesCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.RemovedItems.Count > 0) {
                BookView.Categories.Remove((Category) e.RemovedItems[0]);
            }

            BookView.Categories.Add((Category) e.AddedItems[0]);
        }
    }
}