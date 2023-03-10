using System.Data;
using System.Data.SqlClient;
using WallaShops.Data;
using WallaShops.Objects;

namespace UpdatingFeedBackStatus.DAL
{
  public sealed class OrdersDalSingleton : WSSqlHelper
  {
    #region Singleton initialization

    private static OrdersDalSingleton s_Instance = null;

    private OrdersDalSingleton() : base(WSPlatforms.WallaShops) { }

    public static OrdersDalSingleton Instance
    {
      get
      {
        if (s_Instance == null)
        {
          s_Instance = new OrdersDalSingleton();
        }

        return s_Instance;
      }
    }

    #endregion

    #region DAL operations

    public DataTable GetPendingClientsOrders()
    {
      return GetDataTable("getFeedBackListNewOrderSys");
    }

    public void UpdateOrderFeedBackInWSOrdersMainTable(int @order_id)
    {
      SqlParameter[] spParams = new SqlParameter[1];
      spParams[0] = new SqlParameter("@order_id", @order_id);
      ExecuteNonQuery("updateOrderFeedbackNewSys", ref spParams);
    }

    #endregion
  }
}