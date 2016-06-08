using Skeleton.Common;
using Skeleton.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton.Infrastructure.Service
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IDependencyResolver _resolver;

        public CommandDispatcher(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public void Execute<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var handler = _resolver.Resolve<ICommandHandler<TCommand>>();

            if (handler == null)
            {
                throw new ArgumentNullException(typeof(TCommand).Name);
            }

            handler.Execute(command);
        }
    }
}
