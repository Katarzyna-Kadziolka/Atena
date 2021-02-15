﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using static System.Collections.IEnumerable;

namespace Athena.Data
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int? PublishmentYear { get; set; }
        public Language Language { get; set; }
        public string ISBN { get; set; }
        public string Comment { get; set; }
        public int? VolumeNumber { get; set; }

        public virtual ICollection<Author> Authors { get; set; }
        public virtual Series Series { get; set; }
        public virtual PublishingHouse PublishingHouse { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual StoragePlace StoragePlace { get; set; }
        public virtual Borrowing Borrowing { get; set; }

        //public override string ToString()
        //{
        //    string authorsList="";
        //    int authorsCount = Authors.Count - 1;
        //    foreach (var author in Authors)
        //    {
        //        if (string.IsNullOrWhiteSpace(author.FirstName))
        //        {
        //            authorsList += author.LastName;
        //        }
        //        else
        //        {
        //            authorsList += $"{author.FirstName} {author.LastName}";
        //        }
        //        if (authorsCount > 0)
        //        {
        //            authorsList += ", ";
        //            authorsCount--;
        //        }
        //    }
            
        //    return authorsList;
        //}

    }
}
