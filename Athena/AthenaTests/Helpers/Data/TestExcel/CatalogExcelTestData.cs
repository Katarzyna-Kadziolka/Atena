﻿using Athena.Data;

namespace AthenaTests.Helpers.Data.TestExcel {
    public class CatalogExcelTestData {
        public string Title { get; set; }
        public string Author => $"{AuthorFirstName} {AuthorLastName}";
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string Series { get; set; }
        public string SeriesName { get; set; }
        public int VolumeNumber { get; set; }
        public string PublishingHouse { get; set; }
        public string Year { get; set; }
        public string Town { get; set; }
        public string ISBN { get; set; }
        public string Language { get; set; }
        public Language LanguageEnum { get; set; }
        public string StoragePlace { get; set; }
        public string Comment { get; set; }
        public string ColorCode { get; set; }
    }
}