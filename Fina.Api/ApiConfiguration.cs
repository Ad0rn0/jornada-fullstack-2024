using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fina.Api
{
	public static class ApiConfiguration
	{
		public const string UserId = "jornada@balta.io";
		public static string ConnectionString { get; set; } = string.Empty;
		public static string CorsPolicyName = "wasm";
	}
}