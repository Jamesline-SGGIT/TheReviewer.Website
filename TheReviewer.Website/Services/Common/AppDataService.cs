using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static CommonServices.ExcelConversionService.TablesModel;

namespace TheReviewer.Website.Services.Common
{
    public class AppDataService
    {
        private string _status;
        private bool _fileloaded;
        private bool _fileloaded2;
        public MemoryStream CogeMem { get; set; }
        public MemoryStream GecoMem { get; set; }
        public MemoryStream OtherMem { get; set; }
        public MemoryStream FileMem { get; set; }
        public MemoryStream FileMem2 { get; set; }
        public TableModel GecoTable { get; set; }
        public TableModel CogeTable { get; set; }
        public string MasterStatus
        {
            get => _status;

            set
            {
                _status = value;
                NotifyDataChanged();
            }
        }
        public string MasterStatus2
        {
            get => _status;

            set
            {
                _status = value;
                NotifyDataChanged();
            }
        }
        public bool FileLoaded
        {
            get => _fileloaded;

            set
            {
                _fileloaded = value;
                NotifyDataChanged();
            }
        }
        public bool FileLoaded1
        {
            get => _fileloaded;

            set
            {
                _fileloaded = value;
                NotifyDataChanged();
            }
        }
        public bool FileLoaded2
        {
            get => _fileloaded2;
            set
            {
                _fileloaded2 = value;
                NotifyDataChanged();
            }
        }

        public event Action OnChange;

        private void NotifyDataChanged() => OnChange?.Invoke();
    }
}
