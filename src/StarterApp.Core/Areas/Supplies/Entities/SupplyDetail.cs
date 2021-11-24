using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Common;

namespace StarterApp.Core.Areas.Supplies.Entities
{
    public class SupplyDetail : AuditableEntity
    {
        #region Constructors

        public SupplyDetail()
        {
        }
        public SupplyDetail(long productId, int qty)
        {
            ProductId = productId;
            Quantity = qty;
        }

        #endregion

        #region Columns

        public long ProductId { get; private set; }
        public int Quantity { get; private set; }
        public Product Product { get; private set; }
        public SupplyHeader SupplyHeader { get; private set; }

        #endregion

        #region Domain Methods

        public void Update(long productId, int qty)
        {
            ProductId = productId;
            Quantity = qty;
        }

        #endregion
    }
}
