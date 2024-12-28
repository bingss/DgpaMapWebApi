using DgpaMapWebApi.Dto;
using DgpaMapWebApi.Dtos;

namespace DgpaMapWebApi.Interface
{
    public interface IUpdateDateService
    {
        public DateOnly? GetLastestDate();

    }
}
