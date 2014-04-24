using System.IO;

namespace TFSUtility.Commands
{
    public interface ICommand
    {
        void Execute(TextWriter console);
    }
}
