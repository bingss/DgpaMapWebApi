using DgpaMapWebApi.Dto;
using DgpaMapWebApi.Dtos;
using DgpaMapWebApi.Interface;
using DgpaMapWebApi.Models;
using Microsoft.Extensions.Logging;

namespace DgpaMapWebApi.Service
{
    public class UpdateDateService : IUpdateDateService
    {
        private readonly DgpaDbContext _dgpaDb;

        public UpdateDateService(DgpaDbContext dgpaDb)
        {
            _dgpaDb = dgpaDb;
        }

        public DateOnly? GetLastestDate()
        {
            var latestDate = _dgpaDb.UpdateDate
                        .Max(x => x.LastUpdateDate);

            return latestDate;
        }
    }
}
