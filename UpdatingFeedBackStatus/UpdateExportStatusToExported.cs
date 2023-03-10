using System;
using System.Data;
using UpdatingFeedBackStatus.DAL;
using UpdatingFeedBackStatus.LogToConsole;
using WallaShops.Common.SalesForce;
using WallaShops.Common.SalesForce.Models;
using WallaShops.Utils;

namespace UpdatingFeedBackStatus
{
  public class UpdateExportStatusToExported
  {
    #region class fields

    private OrdersDalSingleton OrdersDal { get; } = OrdersDalSingleton.Instance;
    private DataTable ClientsOrdersDataTable { get; set; }

    #endregion

    #region CTOR

    public UpdateExportStatusToExported()
    {
      getClientsOrders();
    }

    private void getClientsOrders()
    {
      try
      {
        ClientsOrdersDataTable = OrdersDal.GetPendingClientsOrders();
      }
      catch (Exception ex)
      {
        Logger.LogToConsole(ex.Message);
      }
    }

    #endregion

    #region Execute cutomer feed back update process

    public void Execute()
    {
      byte minimunNumberOfRows = 0;
      int numberOfOrdersPendingForUpdate = ClientsOrdersDataTable.Rows.Count;

      try
      {
        Logger.LogToConsole($"Number of orders pending to update: {WSStringUtils.ToString(numberOfOrdersPendingForUpdate)}");
        if (numberOfOrdersPendingForUpdate > minimunNumberOfRows)
        {
          foreach (DataRow customerOrderRow in ClientsOrdersDataTable.Rows)
          {
            Logger.LogToConsole($"Updating now order number: {customerOrderRow["order_id"]}");
            setFeedBackReady(customerOrderRow);
          }
        }
      }
      catch (Exception ex)
      {
        Logger.LogToConsole(ex.Message);
      }
    }

    #endregion

    #region helper methods for executing cutomer feed back update process

    private void setFeedBackReady(DataRow customerOrderRow)
    {
      try
      {
        setOrderfeedBackReadyToOne(customerOrderRow);
      }
      catch (Exception ex)
      {
        Logger.LogToConsole(ex.Message);
      }
    }

    private void setOrderfeedBackReadyToOne(DataRow customerOrderRow) 
    {
      string sfOrderId = WSStringUtils.ToString(customerOrderRow["sales_force_order_no"]);
      int orderId = WSStringUtils.ToInt(customerOrderRow["order_id"]);

      SalesForceWebClient salesForceClient = new SalesForceWebClient();
      SalesForceOrder order = WSJsonConverter.DeserializeObject<SalesForceOrder>(salesForceClient.GetOrderDetails(sfOrderId).ToString());
      order.FeedbackReady = 1;
      salesForceClient.SetOrderDetails(sfOrderId, order, SalesForcePlatforms.WallaShops);
      OrdersDal.UpdateOrderFeedBackInWSOrdersMainTable(orderId);
      Logger.LogToConsole();
    }

    #endregion
  }
}