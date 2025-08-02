  public interface ICommand
    {
        void Execute();
        void Undo();
    }
    public class Light
    {
        public void On() => Console.WriteLine("Light is ON");
        public void Off() => Console.WriteLine("Light is OFF");
    }
    public class LightOnCommand : ICommand
    {
        private readonly Light _light;
    
        public LightOnCommand(Light light)
        {
            _light = light;
        }
    
        public void Execute()
        {
            _light.On();
        }
    
        public void Undo()
        {
            _light.Off();
        }
    }
    public class RemoteControl
    {
        private ICommand[] _commands = new ICommand[2];
        private ICommand _lastCommand;
    
        public RemoteControl()
        {
            ICommand noCommand = new NoCommand();
            for (int i = 0; i < _commands.Length; i++)
            {
                _commands[i] = noCommand;
            }
            _lastCommand = noCommand;
        }
    
        public void SetCommand(int slot, ICommand command)
        {
            _commands[slot] = command;
        }
    
        public void PressButton(int slot)
        {
            _commands[slot].Execute();
            _lastCommand = _commands[slot];
        }
    
        public void PressUndoButton()
        {
            Console.Write("Undo pressed: ");
            _lastCommand.Undo();
        }   
    }
    public class NoCommand : ICommand
    {
        public void Execute() { }
        public void Undo() { }
    }
