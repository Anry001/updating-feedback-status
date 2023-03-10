namespace UpdatingFeedBackStatus
{
  public class Program
  {
    public static void Main(string[] args)
    {
      UpdateExportStatusToExported updateExportStatusToExported = new UpdateExportStatusToExported();
      updateExportStatusToExported.Execute();
    }
  }
}