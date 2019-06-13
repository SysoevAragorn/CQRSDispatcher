using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSDispatcher {
	public interface IQuery<out IQueryResult> {

	}
	public interface IQueryResult {
	}
	public class TestQuery : IQuery<TestQueryResult> {
		public int QueryParameter { get; set; }
	}

	public class TestQueryResult: IQueryResult {
		public int MyProperty {
			get; set;
		}
	}
}
