namespace PresentationLayer.UserInterfaces.Factories;

public interface IUserInterfaceFactory
{
  public Task<IUserInterface?> Create();
}