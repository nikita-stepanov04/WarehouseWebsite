using System.Data;
using WarehouseWebsite.Application.Helpers;
using WarehouseWebsite.Application.Interfaces;
using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Interfaces;
using WarehouseWebsite.Domain.Interfaces.Repositories;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Domain.Models.Orders;

namespace WarehouseWebsite.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemRepository _itemRepository;
        private readonly IMissingItemRepository _missingItemRepository;
        private readonly IImageRepository _imageRepository;

        public ItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _itemRepository = unitOfWork.ItemRepository;
            _missingItemRepository = unitOfWork.MissingItemRepository;
            _imageRepository = unitOfWork.ImageRepository;
        }

        public async Task AddItemAsync(Item item, Stream image)
        {
            var compressor = new ImageCompressor();
            await compressor.CompressImageInStreamAsync(image);

            Guid imageId = await _imageRepository.UploadAsync(image, "image/jpeg");
            item.PhotoBlobId = imageId;

            await _itemRepository.AddAsync(item);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Item?> GetByIdAsync(Guid id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item != null)
                item.PhotoUrl = _imageRepository.GetImageUri(item.PhotoBlobId);
            return item;
        }

        public async Task<IEnumerable<Item>> GetItemsByFilterAsync(
            FilterParameters<Item> filter, CancellationToken token)
        {
            var items = await _itemRepository.GetItemsByFilterAsync(filter, token);
            foreach(var item in items)
            {
                item.PhotoUrl = _imageRepository.GetImageUri(item.PhotoBlobId);
            }
            return items;
        }

        public async Task<IEnumerable<MissingItem>> GetMissingItemsAsync(
            FilterParameters<MissingItem> filter, CancellationToken token)
        {
            var missingItems = await _missingItemRepository.GetItemsByFilterAsync(filter, token);
            foreach(var missing in missingItems)
            {
                missing.Item.PhotoUrl = _imageRepository.GetImageUri(missing.Item.PhotoBlobId);
            }
            return missingItems;
        }

        public async Task RemoveItemByIdAsync(Guid id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item != null)
            {
                _itemRepository.Remove(item);
            }
            await _unitOfWork.SaveAsync();
        }

        public async Task RestockItemAsync(Guid id, int addQuantity)
        {
            try
            {
                var cancellationSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(2000));
                var token = cancellationSource.Token;
                //var token = CancellationToken.None;
                await _unitOfWork.BeginTransactionAsync(IsolationLevel.RepeatableRead, token);

                var item = await _itemRepository.GetByIdAsync(id);

                if (item == null) return;
                
                var missingItem = await _missingItemRepository.GetByItemIdNotPopulated(id);

                if (missingItem != null)
                {
                    if (missingItem.Missing > addQuantity)
                    {
                        missingItem.Missing -= addQuantity;
                        addQuantity = 0;
                    }
                    else
                    {
                        _missingItemRepository.Remove(missingItem);
                        addQuantity -= missingItem.Missing;
                    }
                }
                
                item.Quantity += addQuantity;

                await _unitOfWork.SaveAsync(token);
                await _unitOfWork.CommitTransactionAsync(token);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
