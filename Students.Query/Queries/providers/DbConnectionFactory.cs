﻿using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Query.Queries.providers
{

	public class SqlConnectionFactory : IDbConnectionFactory
	{
		private readonly string _connectionString;

		public SqlConnectionFactory(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
		{
			var connection = new SqlConnection(_connectionString);
			await connection.OpenAsync(token);
			return connection;
		}
	}
}
