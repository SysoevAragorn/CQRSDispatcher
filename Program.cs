using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSDispatcher {
	class Program {
		static void Main(string[] args) {

			var a = GetResult();
		}

		private static IQuery<IQueryResult> GetQuery() => new TestQuery();
		private static TestQueryResult GetResult() {
			var scope = DependencyFactory.GetResolver(new CustomModule());
			var dispatcher = new AutofacQueryDispatcher(scope);
			dynamic query = GetQuery();
			return dispatcher.DispatchAsync<TestQueryResult>(query);
		}
	}
}
