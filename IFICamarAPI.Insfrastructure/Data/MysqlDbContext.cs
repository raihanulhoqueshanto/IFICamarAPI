using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using IFICamarAPI.Application.Pagings;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace IFICamarAPI.Insfrastructure.Data
{
    public class MysqlDbContext : IDisposable
    {
        private readonly IConfiguration _configuration;
        private MySqlConnection _connection;

        public MysqlDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        private async Task OpenConnectionAsync()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                await _connection.OpenAsync();
            }
        }

        private async Task CloseConnectionAsync()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                await _connection.CloseAsync();
            }
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

        public async Task<PagedList<T>> GetPagedListAsync<T>(int currentPage, int itemsPerPage, string sqlQuery, DynamicParameters parameter)
        {
            await using var connection = _connection;
            try
            {
                await OpenConnectionAsync();
                DefaultTypeMap.MatchNamesWithUnderscores = true;
                var items = await connection.QueryAsync<T>(sqlQuery, parameter);
                string totalItemQuery = $"SELECT TotalItems FROM ({sqlQuery})  AS result  ORDER BY TotalItems LIMIT 1";
                int totalItem = connection.QueryFirstOrDefault<int>(totalItemQuery, parameter);
                if (itemsPerPage == 0)
                {
                    itemsPerPage = totalItem;
                }
                await CloseConnectionAsync();
                return new PagedList<T>(items.AsList(), currentPage, itemsPerPage, totalItem);
            }
            catch (Exception ex)
            {
                await CloseConnectionAsync();
                throw new Exception(ex.Message);
            }
            finally
            {
                await CloseConnectionAsync();
            }
        }

        public async Task<IEnumerable<T>> GetListAsync<T>(string sqlQuery, DynamicParameters parameter)
        {
            await using var connection = _connection;
            try
            {
                await OpenConnectionAsync();
                DefaultTypeMap.MatchNamesWithUnderscores = true;
                var result = await connection.QueryAsync<T>(sqlQuery, parameter);
                await CloseConnectionAsync();
                return result;
            }
            catch (Exception ex)
            {
                await CloseConnectionAsync();
                throw new Exception(ex.Message);
            }
            finally
            {
                await CloseConnectionAsync();
            }
        }

        public async Task<T> GetFirstOrDefaultAsync<T>(string sqlQuery, DynamicParameters parameter)
        {
            await using var connection = _connection;
            try
            {
                await OpenConnectionAsync();
                DefaultTypeMap.MatchNamesWithUnderscores = true;
                var result = await connection.QueryFirstOrDefaultAsync<T>(sqlQuery, parameter);
                await CloseConnectionAsync();
                return result;
            }
            catch (Exception ex)
            {
                await CloseConnectionAsync();
                throw new Exception(ex.Message);
            }
            finally
            {
                await CloseConnectionAsync();
            }
        }
    }
}
