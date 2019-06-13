using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSDispatcher {
	public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> {
		TResult Handle(TQuery query);
	}
	public class TestQueryHandler : IQueryHandler<TestQuery, TestQueryResult> {
		public TestQueryResult Handle(TestQuery query) {
			return new TestQueryResult() { MyProperty = query.QueryParameter };
		}

	}
}
