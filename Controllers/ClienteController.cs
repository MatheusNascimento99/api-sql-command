using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using ProjetoClients_Teste.Models;

namespace ProjetoClients_Teste.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IConfiguration _configuration;


        public ClienteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var clientes = new List<Cliente>();
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")); 
            connection.Open();
            var command = new SqlCommand("SELECT * FROM Clientes", connection);
            var reader = command.ExecuteReader(); 

            while (reader.Read())
            {
                clientes.Add(new Cliente
                {
                    Id = (int)reader["Id"],
                    Nome = reader["Nome"].ToString(),
                    Email = reader["Email"].ToString(),
                    DataNascimento = (DateTime)reader["DataNascimento"]
                });
            }
            return Ok(clientes);
        }

        [HttpPost]
        public IActionResult Post(Cliente cliente)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            var command = new SqlCommand("INSERT INTO Clientes (Nome, Email, DataNascimento) VALUES (@nome, @email, @data)", connection);
            command.Parameters.AddWithValue("@nome", cliente.Nome);
            command.Parameters.AddWithValue("@email", cliente.Email);
            command.Parameters.AddWithValue("@data", cliente.DataNascimento);

            command.ExecuteNonQuery();
            return Ok("Cliente inserido com sucesso!");
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Cliente cliente)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            var command = new SqlCommand("UPDATE Clientes SET Nome=@nome, Email=@email, DataNascimento=@data WHERE Id=@id", connection);

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@nome", cliente.Nome);
            command.Parameters.AddWithValue("@email", cliente.Email);
            command.Parameters.AddWithValue("@data", cliente.DataNascimento);
            command.ExecuteNonQuery();
            return Ok("Cliente atualizado com sucesso!");
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Cliente cliente)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            var command = new SqlCommand("UPDATE  Clientes SET Nome=@nome, Email=@email, DataNascimento=@data WHERE Id=@id", connection);

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@nome", cliente.Nome);
            command.Parameters.AddWithValue("@email", cliente.Email);
            command.Parameters.AddWithValue("@data", cliente.DataNascimento);

            command.ExecuteNonQuery();
            return Ok("Campo atualizado com sucesso!");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
            var command = new SqlCommand("DELETE FROM Clientes WHERE Id=@id", connection);

            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            return Ok("Cliente removido com sucesso!");
        }


    }
}