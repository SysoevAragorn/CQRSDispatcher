using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Activation;
using Ninject.Parameters;

namespace CQRSDispatcher {
	public interface IQueryDispatcher {
		/// <summary>
		/// Runs the query handler registered for the given command type.
		/// If there is no handler for a given query type or there is more than one, this method will throw.
		/// </summary>
		/// <typeparam name="TResult">Type of the query</typeparam>
		/// <param name="query">Instance of the query</param>
		/// <param name="cancellationToken">Optional cancellation token</param>
		/// <returns>Task that resolves to a result of the query handler</returns>
		TResult DispatchAsync<TResult>(IQuery<TResult> query);
	}
	public class AutofacQueryDispatcher : IQueryDispatcher {

		private readonly IKernel _lifetimeScope;

		public AutofacQueryDispatcher(IKernel lifetimeScope) {
			_lifetimeScope = lifetimeScope;
		}

		public TResult DispatchAsync<TResult>(IQuery<TResult> query) {
			TryGetSyncHandler(_lifetimeScope, query, out var syncHandler);

			object result;
			//if (asyncHandlerExists)
			//{
			//	result = asyncHandler
			//		.GetType()
			//		.GetRuntimeMethod("HandleAsync", new[] { query.GetType(), typeof(CancellationToken) })
			//		.Invoke(asyncHandler, new object[] { query, cancellationToken });

			//	return await ((Task<TResult>)result).ConfigureAwait(false);
			//}
			result = syncHandler.GetType().GetRuntimeMethod("Handle", new[] { query.GetType() }).Invoke(syncHandler, new object[] { query });
			return (TResult)result;

		}

		private static void TryGetSyncHandler<TResult>(IKernel scope, IQuery<TResult> query, out object handler) {
			var asyncGenericType = typeof(IQueryHandler<,>);
			var closedAsyncGeneric = asyncGenericType.MakeGenericType(query.GetType(), typeof(TResult));
			IRequest request = scope.CreateRequest(closedAsyncGeneric, null, new Parameter[0], true, true);
			handler = scope.Resolve(request).SingleOrDefault();
		}

		private static string GetCommandName(object command) {
			return command.GetType().Name;
		}
	}
}
