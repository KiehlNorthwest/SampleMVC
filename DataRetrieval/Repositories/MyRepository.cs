using ExcelDataReader;
using SampleMVC.Data.Entities;
using SampleMVC.Data.Types;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMVC.Data.Repositories
{
    public class MyRepository : IDisposable, IMyRepository
    {
        public PagedSearchResponseDto<List<PersonSearchResultDto>> SearchPeople(PagedSearchDto dto)
        {
            using (MyDbContext context = new MyDbContext())
            {
                SqlParameter pageSize = new SqlParameter("@PageSize", dto.PageSize ?? (object)DBNull.Value)
                {
                    DbType = System.Data.DbType.Int32
                };
                SqlParameter pageNumber = new SqlParameter("@PageNumber", dto.PageNumber ?? (object)DBNull.Value)
                {
                    DbType = System.Data.DbType.Int32
                };
                SqlParameter orderBy = new SqlParameter("@OrderBy", string.IsNullOrEmpty(dto.OrderByColumn) ? (object)DBNull.Value : dto.OrderByColumn);
                SqlParameter orderAsc = new SqlParameter("@OrderAsc", dto.OrderAscending ?? (object)DBNull.Value);
                SqlParameter totalRows = new SqlParameter("@TotalRows", 0)
                {
                    DbType = System.Data.DbType.Int32,
                    Direction = System.Data.ParameterDirection.Output
                };

                List<PersonSearchResultDto> results = context.Database.SqlQuery<PersonSearchResultDto>("EXEC dbo.GetPeople @PageSize, @PageNumber, @OrderBy, @OrderAsc, @TotalRows OUTPUT",
                    pageSize, pageNumber, orderBy, orderAsc, totalRows).ToList();

                PagedSearchResponseDto<List<PersonSearchResultDto>> response = new PagedSearchResponseDto<List<PersonSearchResultDto>>
                {
                    PageSize = dto.PageSize,
                    PageNumber = dto.PageNumber,
                    OrderByColumn = dto.OrderByColumn,
                    OrderAscending = dto.OrderAscending,
                    TotalRows = (int?)totalRows.Value,
                    Result = results
                };
                return response;
            }
        }

        public ICollection<Person> GetPeople()
        {
            using (MyDbContext context = new MyDbContext())
            {
                return context.People.OrderBy(p => p.Id).ToList();
            }
        }

        public void SavePeople(Stream stream, string fileName, out FileInformation output)
        {
            output = new FileInformation() { FileName = fileName, PercentSaved = 0 };
            List<Person> people = new List<Person>();
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                while (reader.Read())
                {
                    if (reader.FieldCount == 3)
                    {
                        var first = reader.GetValue(0)?.ToString();
                        var second = reader.GetValue(1)?.ToString();
                        var third = reader.GetValue(2)?.ToString();
                        Person p = new Person()
                        {
                            FirstName = first,
                            MiddleName = third,
                            LastName = second

                        };
                        people.Add(p);
                    }
                }

            }

            SavePeople(people: people, output: output);
        }

        public void SavePeople(ICollection<Person> people, FileInformation output = null)
        {
            int batchSize = 100;
            int total = people.Count;
            int batches = total % batchSize == 0 ? total / batchSize : total / batchSize + 1;
            FileInformation fi = new FileInformation();

            for (int i = 0; i < batches; i++)
            {
                using (MyDbContext context = new MyDbContext())
                {
                    if (output != null)
                    {
                        //Note: There is a risk of multiple files with the same name being uploaded at the same time
                        fi = context.FileInformationRecords.FirstOrDefault(f => f.FileName == output.FileName);
                        if (fi == null)
                        {
                            fi = new FileInformation();
                            context.FileInformationRecords.Add(fi);
                        }
                        fi.FileName = output.FileName;
                    }
                    fi.PercentSaved = (int)Math.Floor(((double)i / batches) * 100);

                    var peopleToSave = people
                        .Skip(batchSize * i)
                        .Take(batchSize)
                        .ToList();

                    context.People.AddRange(peopleToSave);
                    context.SaveChanges();
                    output.Id = fi.Id;
                }
            }
        }
        public FileInformation GetFileInfo(int id)
        {
            using (MyDbContext context = new MyDbContext())
            {
                return context.FileInformationRecords.Find(id);
            }
        }

        public FileInformation GetFileInfo(string fileName)
        {
            using (MyDbContext context = new MyDbContext())
            {
                return context.FileInformationRecords.FirstOrDefault(f=> f.FileName == fileName);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SampleMVCRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
