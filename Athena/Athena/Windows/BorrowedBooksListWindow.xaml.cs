﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Athena.Data.Borrowings;
using Microsoft.EntityFrameworkCore;
using Athena.Data.Books;
using System.Windows.Controls;
using System.Windows;


namespace Athena.Windows {
    public partial class BorrowedBooksListWindow {
        public ObservableCollection<BorrowingView> Borrowings { get; set; }
        public BorrowedBooksListWindow() {
            InitializeComponent();
            this.DataContext = this;
            ApplicationDbContext.Instance.Borrowings
                .Include(a => a.Book)
                .ThenInclude(a => a.StoragePlace)
                .Include(a => a.Book)
                .ThenInclude(a => a.PublishingHouse)
                .Include(a => a.Book)
                .ThenInclude(a => a.Authors)
                .Load();
            Borrowings = Mapper.Instance.Map<ObservableCollection<BorrowingView>>(ApplicationDbContext.Instance.Borrowings.Local.ToObservableCollection());
            BorrowedBookList.ItemsSource = Borrowings;
            if (Borrowings.Count == 0) {
                TextBlock.Visibility = Visibility.Visible;
            }
        }

        private void OpenReturnWindow_Click(object sender, System.Windows.RoutedEventArgs e) {
            Button button = (Button) sender;
            BorrowingView borrowing = (BorrowingView) button.DataContext;
            Book book = borrowing.Book;
            book.Borrowing.Add(Mapper.Instance.Map<Borrowing>(borrowing));
            ReturnWindow returnWindow = new ReturnWindow(book);
            returnWindow.BookReturned += (_, args) => button.Visibility = Visibility.Hidden;
            returnWindow.BookReturned += (sender, e) => Borrowings.Last(b => b.Book.Id == book.Id).ReturnDate = ApplicationDbContext.Instance.Books.Single(b => b.Id == book.Id).Borrowing.Last().ReturnDate;
            returnWindow.BookReturned += (sender, e) => BorrowedBookList.ItemsSource = Borrowings;
            //Borrowings.Single(b => b.Book.Id == book.Id).ReturnDate = ApplicationDbContext.Instance.Borrowings.Single(b => b.Book.Id == book.Id).ReturnDate;
            //Tu zrobić update
            returnWindow.Show();
        }
    }
}