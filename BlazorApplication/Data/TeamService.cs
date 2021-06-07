using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorApplication.Models;

namespace BlazorApplication.Data
{
    public class TeamService
    {
        private readonly HttpClient _httpClient;

        public TeamService(HttpClient client) 
        { 
            _httpClient = client;
        }
        
        // GET REQUESTS
        public async Task<List<Team>> GetAllTeamsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/teams");
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}, {response.ReasonPhrase}");
            
            string responseContent = await response.Content.ReadAsStringAsync();
            
            List<Team> allTeams = JsonSerializer.Deserialize<List<Team>>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return allTeams;;
        }
        
        
        // POST REQUESTS
        public async Task AddNewTeamAsync(Team newTeam)
        {
            var jsonTeam = new StringContent(
                JsonSerializer.Serialize(newTeam, typeof(Team), new JsonSerializerOptions(JsonSerializerDefaults.Web)), Encoding.UTF8, "application/json");

            using var httpResponse = await _httpClient.PostAsync("/teams", jsonTeam);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception(httpResponse.Content.ReadAsStringAsync().Result);
            }
        }


        public async Task AddNewPlayerAsync(Player newPlayer, string selectedTeam)
        {
            var jsonPlayer = new StringContent(
                JsonSerializer.Serialize(newPlayer, typeof(Player), new JsonSerializerOptions(JsonSerializerDefaults.Web)), Encoding.UTF8, "application/json");

            using var httpResponse = await _httpClient.PostAsync($"/players/{selectedTeam}", jsonPlayer);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception(httpResponse.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task DeletePlayerAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"/players/{id}");
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}, {response.ReasonPhrase}");
        }
    }
}