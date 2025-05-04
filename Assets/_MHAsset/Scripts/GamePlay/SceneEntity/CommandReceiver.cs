using System.Collections.Generic;
using MH.Command;
using MH.EnitySystem;

namespace MH
{
    [System.Serializable]
    public class CommandBinding
    {
        public string CommandName;
        public CommandSO[] Commands;
    }
    
    public class CommandReceiver : EntityComponent
    {
        
        public CommandBinding[] CommandBindings;

        private Dictionary<string, CommandBinding> _commandMap = new();

        public override void Initialized(BaseEnitity enitity)
        {
            base.Initialized(enitity);

            for (int i=0; i< CommandBindings.Length; i++)
            {
                var commandBinding = CommandBindings[i];
                if (commandBinding == null)
                {
                    continue;
                }

                if (_commandMap.ContainsKey(commandBinding.CommandName))
                {
                    continue;
                }

                _commandMap[commandBinding.CommandName] = commandBinding;
            }

        }

        public void TryExecuteCommand(string commandName)
        {
            if (_commandMap.ContainsKey(commandName))
            {
                var commandBinding = _commandMap[commandName];
                for (int i = 0; i < commandBinding.Commands.Length; i++)
                {
                    var command = commandBinding.Commands[i];
                    if (command == null)
                    {
                        continue;
                    }

                    command.Execute(_entity);
                }
            }

        }
    }
}