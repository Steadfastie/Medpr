using MedprCore.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MedprBusiness;

public class OpenFDAService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly string _key;
    public OpenFDAService(HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;

        _httpClient.DefaultRequestHeaders.Add(
            HeaderNames.Accept, "application/json");
    }

    public async Task<DrugDTO> GetRandomDrug()
    {
        var path = _configuration["OpenFDA:APIkeyPath"];
        var file = await File.ReadAllTextAsync(path);

        var secret = JsonConvert.DeserializeObject<APIKey>(file);

        _httpClient.BaseAddress = new Uri(
            $"{_configuration["OpenFDA:Base"]}?" +
            $"api_key={secret.Key}&");

        var total = await GetDrugAmount();
        var randomDrug = await GetDrug(total);
        return randomDrug;
    }

    private async Task<DrugDTO> GetDrug(int total)
    {
        try
        {
            var rand = new Random();
            int skip = rand.Next(0, total);

            var initialRequest = new HttpRequestMessage(HttpMethod.Get,
            new Uri(_httpClient.BaseAddress +
            $"search=_exists_:openfda.pharm_class_epc&" +
            $"skip={skip}&limit=1"));

            var response = await _httpClient.SendAsync(initialRequest);
            response.EnsureSuccessStatusCode();
            var responseStream = await response.Content.ReadAsStreamAsync();

            using var sr = new StreamReader(responseStream);
            var data = await sr.ReadToEndAsync();
            var resp = JsonConvert.DeserializeObject<dynamic>(data);

            string respName = resp.results[0].products[0].active_ingredients[0].name.Value.ToLowerInvariant();
            string respGroup = resp.results[0].openfda.pharm_class_epc[0].Value;

            var drug = new DrugDTO()
            {
                Name = $"{Char.ToUpper(respName[0])}{respName[1..]}",
                PharmGroup = respGroup.Replace("[EPC]", "").TrimEnd()
            };
            return drug;
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            var drug = new DrugDTO();
            return drug;
        }

    }

    private async Task<int> GetDrugAmount()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
            new Uri(_httpClient.BaseAddress +
            $"search=_exists_:openfda.pharm_class_epc&" +
            $"limit=1"));

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseStream = await response.Content.ReadAsStreamAsync();

            using var sr = new StreamReader(responseStream);
            var data = await sr.ReadToEndAsync();
            var resp = JsonConvert.DeserializeObject<dynamic>(data);

            var total = (int)resp.meta.results.total.Value;
            return total;
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            return 0;
        }
    }

    private class APIKey
    {
        public string Key { get; set; }
    }

}
