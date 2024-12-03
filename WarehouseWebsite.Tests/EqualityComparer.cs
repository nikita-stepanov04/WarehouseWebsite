using WarehouseWebsite.Domain.Models;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Domain.Models.Orders;

namespace WarehouseWebsite.Tests
{
    internal class ItemEqualityComparer : IEqualityComparer<Item>
    {
        public bool Equals(Item? x, Item? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            return x.Id == y.Id &&
                   x.Name == y.Name &&
                   x.Quantity == y.Quantity &&
                   x.Description == y.Description &&
                   x.Price == y.Price &&
                   x.Weight == y.Weight &&
                   x.Category == y.Category &&
                   x.PhotoBlobId == y.PhotoBlobId &&
                   x.IsRemoved == y.IsRemoved;
        }

        public int GetHashCode(Item obj)
        {
            return HashCode.Combine(obj.Id, obj.Name, obj.Quantity, obj.Description, obj.Price, obj.Weight, obj.Category);
        }
    }

    public class OrderEqualityComparer : IEqualityComparer<Order>
    {
        public bool Equals(Order? x, Order? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            return x.Id == y.Id &&
                   x.CustomerId == y.CustomerId &&
                   x.OrderTime == y.OrderTime &&
                   x.Status == y.Status &&
                   x.TotalPrice == y.TotalPrice &&
                   x.CustomerId == y.CustomerId;
        }

        public int GetHashCode(Order obj)
        {
            return HashCode.Combine(obj.Id, obj.CustomerId, obj.OrderTime, obj.Status, obj.TotalPrice, obj.Customer.Id);
        }
    }

    public class OrderItemEqualityComparer : IEqualityComparer<OrderItem>
    {
        public bool Equals(OrderItem? x, OrderItem? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            return x.Id == y.Id &&
                   x.ItemId == y.ItemId &&
                   x.OrderId == y.OrderId &&
                   x.Quantity == y.Quantity &&
                   x.Price == y.Price;
        }

        public int GetHashCode(OrderItem obj)
        {
            return HashCode.Combine(obj.Id, obj.ItemId, obj.OrderId, obj.Quantity, obj.Price);
        }
    }

    public class RefreshTokenEqualityComparer : IEqualityComparer<RefreshToken>
    {
        public bool Equals(RefreshToken? x, RefreshToken? y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;

            return x.Token == y.Token &&
                   x.Created == y.Created &&
                   x.Expires == y.Expires &&
                   x.IsRevoked == y.IsRevoked;
        }

        public int GetHashCode(RefreshToken obj)
        {
            return HashCode.Combine(obj.Token, obj.Created, obj.Expires, obj.IsRevoked);
        }
    }

}
