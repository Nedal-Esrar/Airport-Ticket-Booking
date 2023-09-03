namespace PresentationLayer.CommandInvokers;

public interface ICommandInvoker
{
  Task Invoke(int choice);
}