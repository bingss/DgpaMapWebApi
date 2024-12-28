using DgpaMapWebApi.Dto;
using DgpaMapWebApi.Dtos;
using DgpaMapWebApi.Parameter;
using Microsoft.AspNetCore.Mvc;

namespace DgpaMapWebApi.Interface
{
    public interface IJobService
    {
        public Task<List<JobDto>> GetAllDataAsync();
        public Task<List<JobSelectDto>> GetQueryDataAsync(JobSelectParameter? value);
        public Task<int> GetCountAsync();

        public Task<FeatureCollection> GetGeojsonDataAsync();
        public Task<FeatureCollection> GetQueryGeojsonDataAsync(JobSelectParameter? value);
    }
}
