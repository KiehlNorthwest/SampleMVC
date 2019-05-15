using System.Collections.Generic;
using System.IO;
using SampleMVC.Data.Entities;
using SampleMVC.Data.Types;

namespace SampleMVC.Data.Repositories
{
    public interface IMyRepository
    {
        ICollection<Person> GetPeople();
        void SavePeople(ICollection<Person> people, FileInformation output = null);
        void SavePeople(Stream stream, string fileName, out FileInformation output);
        FileInformation GetFileInfo(int id);
        FileInformation GetFileInfo(string filename);
        PagedSearchResponseDto<List<PersonSearchResultDto>> SearchPeople(PagedSearchDto dto);
    }
}