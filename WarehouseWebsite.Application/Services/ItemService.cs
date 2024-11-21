﻿using System.Data;
using WarehouseWebsite.Application.Helpers;
using WarehouseWebsite.Application.Interfaces;
using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Interfaces;
using WarehouseWebsite.Domain.Interfaces.Repositories;
using WarehouseWebsite.Domain.Models.Items;

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
            return await _itemRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Item>> GetItemsByFilterAsync(
            FilterParameters<Item> filter, CancellationToken token)
        {
            return await _itemRepository.GetItemsByFilterAsync(filter, token);
        }

        public async Task<IEnumerable<MissingItem>> GetMissingItemsAsync(
            FilterParameters<MissingItem> filter, CancellationToken token)
        {
            return await _missingItemRepository.GetItemsByFilterAsync(filter, token);
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
                var cancellationSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));
                var token = cancellationSource.Token;
                await _unitOfWork.BeginTransactionAsync(IsolationLevel.RepeatableRead, token);

                var item = await _itemRepository.GetByIdAsync(id);

                if (item == null) return;

                if (item.Quantity == 0)
                {
                    var missingItem = await _missingItemRepository.GetByIdAsync(id);

                    if (missingItem != null)
                    {
                        if (missingItem.Missing > addQuantity)
                        {
                            addQuantity = 0;
                            missingItem.Missing -= addQuantity;
                        }
                        else
                        {
                            _missingItemRepository.Remove(missingItem);
                            addQuantity -= missingItem.Missing;
                        }
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